using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Update;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.Services.Repositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using ImageMagick;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.Suppliers
{
    public class CreateDuePaymentCommand : IRequest<int>
    {
        public int SupplierDueId { get; set; }
        public int? CurrencyTypeId { get; set; }
        public decimal ExchangeRateToDueCurrency { get; set; }
        public decimal AmmountPaid { get; set; }
        public decimal AmountInDueCurrency { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Remarks { get; set; }
        public IFormFile? Attachment { get; set; }
    }

    public class UpdateDuePaymentCommand : IRequest<bool>
    {
        public int DuePaymentId { get; set; }
        public int SupplierDueId { get; set; }
        public int? CurrencyTypeId { get; set; }
        public decimal ExchangeRateToDueCurrency { get; set; }
        public decimal AmmountPaid { get; set; }
        public decimal AmountInDueCurrency { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Remarks { get; set; }
        public IFormFile? Attachment { get; set; }
    }

    public class DeleteDuePaymentCommand : IRequest<bool>
    {
        public int DuePaymentId { get; set; }
        public object Remarks { get; internal set; }
    }

    public class GetDuePaymentByIdQuery : IRequest<DuePayment?>
    {
        public int DuePaymentId { get; set; }
    }

    public class GetAllDuePaymentsQuery : IRequest<List<DuePaymentDTO>>
    {
        public string? SearchBy { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
    }

    public class CreateDuePaymentHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<CreateDuePaymentCommand, int>
    {
        public async Task<int> Handle(CreateDuePaymentCommand request, CancellationToken cancellationToken)
        {
            var due = await context.SupplierDue.FindAsync(request.SupplierDueId, cancellationToken) ?? throw new KeyNotFoundException($"Due not fount with id {request.SupplierDueId}");
            var mainAccount = context.MainAccount.FirstOrDefault(x => !x.IsDeleted && x.CurrencyTypeId == request.CurrencyTypeId && x.OwnerUserId == loggedInUser.Id) ?? throw new KeyNotFoundException($"No account with currency Id {request.CurrencyTypeId} found");
            var entity = new DuePayment
            {
                SupplierDueId = request.SupplierDueId,
                CurrencyTypeId = request.CurrencyTypeId,
                ExchangeRateToDueCurrency = request.ExchangeRateToDueCurrency,
                AmmountPaid = request.AmmountPaid,
                AmountInDueCurrency = request.AmountInDueCurrency,
                paymentDate = request.PaymentDate,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUser.Id
            };

            #region Update Main Asset Record
            mainAccount.TotalDebitAmount += Convert.ToDouble(request.AmmountPaid);
            mainAccount.BalanceAmount -= Convert.ToDouble(request.AmmountPaid);
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
                DebitAmount = Convert.ToDouble(request.AmmountPaid),
                CreditAmount = 0,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.EXPENSE,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            #endregion
            string FilePath = "";

            if (request.Attachment != null)
            {
                var attachment = request.Attachment;
                FileHandler _sotrage = new();
                if (attachment.FileName.Length > 0)
                {
                    string ext = Path.GetExtension(attachment.FileName);
                    FilePath = await _sotrage.CreateAsync(attachment.OpenReadStream(), ext, "wwwroot", AppConfig.Archive_ArchivedDocuments);
                }
            }
            entity.AttachmentPath = FilePath;
            due.PaidAmount += request.AmountInDueCurrency;
            due.RemainAmount = due.DueAmount - due.PaidAmount;
            context.SupplierDue.Update(due);
            context.AccountTracking.Add(AccountTracking);
            context.DuePayment.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity.DuePaymentId;
        }
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
            var oldAmount = Convert.ToDouble(entity.AmmountPaid);
            MainAccount newMainAcount;
            var mainAccount = context.MainAccount.Where(x => !x.IsDeleted && x.CurrencyTypeId == entity.CurrencyTypeId && x.OwnerUserId == loggedInUser.Id).FirstOrDefault() ?? throw new KeyNotFoundException($"No account with currency Id {request.CurrencyTypeId} found");

            mainAccount.TotalDebitAmount -= Convert.ToDouble(entity.AmmountPaid);
            mainAccount.BalanceAmount += Convert.ToDouble(entity.AmmountPaid);
            if (entity.CurrencyTypeId != request.CurrencyTypeId)
            {
                #region Add Asset Tracking Record
                AccountTracking UAccountTracking = new()
                {
                    CurrencyTypeId = mainAccount.CurrencyTypeId,
                    TransactionDate = DateTime.Now,
                    Description = $"{nameof(DuePayment)}_Update: {request.Remarks}",
                    UserId = entity.CreatedBy,
                    DebitAmount = 0,
                    CreditAmount = oldAmount,
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
                newMainAcount = context.MainAccount.Where(x => !x.IsDeleted && x.CurrencyTypeId == request.CurrencyTypeId && x.OwnerUserId == loggedInUser.Id).FirstOrDefault() ?? throw new KeyNotFoundException($"No account with currency Id {request.CurrencyTypeId} found");

            }
            else
            {
                newMainAcount = mainAccount;
            }
            due.PaidAmount -= request.AmountInDueCurrency;

            entity.SupplierDueId = request.SupplierDueId;
            entity.CurrencyTypeId = request.CurrencyTypeId;
            entity.ExchangeRateToDueCurrency = request.ExchangeRateToDueCurrency;
            entity.AmmountPaid = request.AmmountPaid;
            entity.AmountInDueCurrency = request.AmountInDueCurrency;
            entity.paymentDate = request.PaymentDate;
            entity.ModifiedOn = DateTime.UtcNow;
            entity.ModifiedBy = loggedInUser.Id;
            string FilePath = "";

            if (request.Attachment != null)
            {
                var attachment = request.Attachment;
                FileHandler _sotrage = new();
                if (attachment.FileName.Length > 0)
                {
                    await _sotrage.RemoveFile("wwwroot", entity.AttachmentPath);
                    string ext = Path.GetExtension(attachment.FileName);
                    FilePath = await _sotrage.CreateAsync(attachment.OpenReadStream(), ext, "wwwroot", AppConfig.Archive_ArchivedDocuments);
                }
            }
            entity.AttachmentPath = FilePath;

            due.PaidAmount += request.AmountInDueCurrency;
            due.RemainAmount = due.DueAmount - due.PaidAmount;
            context.SupplierDue.Update(due);

            #region Update Main Asset Record
            newMainAcount.TotalDebitAmount += Convert.ToDouble(entity.AmmountPaid);
            newMainAcount.BalanceAmount -= Convert.ToDouble(entity.AmmountPaid);
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
                DebitAmount = Convert.ToDouble(entity.AmmountPaid),
                CreditAmount = oldAmount,
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
            mainAccount.TotalDebitAmount -= Convert.ToDouble(entity.AmmountPaid);
            mainAccount.BalanceAmount += Convert.ToDouble(entity.AmmountPaid);

            #region Add Asset Tracking Record
            AccountTracking UAccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(DuePayment)}_Update: {request.Remarks}",
                UserId = entity.CreatedBy,
                DebitAmount = 0,
                CreditAmount = Convert.ToDouble(entity.AmmountPaid),
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

    public class GetDuePaymentByIdHandler(ERP_DbContext context)
        : IRequestHandler<GetDuePaymentByIdQuery, DuePayment?>
    {
        public async Task<DuePayment?> Handle(GetDuePaymentByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.DuePayment
                //.Include(dp => dp.SupplierDue)
                .Include(dp => dp.CurrencyType)
                .Where(dp => !dp.IsDeleted)
                .FirstOrDefaultAsync(dp => dp.DuePaymentId == request.DuePaymentId, cancellationToken);
        }
    }

    public class GetAllDuePaymentsHandler(ERP_DbContext context)
        : IRequestHandler<GetAllDuePaymentsQuery, List<DuePaymentDTO>>
    {
        public async Task<List<DuePaymentDTO>> Handle(GetAllDuePaymentsQuery request, CancellationToken cancellationToken)
        {
            var data = context.DuePayment
                //.Include(dp => dp.SupplierDue)
                //.ThenInclude(sd => sd!.Supplier)
                .Where(dp => !dp.IsDeleted)
                .OrderByDescending(dp => dp.DuePaymentId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                //data = data.Where(dp => dp.SupplierDue != null && dp.SupplierDue.Supplier != null
                //&& dp.SupplierDue.Supplier.Name.Contains(request.SearchBy));
            }

            if (request.LastId.HasValue)
            {
                data = data.Where(dp => dp.DuePaymentId < request.LastId);
            }

            return await data.Take(request.PageSize).Select(x=> new DuePaymentDTO(x)).ToListAsync(cancellationToken);
        }
    }
}