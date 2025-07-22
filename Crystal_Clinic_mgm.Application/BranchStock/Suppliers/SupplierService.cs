using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.Suppliers
{
    public class CreateSupplierCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateSupplierCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class DeleteSupplierCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class GetSupplierByIdQuery : IRequest<Supplier?>
    {
        public int Id { get; set; }
    }

    public class GetAllSuppliersQuery : IRequest<List<Supplier>>
    {
        public string? SearchBy { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
    }

    public class CreateSupplierHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<CreateSupplierCommand, int>
    {
        public async Task<int> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = new Supplier
            {
                Name = request.Name,
                ContactPerson = request.ContactPerson,
                ContactInfo = request.ContactInfo,
                Email = request.Email,
                Address = request.Address,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUser.Id
            };

            context.Supplier.Add(supplier);
            await context.SaveChangesAsync(cancellationToken);
            return supplier.Id;
        }
    }

    public class UpdateSupplierHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<UpdateSupplierCommand, bool>
    {
        public async Task<bool> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await context.Supplier.FindAsync(request.Id);
            if (supplier == null || supplier.IsDeleted)
                return false;

            supplier.Name = request.Name;
            supplier.ContactPerson = request.ContactPerson;
            supplier.ContactInfo = request.ContactInfo;
            supplier.Email = request.Email;
            supplier.Address = request.Address;
            supplier.ModifiedOn = DateTime.UtcNow;
            supplier.ModifiedBy = loggedInUser.Id;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public class DeleteSupplierHandler(ERP_DbContext context)
        : IRequestHandler<DeleteSupplierCommand, bool>
    {
        public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await context.Supplier.FindAsync(request.Id);
            if (supplier == null || supplier.IsDeleted)
                return false;

            supplier.IsDeleted = true;
            context.Supplier.Update(supplier);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public class GetSupplierByIdHandler(ERP_DbContext context)
        : IRequestHandler<GetSupplierByIdQuery, Supplier?>
    {
        public async Task<Supplier?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Supplier
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        }
    }

    public class GetAllSuppliersHandler(ERP_DbContext context)
        : IRequestHandler<GetAllSuppliersQuery, List<Supplier>>
    {
        public async Task<List<Supplier>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var data = context.Supplier
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.Id)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                data = data.Where(s => s.Name.Contains(request.SearchBy)
                    || s.ContactPerson.Contains(request.SearchBy)
                    || s.ContactInfo.Contains(request.SearchBy)
                    || s.Email != null && s.Email.Contains(request.SearchBy)
                    || s.Address != null && s.Address.Contains(request.SearchBy));
            }

            if (request.LastId.HasValue)
            {
                data = data.Where(s => s.Id < request.LastId);
            }

            return await data.Take(request.PageSize).ToListAsync(cancellationToken);
        }
    }
}