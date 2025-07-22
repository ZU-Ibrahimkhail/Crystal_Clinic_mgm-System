using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.Suppliers
{
    public class CreateSupplierDueCommand : IRequest<int>
    {
        public int SupplierId { get; set; }
        public decimal DueAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public int CurrencyTypeId { get; set; }
    }

    public class UpdateSupplierDueCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public decimal DueAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public int CurrencyTypeId { get; set; }
    }

    public class DeleteSupplierDueCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class GetSupplierDueByIdQuery : IRequest<SupplierDue?>
    {
        public int Id { get; set; }
    }

    public class GetAllSupplierDuesQuery : IRequest<List<SupplierDue>>
    {
        public string? SearchBy { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
    }

    public class CreateSupplierDueHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<CreateSupplierDueCommand, int>
    {
        public async Task<int> Handle(CreateSupplierDueCommand request, CancellationToken cancellationToken)
        {
            var supplierDue = new SupplierDue
            {
                SupplierId = request.SupplierId,
                DueAmount = request.DueAmount,
                PaidAmount = request.PaidAmount,
                RemainAmount = request.RemainAmount,
                CurrencyTypeId = request.CurrencyTypeId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUser.Id
            };

            context.SupplierDue.Add(supplierDue);
            await context.SaveChangesAsync(cancellationToken);
            return supplierDue.Id;
        }
    }

    public class UpdateSupplierDueHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<UpdateSupplierDueCommand, bool>
    {
        public async Task<bool> Handle(UpdateSupplierDueCommand request, CancellationToken cancellationToken)
        {
            var supplierDue = await context.SupplierDue.FindAsync(request.Id);
            if (supplierDue == null || supplierDue.IsDeleted)
                return false;

            supplierDue.SupplierId = request.SupplierId;
            supplierDue.DueAmount = request.DueAmount;
            supplierDue.PaidAmount = request.PaidAmount;
            supplierDue.RemainAmount = request.RemainAmount;
            supplierDue.CurrencyTypeId = request.CurrencyTypeId;
            supplierDue.ModifiedOn = DateTime.UtcNow;
            supplierDue.ModifiedBy = loggedInUser.Id;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
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

    public class GetSupplierDueByIdHandler(ERP_DbContext context)
        : IRequestHandler<GetSupplierDueByIdQuery, SupplierDue?>
    {
        public async Task<SupplierDue?> Handle(GetSupplierDueByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.SupplierDue
                .Include(sd => sd.Supplier)
                .Include(x => x.Payments)
                .Include(sd => sd.CurrencyType)
                .Where(sd => !sd.IsDeleted)
                .FirstOrDefaultAsync(sd => sd.Id == request.Id, cancellationToken);
        }
    }

    public class GetAllSupplierDuesHandler(ERP_DbContext context)
        : IRequestHandler<GetAllSupplierDuesQuery, List<SupplierDue>>
    {
        public async Task<List<SupplierDue>> Handle(GetAllSupplierDuesQuery request, CancellationToken cancellationToken)
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

            return await data.Take(request.PageSize).ToListAsync(cancellationToken);
        }
    }
}