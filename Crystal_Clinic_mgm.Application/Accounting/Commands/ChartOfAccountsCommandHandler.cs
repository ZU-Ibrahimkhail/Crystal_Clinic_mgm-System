using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Chart of Accounts
    public class CreateChartOfAccountsCommand : IRequest<Result>
    {
        public CreateChartOfAccountsDto Dto { get; set; } = null!;
    }

    public class CreateChartOfAccountsCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateChartOfAccountsCommand, Result>
    {
        public async Task<Result> Handle(CreateChartOfAccountsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingAccount = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(c => c.AccountCode == request.Dto.AccountCode && !c.IsDeleted, cancellationToken);

                if (existingAccount != null)
                    return Result.Fail("Account code already exists.");

                var account = new ChartOfAccounts
                {
                    AccountCode = request.Dto.AccountCode,
                    AccountName = request.Dto.AccountName,
                    AccountType = request.Dto.AccountType,
                    AccountCategory = request.Dto.AccountCategory,
                    NormalBalance = request.Dto.NormalBalance,
                    IsSystemAccount = false,
                    IsActive = true,
                    Description = request.Dto.Description ?? string.Empty,
                    ParentAccountId = request.Dto.ParentAccountId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.ChartOfAccounts.Add(account);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(account.Id, "Chart of Accounts created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Chart of Accounts: {ex.Message}");
            }
        }
    }
    #endregion

    #region Update Chart of Accounts
    public class UpdateChartOfAccountsCommand : IRequest<Result>
    {
        public UpdateChartOfAccountsDto Dto { get; set; } = null!;
    }

    public class UpdateChartOfAccountsCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateChartOfAccountsCommand, Result>
    {
        public async Task<Result> Handle(UpdateChartOfAccountsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(c => c.Id == request.Dto.Id && !c.IsDeleted, cancellationToken);

                if (account == null)
                    return Result.Fail("Chart of Accounts not found.");

                account.AccountName = request.Dto.AccountName;
                account.IsActive = request.Dto.IsActive;
                account.Description = request.Dto.Description ?? account.Description;
                account.ModifiedBy = loggedInUser.Id;
                account.ModifiedOn = DateTime.UtcNow;

                context.ChartOfAccounts.Update(account);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Chart of Accounts updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating Chart of Accounts: {ex.Message}");
            }
        }
    }
    #endregion

    #region Delete Chart of Accounts
    public class DeleteChartOfAccountsCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteChartOfAccountsCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeleteChartOfAccountsCommand, Result>
    {
        public async Task<Result> Handle(DeleteChartOfAccountsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

                if (account == null)
                    return Result.Fail("Chart of Accounts not found.");

                if (account.IsSystemAccount)
                    return Result.Fail("System accounts cannot be deleted.");

                var hasTransactions = await context.GeneralLedgers
                    .AnyAsync(g => g.ChartOfAccountId == request.Id && !g.IsDeleted, cancellationToken);

                if (hasTransactions)
                    return Result.Fail("Cannot delete account with existing transactions. Deactivate instead.");

                account.IsDeleted = true;
                account.ModifiedBy = loggedInUser.Id;
                account.ModifiedOn = DateTime.UtcNow;

                context.ChartOfAccounts.Update(account);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Chart of Accounts deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error deleting Chart of Accounts: {ex.Message}");
            }
        }
    }
    #endregion

    #region Move Chart of Accounts
    public class MoveChartOfAccountCommand : IRequest<Result>
    {
        public int AccountId { get; set; }
        public int? NewParentAccountId { get; set; }
    }

    public class MoveChartOfAccountCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<MoveChartOfAccountCommand, Result>
    {
        public async Task<Result> Handle(MoveChartOfAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(c => c.Id == request.AccountId && !c.IsDeleted, cancellationToken);

                if (account == null)
                    return Result.Fail("Chart of Accounts not found.");

                if (account.IsSystemAccount)
                    return Result.Fail("System accounts cannot be moved.");

                if (request.NewParentAccountId.HasValue)
                {
                    var parentAccount = await context.ChartOfAccounts
                        .FirstOrDefaultAsync(c => c.Id == request.NewParentAccountId && !c.IsDeleted, cancellationToken);

                    if (parentAccount == null)
                        return Result.Fail("Parent account not found.");

                    if (parentAccount.Id == account.Id)
                        return Result.Fail("An account cannot be its own parent.");
                }

                account.ParentAccountId = request.NewParentAccountId;
                account.ModifiedBy = loggedInUser.Id;
                account.ModifiedOn = DateTime.UtcNow;

                context.ChartOfAccounts.Update(account);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Account moved successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error moving account: {ex.Message}");
            }
        }
    }
    #endregion
}
