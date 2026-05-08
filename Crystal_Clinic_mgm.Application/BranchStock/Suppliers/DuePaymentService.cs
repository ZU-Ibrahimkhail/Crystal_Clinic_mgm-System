using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Update;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.Services.Repositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using ImageMagick;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.Suppliers
{
    public class CreateDuePaymentCommand : IRequest<JsonResult>
    {
        public int SupplierDueId { get; set; }
        public int? CurrencyTypeId { get; set; }
        public decimal ExchangeRateToDueCurrency { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountInDueCurrency { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Remarks { get; set; }
        public List<string>? Attachment { get; set; } = [];
    }
    public class CreateDuePaymentCommandValidator : AbstractValidator<CreateDuePaymentCommand>
    {
        public CreateDuePaymentCommandValidator()
        {
            RuleFor(x => x.AmountPaid)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Amount paid cannot be negative.");
            RuleFor(x => x.ExchangeRateToDueCurrency)
                .GreaterThan(0)
                .WithMessage("Exchange rate must be positive.");
        }
    }
    public class CreateDuePaymentHandler(ERP_DbContext context, ILoggedInUser loggedInUser, IMessage message)
        : IRequestHandler<CreateDuePaymentCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(CreateDuePaymentCommand request, CancellationToken cancellationToken)
        {
            var validations = new CreateDuePaymentCommandValidator().Validate(request).Errors;
            if (validations.Any())
            {
                message.CheckCCValidationError(validations);
            }
            var due = await context.SupplierDue.FindAsync(request.SupplierDueId, cancellationToken);
            if (due == null || due.IsDeleted)
            {
                return message.RecordNotFound($"Due not fount with id {request.SupplierDueId}");
            }
            var mainAccount = context.MainAccount.FirstOrDefault(x => !x.IsDeleted && x.CurrencyTypeId == request.CurrencyTypeId && x.OwnerUserId == loggedInUser.Id);
            if (mainAccount == null || mainAccount.IsDeleted)
            {
                return message.RecordNotFound($"No account with currency Id {request.CurrencyTypeId} found");
            }
            var entity = new DuePayment
            {
                SupplierDueId = request.SupplierDueId,
                CurrencyTypeId = request.CurrencyTypeId,
                ExchangeRateToDueCurrency = request.ExchangeRateToDueCurrency,
                AmountPaid = request.AmountPaid,
                AmountInDueCurrency = request.AmountInDueCurrency,
                paymentDate = request.PaymentDate,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUser.Id
            };

            #region Update Main Asset Record
            mainAccount.TotalDebitAmount += Convert.ToDouble(request.AmountPaid);
            mainAccount.BalanceAmount -= Convert.ToDouble(request.AmountPaid);
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = loggedInUser.Id;
            context.MainAccount.Update(mainAccount);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(DuePayment)}: {request.Remarks}",
                UserId = loggedInUser.Id,
                DebitAmount = Convert.ToDouble(request.AmountPaid),
                CreditAmount = 0,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.EXPENSE,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            #endregion
            
            entity.AttachmentPath = request.Attachment;
            entity.Remarks = request.Remarks;
            due.PaidAmount += request.AmountInDueCurrency;
            due.RemainAmount = due.DueAmount - due.PaidAmount;
            context.SupplierDue.Update(due);
            context.AccountTracking.Add(AccountTracking);
            context.DuePayment.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
            return message.Saved(entity);
        }
    }


    public class UpdateDuePaymentCommand : IRequest<bool>
    {
        public int DuePaymentId { get; set; }
        public int SupplierDueId { get; set; }
        public int? CurrencyTypeId { get; set; }
        public decimal ExchangeRateToDueCurrency { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountInDueCurrency { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Remarks { get; set; }
        public List<string>? Attachment { get; set; } = [];
    }
    public class UpdateDuePaymentHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<UpdateDuePaymentCommand, bool>
    {
        public async Task<bool> Handle(UpdateDuePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.DuePayment.FindAsync(request.DuePaymentId);

            if (entity == null || entity.IsDeleted)
                return false;
            var due = await context.SupplierDue.FindAsync(request.SupplierDueId, cancellationToken) ?? throw new KeyNotFoundException($"Due not fount with id {request.SupplierDueId}");

            var oldAmount = Convert.ToDouble(entity.AmountPaid);
            MainAccount newMainAcount;
            var mainAccount = context.MainAccount.Where(x => !x.IsDeleted && x.CurrencyTypeId == entity.CurrencyTypeId && x.OwnerUserId == loggedInUser.Id).FirstOrDefault() ?? throw new KeyNotFoundException($"No account with currency Id {request.CurrencyTypeId} found");
            if (mainAccount.BalanceAmount < Convert.ToDouble(request.AmountPaid))
                throw new InvalidOperationException("Insufficient balance in main account.");
            if (due.RemainAmount < request.AmountPaid)
                throw new InvalidOperationException("Amount can not exceed the remain amount " + due.RemainAmount);

            mainAccount.TotalDebitAmount -= Convert.ToDouble(entity.AmountPaid);
            mainAccount.BalanceAmount += Convert.ToDouble(entity.AmountPaid);
            if (entity.CurrencyTypeId != request.CurrencyTypeId)
            {
                mainAccount.TotalDebitAmount -= Convert.ToDouble(entity.AmountPaid);
                mainAccount.BalanceAmount += Convert.ToDouble(entity.AmountPaid);
                context.MainAccount.Update(mainAccount);
                context.AccountTracking.Add(new AccountTracking
                {
                    CurrencyTypeId = mainAccount.CurrencyTypeId,
                    TransactionDate = DateTime.UtcNow,
                    Description = $"{nameof(DuePayment)}_Reversed: {request.Remarks}",
                    UserId = loggedInUser.Id,
                    DebitAmount = 0,
                    CreditAmount = Convert.ToDouble(entity.AmountPaid),
                    BalanceAmount = mainAccount.BalanceAmount,
                    MainAccountId = mainAccount.ID,
                    trackType = TrackType.EXPENSE,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                });

                newMainAcount = await context.MainAccount.FirstOrDefaultAsync(x => !x.IsDeleted && x.CurrencyTypeId == request.CurrencyTypeId && x.OwnerUserId == loggedInUser.Id, cancellationToken)
                    ?? throw new KeyNotFoundException($"No account with currency Id {request.CurrencyTypeId} found");
            }
            else
            {
                newMainAcount = mainAccount;
            }
            due.PaidAmount -= request.AmountInDueCurrency;

            entity.SupplierDueId = request.SupplierDueId;
            entity.CurrencyTypeId = request.CurrencyTypeId;
            entity.ExchangeRateToDueCurrency = request.ExchangeRateToDueCurrency;
            entity.AmountPaid = request.AmountPaid;
            entity.AmountInDueCurrency = request.AmountInDueCurrency;
            entity.paymentDate = request.PaymentDate;
            entity.ModifiedOn = DateTime.UtcNow;
            entity.ModifiedBy = loggedInUser.Id;
            string FilePath = "";
            entity.Remarks = request.Remarks;

            entity.AttachmentPath = request.Attachment;

            due.PaidAmount += request.AmountInDueCurrency;
            due.RemainAmount = due.DueAmount - due.PaidAmount;
            context.SupplierDue.Update(due);


            #region Update Main Asset Record
            newMainAcount.TotalDebitAmount += Convert.ToDouble(entity.AmountPaid);
            newMainAcount.BalanceAmount -= Convert.ToDouble(entity.AmountPaid);
            newMainAcount.ModifiedOn = DateTime.Now;
            newMainAcount.ModifiedBy = loggedInUser.Id;
            context.MainAccount.Update(newMainAcount);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = newMainAcount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(DuePayment)}_Update: {request.Remarks}",
                UserId = entity.CreatedBy,
                DebitAmount = Convert.ToDouble(entity.AmountPaid),
                CreditAmount = 0,
                BalanceAmount = newMainAcount.BalanceAmount,
                MainAccountId = newMainAcount.ID,
                trackType = TrackType.EXPENSE,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            context.AccountTracking.Add(AccountTracking);
            #endregion
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }


    public class DeleteDuePaymentCommand : IRequest<bool>
    {
        public int DuePaymentId { get; set; }
        public string? Remarks { get; set; }
    }
    public class DeleteDuePaymentHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<DeleteDuePaymentCommand, bool>
    {
        public async Task<bool> Handle(DeleteDuePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.DuePayment.FindAsync(request.DuePaymentId);
            if (entity == null || entity.IsDeleted)
                return false;

            entity.IsDeleted = true;
            context.DuePayment.Update(entity);

            var mainAccount = context.MainAccount.Where(x => !x.IsDeleted && x.CurrencyTypeId == entity.CurrencyTypeId && x.OwnerUserId == loggedInUser.Id).FirstOrDefault() ?? throw new KeyNotFoundException($"No account with currency Id {entity.CurrencyTypeId} found");
            mainAccount.TotalDebitAmount -= Convert.ToDouble(entity.AmountPaid);
            mainAccount.BalanceAmount += Convert.ToDouble(entity.AmountPaid);

            #region Add Asset Tracking Record
            AccountTracking UAccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(DuePayment)}_Deleted: {request.Remarks ?? "Payment deleted"}",
                UserId = entity.CreatedBy,
                DebitAmount = 0,
                CreditAmount = Convert.ToDouble(entity.AmountPaid),
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.EXPENSE,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            context.AccountTracking.Add(UAccountTracking);
            #endregion

            context.MainAccount.Update(mainAccount);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }


    public class GetDuePaymentByIdQuery : IRequest<DuePayment?>
    {
        public int DuePaymentId { get; set; }
    }
    public class GetDuePaymentByIdHandler(ERP_DbContext context)
        : IRequestHandler<GetDuePaymentByIdQuery, DuePayment?>
    {
        public async Task<DuePayment?> Handle(GetDuePaymentByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.DuePayment
                .Include(dp => dp.CurrencyType)
                .Where(dp => !dp.IsDeleted)
                .FirstOrDefaultAsync(dp => dp.DuePaymentId == request.DuePaymentId, cancellationToken);
        }
    }


    public class GetAllDuePaymentsQuery : IRequest<List<DuePaymentDTO>>
    {
        public string? SearchBy { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
    }
    public class GetAllDuePaymentsHandler(ERP_DbContext context)
        : IRequestHandler<GetAllDuePaymentsQuery, List<DuePaymentDTO>>
    {
        public async Task<List<DuePaymentDTO>> Handle(GetAllDuePaymentsQuery request, CancellationToken cancellationToken)
        {
            var data = context.DuePayment
                .Where(dp => !dp.IsDeleted)
                .Include(x=>x.SupplierDue)
                .Include(x=>x.SupplierDue!.Supplier)
                .OrderByDescending(dp => dp.DuePaymentId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                data = data.Where(dp => dp.SupplierDue != null && dp.SupplierDue.Supplier != null
                && dp.SupplierDue.Supplier.Name.Contains(request.SearchBy));
            }

            if (request.LastId.HasValue)
            {
                data = data.Where(dp => dp.DuePaymentId < request.LastId);
            }

            return await data.Take(request.PageSize).Select(x => new DuePaymentDTO(x)).ToListAsync(cancellationToken);
        }
    }
}