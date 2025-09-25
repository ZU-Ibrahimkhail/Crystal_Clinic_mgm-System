using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.Suppliers
{
    public class CreateSupplierDueCommand : IRequest<int>
    {
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public decimal DueAmount { get; set; }
        public DateTime? DueDate { get; set; } = DateTime.Now;

        [Required]
        public int CurrencyTypeId { get; set; }
    }
    public class CreateSupplierDueHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
    : IRequestHandler<CreateSupplierDueCommand, int>
    {
        public async Task<int> Handle(CreateSupplierDueCommand request, CancellationToken cancellationToken)
        {
            if (request.DueAmount < 0)
                throw new ArgumentException("Due amount cannot be negative.");
            var supplier = await context.Supplier.FindAsync(request.SupplierId, cancellationToken);
            if (supplier == null)
                throw new InvalidOperationException("Supplier not found.");

            var currencyType = await context.CurrencyType.FindAsync(request.CurrencyTypeId, cancellationToken);
            if (currencyType == null)
                throw new InvalidOperationException("Currency type not found.");
            var supplierDue = new SupplierDue
            {
                SupplierId = request.SupplierId,
                DueAmount = request.DueAmount,
                DueDate = request.DueDate,
                PaidAmount = 0,
                RemainAmount = request.DueAmount,
                CurrencyTypeId = request.CurrencyTypeId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUser.Id
            };

            context.SupplierDue.Add(supplierDue);
            await context.SaveChangesAsync(cancellationToken);
            return supplierDue.Id;
        }
    }
    
    public class UpdateSupplierDueCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime? DueDate { get; set; } = DateTime.Now;

    }
    public class UpdateSupplierDueHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<UpdateSupplierDueCommand, bool>
    {
        public async Task<bool> Handle(UpdateSupplierDueCommand request, CancellationToken cancellationToken)
        {
            var supplierDue = await  context.SupplierDue.FindAsync(request.Id, cancellationToken);
            if (supplierDue == null || supplierDue.IsDeleted)
                return false;
            if (request.DueAmount < supplierDue.PaidAmount)
                throw new ArgumentException("Due amount cannot be less than paid amount.");
            supplierDue.SupplierId = request.SupplierId;
            supplierDue.DueAmount = request.DueAmount;
            supplierDue.DueDate = request.DueDate;
            supplierDue.RemainAmount = request.DueAmount - supplierDue.PaidAmount;
            supplierDue.ModifiedOn = DateTime.UtcNow;
            supplierDue.ModifiedBy = loggedInUser.Id;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    
    public class DeleteSupplierDueCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeleteSupplierDueHandler(ERP_DbContext context)
        : IRequestHandler<DeleteSupplierDueCommand, bool>
    {
        public async Task<bool> Handle(DeleteSupplierDueCommand request, CancellationToken cancellationToken)
        {
            var supplierDue = await context.SupplierDue.FindAsync(request.Id);
            if (supplierDue == null || supplierDue.IsDeleted)
                return false;

            supplierDue.IsDeleted = true;
            context.SupplierDue.Update(supplierDue);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    
    public class GetSupplierDueByIdQuery : IRequest<SupplierDueDetailDTO?>
    {
        public int Id { get; set; }
    }
    public class GetSupplierDueByIdHandler(ERP_DbContext context)
        : IRequestHandler<GetSupplierDueByIdQuery, SupplierDueDetailDTO?>
    {
        public async Task<SupplierDueDetailDTO?> Handle(GetSupplierDueByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.SupplierDue
                .Include(sd => sd.Supplier)
                .Include(x => x.Payments.Where(y=>!y.IsDeleted))
                .ThenInclude(x=>x.CurrencyType)
                .Include(sd => sd.CurrencyType)
                .Where(sd => !sd.IsDeleted && sd.Id == request.Id)
                .Select(sd=> new SupplierDueDetailDTO(sd))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }


    public class GetAllSupplierDuesQuery : IRequest<List<SupplierDueDetailDTO>>
    {
        public string? SearchBy { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
    }
    public class GetAllSupplierDuesHandler(ERP_DbContext context)
        : IRequestHandler<GetAllSupplierDuesQuery, List<SupplierDueDetailDTO>>
    {
        public async Task<List<SupplierDueDetailDTO>> Handle(GetAllSupplierDuesQuery request, CancellationToken cancellationToken)
        {
            var data = context.SupplierDue
                .Include(sd => sd.Supplier)
                .Include(c=>c.CurrencyType)
                .Where(sd => !sd.IsDeleted)
                .OrderByDescending(sd => sd.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                data = data.Where(sd => sd.Supplier != null && sd.Supplier.Name.Contains(request.SearchBy));
            }

            if (request.LastId.HasValue)
            {
                data = data.Where(sd => sd.Id < request.LastId);
            }

            return await data.Take(request.PageSize).Select(x=> new SupplierDueDetailDTO(x)).ToListAsync(cancellationToken);
        }
    }

    public class SupplierDueDetailDTO(SupplierDue due)
    {
        public int Id { get; set; } = due.Id;
        public int SupplierId { get; set; } = due.SupplierId;

        // i also want to send dueDate in list
        public DateTime? DueDate { get; set; } = due.DueDate;
        public string SupplierName { get; set; } = due.Supplier?.Name ?? string.Empty;
        public string SupplierPersonName { get; set; } = due.Supplier?.ContactPerson ?? string.Empty;
        public string SupplierContact { get; set; } = due.Supplier?.ContactInfo ?? string.Empty;
        public decimal DueAmount { get; set; } = due.DueAmount;
        public decimal PaidAmount { get; set; } = due.PaidAmount;
        public decimal RemainAmount { get; set; } = due.RemainAmount;
        public int CurrencyTypeId { get; set; } = due.CurrencyTypeId;
        public string? CurrencyTypeCode { get; set; } = due.CurrencyType?.Code;
        public List<DuePaymentDTO> Payments { get; set; } = due.Payments.Select(x => new DuePaymentDTO(x)).ToList();
    }
    public class DuePaymentDTO(DuePayment payment)
    {
        public int DuePaymentId { get; set; } = payment.DuePaymentId;
        public int SupplierDueId { get; set; } = payment.SupplierDueId;
        public int? CurrencyTypeId { get; set; } = payment.CurrencyTypeId;
        public string? CurrencyTypeCode { get; set; } = payment.CurrencyType?.Code;
        public decimal ExchangeRateToDueCurrency { get; set; } = payment.ExchangeRateToDueCurrency;
        public decimal AmmountPaid { get; set; } = payment.AmountPaid;
        public decimal AmountInDueCurrency { get; set; } = payment.AmountInDueCurrency;
        public DateTime paymentDate { get; set; } = payment.paymentDate;
        public string? Remarks { get; set; } = payment.Remarks;
        public string? AttachmentPath { get; set; } = payment.AttachmentPath;
    }

}