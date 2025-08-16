using System.Linq.Expressions;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.Services.Repositories;
using Crystal_Clinic_Mgm.Application.Look.BranchDetail;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using ImageMagick;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands.GetApprovedTransactionsQueryHandler;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands
{
    #region Initiate a transaction
    public class InitiateTransferCommand : IRequest<Result>
    {
        public Guid FromAccountId { get; set; }
        public Guid ToUserId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class InitiateTransferCommandHandler(ERP_DbContext accountService, ILoggedInUser loggedInUser) : IRequestHandler<InitiateTransferCommand, Result>
    {
        public async Task<Result> Handle(InitiateTransferCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return Result.Fail("Invalid transfer amount.");

            var fromAccount = await accountService.MainAccount.FindAsync(request.FromAccountId);
            if (fromAccount == null)
                return Result.Fail("Account Not found:" + request.FromAccountId);

            if (fromAccount.BalanceAmount < request.Amount)
                return Result.Fail("Insufficient balance.");

            var transaction = new AccountTracking
            {
                UserId = loggedInUser.Id,
                MainAccountId = fromAccount.ID,
                DebitAmount = request.Amount,
                CurrencyTypeId = fromAccount.CurrencyTypeId,
                BalanceAmount = fromAccount.BalanceAmount - request.Amount,
                TransactionDate = DateTime.UtcNow,
                Description = request.Description,
                trackType = TrackType.TRANSFER,
                transactionStatus = TransactionStatus.PENDING,
                toUserId = request.ToUserId,
                fromUserId = loggedInUser.Id,
                CreatedOn = DateTime.Now,
                CreatedBy = loggedInUser.Id
            };

            await accountService.AccountTracking.AddAsync(transaction, cancellationToken);
            await accountService.SaveChangesAsync();

            return Result.Success(transaction.ID);
        }
    }
    #endregion

    #region Update the Transaction

    public class UpdateTransactionCommand : IRequest<Result>
    {
        public int TransactionId { get; set; }
        public double? NewAmount { get; set; }
        public Guid? NewToUserId { get; set; }
    }

    public class UpdateTransactionCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateTransactionCommand, Result>
    {
        public async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transactionEntry = await context.AccountTracking.FindAsync(request.TransactionId);
            if (transactionEntry == null || transactionEntry.IsDeleted || transactionEntry.transactionStatus != TransactionStatus.PENDING)
                return Result.Fail("Transaction not found.");

            if (transactionEntry.UserId != loggedInUser.Id)
                return Result.Fail("You are not authorized to update this transaction.");

            if (transactionEntry.transactionStatus != TransactionStatus.PENDING)
                return Result.Fail("Transaction must be in pending status to be updated.");

            // Update the transaction details
            if (request.NewAmount.HasValue)
            {
                transactionEntry.DebitAmount = request.NewAmount.Value;
                transactionEntry.BalanceAmount = transactionEntry.BalanceAmount + (transactionEntry.DebitAmount - transactionEntry.DebitAmount); // Adjust balance logic if needed
            }

            if (request.NewToUserId.HasValue)
            {
                transactionEntry.toUserId = request.NewToUserId.Value;
            }

            context.AccountTracking.Update(transactionEntry);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
    #endregion

    #region Confirm the Transaction

    public class ConfirmTransferCommand : IRequest<Result>
    {
        public int TransactionId { get; set; }
    }

    public class ConfirmTransferCommandHandler(ERP_DbContext context, UMS_DbContext umsContext, ILoggedInUser loggedInUser) : IRequestHandler<ConfirmTransferCommand, Result>
    {
        public async Task<Result> Handle(ConfirmTransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var transactionEntry = await context.AccountTracking.FindAsync(request.TransactionId);
                if (transactionEntry == null || transactionEntry.toUserId == null || transactionEntry.fromUserId == null)
                    return Result.Fail("Transaction not found.");

                if (transactionEntry.transactionStatus != TransactionStatus.APPROVED)
                    return Result.Fail("Transaction cannot be confirmed.");

                var toAccount = await context.MainAccount
                    .Where(x => x.OwnerUserId == transactionEntry.toUserId && x.CurrencyTypeId == transactionEntry.CurrencyTypeId)
                    .FirstOrDefaultAsync(cancellationToken);

                var fromAccount = await context.MainAccount
                    .Where(x => x.ID == transactionEntry.MainAccountId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (fromAccount == null)
                    return Result.Fail("Source Account Not found");

                var executionStrategy = context.Database.CreateExecutionStrategy();

                await executionStrategy.ExecuteAsync(async () =>
                {
                    if (toAccount == null)
                    {
                        int branchId = umsContext.Users.Find(transactionEntry.toUserId)?.BranchId ?? loggedInUser.BranchId;
                        toAccount = new MainAccount
                        {
                            OwnerUserId = transactionEntry.toUserId.Value,
                            CurrencyTypeId = transactionEntry.CurrencyTypeId,
                            Description = $"Account Created Via a Transfer from user ID: {transactionEntry.fromUserId}",
                            Code = $"{DateTime.Now:yyyyMMdd}_{Random.Shared.Next(1111, 9999)}",
                            BranchId = branchId,
                            DepositDate = DateTime.Now,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now,
                            CreatedBy = loggedInUser.Id,
                        };
                        context.MainAccount.Add(toAccount);
                        context.SaveChanges();
                    }

                    // Update balances
                    toAccount.BalanceAmount += transactionEntry.DebitAmount;
                    toAccount.TotalCreditAmount += transactionEntry.DebitAmount;
                    transactionEntry.transactionStatus = TransactionStatus.COMPLETED;
                    fromAccount.BalanceAmount -= transactionEntry.DebitAmount;
                    fromAccount.TotalDebitAmount += transactionEntry.DebitAmount;

                    var accountTracking = new AccountTracking
                    {
                        CurrencyTypeId = transactionEntry.CurrencyTypeId,
                        TransactionDate = DateTime.Now,
                        Description = $"Credit via transfer from user ID: {transactionEntry.fromUserId}",
                        UserId = transactionEntry.toUserId.Value,
                        DebitAmount = 0,
                        CreditAmount = transactionEntry.DebitAmount,
                        BalanceAmount = toAccount.BalanceAmount,
                        transactionStatus = TransactionStatus.COMPLETED,
                        fromUserId = transactionEntry.fromUserId,
                        toUserId = transactionEntry.toUserId,
                        approvedBy = transactionEntry.approvedBy,
                        MainAccountId = toAccount.ID,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };

                    context.MainAccount.Update(fromAccount);
                    context.MainAccount.Update(toAccount);
                    context.AccountTracking.Update(transactionEntry);
                    await context.AccountTracking.AddAsync(accountTracking, cancellationToken);

                    await context.SaveChangesAsync(cancellationToken);

                });
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during the transaction. {ex.Message}");
            }
        }
    }
    #endregion

    #region Approve the Transaction

    public class ApproveTransferCommand : IRequest<Result>
    {
        public int TransactionId { get; set; }
    }

    public class ApproveTransferCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ApproveTransferCommand, Result>
    {
        public async Task<Result> Handle(ApproveTransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var transactionEntry = await context.AccountTracking.FindAsync(request.TransactionId);
                if (transactionEntry == null || transactionEntry.toUserId == null || transactionEntry.fromUserId == null)
                    return Result.Fail("Transaction not found.");

                if (transactionEntry.transactionStatus != TransactionStatus.PENDING)
                    return Result.Fail("Transaction cannot be confirmed.");



                var executionStrategy = context.Database.CreateExecutionStrategy();

                await executionStrategy.ExecuteAsync(async () =>
                {
                    transactionEntry.transactionStatus = TransactionStatus.APPROVED;
                    transactionEntry.approvedBy = loggedInUser.Id;
                    transactionEntry.ModifiedOn = DateTime.Now;
                    transactionEntry.ModifiedBy = loggedInUser.Id;
                    context.AccountTracking.Update(transactionEntry);

                    await context.SaveChangesAsync(cancellationToken);

                });
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during the transaction. {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Pending Transactions
    public class GetPendingTransactionQuery : IRequest<AccountTrackingListResponse>
    {
        public string? SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetTransactionStatusQueryHandler(ERP_DbContext context, IGeneralHelperRepositoryAsync helper, ILoggedInUser loggedInUser, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetPendingTransactionQuery, AccountTrackingListResponse>
    {

        public async Task<AccountTrackingListResponse> Handle(GetPendingTransactionQuery request, CancellationToken cancellationToken)
        {
            Localization localization = new(httpContextAccessor);
            Expression<Func<AccountTracking, bool>> predicate = (x => true);
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                predicate = (b => b.Description!.Contains(request.SearchText) ||
                                          b.CurrencyType!.EnglishName.Contains(request.SearchText) ||
                                          b.CurrencyType!.DariName.Contains(request.SearchText) ||
                                          b.CurrencyType!.PashtoName.Contains(request.SearchText) ||
                                          b.DebitAmount.ToString() == request.SearchText);
            }
            var BranchAccounts = context.MainAccount.Where(x => loggedInUser.IsSuperAdmin || (loggedInUser.IsBranchAdmin && x.BranchId == loggedInUser.BranchId)).Select(x => x.ID).ToList();

            var query = context.AccountTracking.Where(x => BranchAccounts.Contains(x.MainAccountId) && x.trackType == TrackType.TRANSFER && x.transactionStatus == TransactionStatus.PENDING).Include(x => x.CurrencyType).Where(predicate).AsQueryable();

            // Get total count for pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new TransferTransactionDto(x, helper, localization)).ToListAsync(cancellationToken);

            return new AccountTrackingListResponse { Transactions = transactions, TotalCount = totalCount };
        }
    }
    #endregion

    #region Get Approved Transactions
    public class GetApprovedTransactionsQuery : IRequest<AccountTrackingListResponse>
    {
        public string? SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetApprovedTransactionsQueryHandler(ERP_DbContext context, IGeneralHelperRepositoryAsync helper, ILoggedInUser loggedInUser, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetApprovedTransactionsQuery, AccountTrackingListResponse>
    {
        public async Task<AccountTrackingListResponse> Handle(GetApprovedTransactionsQuery request, CancellationToken cancellationToken)
        {
            Localization localization = new(httpContextAccessor);

            Expression<Func<AccountTracking, bool>> predicate = (x => true);
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                predicate = (b => b.Description!.Contains(request.SearchText) ||
                                          b.CurrencyType!.EnglishName.Contains(request.SearchText) ||
                                          b.CurrencyType!.DariName.Contains(request.SearchText) ||
                                          b.CurrencyType!.PashtoName.Contains(request.SearchText) ||
                                          b.DebitAmount.ToString() == request.SearchText);
            }
            var BranchAccounts = context.MainAccount.Where(x => loggedInUser.IsSuperAdmin || (loggedInUser.IsBranchAdmin && x.BranchId == loggedInUser.BranchId)).Select(x => x.ID).ToList();

            var query = context.AccountTracking
                .Where(x => (x.toUserId == loggedInUser.Id || BranchAccounts.Contains(x.MainAccountId)) && x.trackType == TrackType.TRANSFER && x.transactionStatus == TransactionStatus.APPROVED)
                .Include(x => x.CurrencyType)
                .Where(predicate).AsQueryable();

            // Get total count for pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new TransferTransactionDto(x, helper, localization)).ToListAsync(cancellationToken);

            return new AccountTrackingListResponse { Transactions = transactions, TotalCount = totalCount };
        }

    }
    #endregion

    #region DTO
    public class AccountTrackingListResponse
    {
        public List<TransferTransactionDto> Transactions { get; set; } = [];
        public int TotalCount { get; set; }
    }
    public class TransferTransactionDto(AccountTracking transaction, IGeneralHelperRepositoryAsync helper, Localization localization)
    {
        public int ID { get; set; } = transaction.ID;
        public int CurrencyTypeId { get; set; } = transaction.CurrencyTypeId;
        public string? CurrencyType { get; set; } = localization.GetName(transaction.CurrencyType);
        public DateTime TransactionDate { get; set; } = transaction.TransactionDate;
        public string? Description { get; set; } = transaction.Description;
        public Guid UserId { get; set; } = transaction.UserId;
        public double DebitAmount { get; set; } = transaction.DebitAmount;
        public Guid MainAccountId { get; set; }
        public string MainAccountCode { get; set; } = transaction.MainAccount?.Code ?? string.Empty;
        public TransactionStatus transactionStatus { get; set; } = transaction.transactionStatus;
        public Guid? approvedByUserId { get; set; } = transaction.approvedBy;
        public string? approvedByUserName { get; set; } = helper.GetUserName(localization.language, transaction.approvedBy);
        public Guid? toUserId { get; set; } = transaction.fromUserId;
        public string? toUserName { get; set; } = helper.GetUserName(localization.language, transaction.toUserId);
        public Guid? fromUserId { get; set; } = transaction.toUserId;
        public string? fromUserName { get; set; } = helper.GetUserName(localization.language, transaction.fromUserId);

    }
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public object? Value { get; }

        protected Result(bool isSuccess, string? error, object? value)
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
        }

        public static Result Success(object? value = null)
        {
            return new Result(true, null, value);
        }

        public static Result Fail(string error)
        {
            return new Result(false, error, null);
        }
    }
    #endregion
}
