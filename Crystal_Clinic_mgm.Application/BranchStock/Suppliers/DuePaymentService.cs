using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
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
    }

    public class DeleteDuePaymentCommand : IRequest<bool>
    {
        public int DuePaymentId { get; set; }
    }

    public class GetDuePaymentByIdQuery : IRequest<DuePayment?>
    {
        public int DuePaymentId { get; set; }
    }

    public class GetAllDuePaymentsQuery : IRequest<List<DuePayment>>
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
            var duePayment = new DuePayment
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

            context.DuePayment.Add(duePayment);
            await context.SaveChangesAsync(cancellationToken);
            return duePayment.DuePaymentId;
        }
    }

    public class UpdateDuePaymentHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<UpdateDuePaymentCommand, bool>
    {
        public async Task<bool> Handle(UpdateDuePaymentCommand request, CancellationToken cancellationToken)
        {
            var duePayment = await context.DuePayment.FindAsync(request.DuePaymentId);
            if (duePayment == null || duePayment.IsDeleted)
                return false;

            duePayment.SupplierDueId = request.SupplierDueId;
            duePayment.CurrencyTypeId = request.CurrencyTypeId;
            duePayment.ExchangeRateToDueCurrency = request.ExchangeRateToDueCurrency;
            duePayment.AmmountPaid = request.AmmountPaid;
            duePayment.AmountInDueCurrency = request.AmountInDueCurrency;
            duePayment.paymentDate = request.PaymentDate;
            duePayment.ModifiedOn = DateTime.UtcNow;
            duePayment.ModifiedBy = loggedInUser.Id;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public class DeleteDuePaymentHandler(ERP_DbContext context)
        : IRequestHandler<DeleteDuePaymentCommand, bool>
    {
        public async Task<bool> Handle(DeleteDuePaymentCommand request, CancellationToken cancellationToken)
        {
            var duePayment = await context.DuePayment.FindAsync(request.DuePaymentId);
            if (duePayment == null || duePayment.IsDeleted)
                return false;

            duePayment.IsDeleted = true;
            context.DuePayment.Update(duePayment);
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
        : IRequestHandler<GetAllDuePaymentsQuery, List<DuePayment>>
    {
        public async Task<List<DuePayment>> Handle(GetAllDuePaymentsQuery request, CancellationToken cancellationToken)
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

            return await data.Take(request.PageSize).ToListAsync(cancellationToken);
        }
    }
}