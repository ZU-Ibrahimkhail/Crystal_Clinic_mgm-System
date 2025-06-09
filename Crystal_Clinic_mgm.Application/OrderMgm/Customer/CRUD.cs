using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Customers.Commands
{

    #region Create Customer
    public class CreateCustomerCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public CustomerType Type { get; set; } = CustomerType.Regular;
        public decimal CreditLimit { get; set; }
    }

    public class CreateCustomerHandler(ERP_DbContext context,ILoggedInUser loggedInUser) : IRequestHandler<CreateCustomerCommand, int>
    {
        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                Type = request.Type,
                CreditLimit = request.CreditLimit,
                IsBlacklisted = false,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now,
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync(cancellationToken);
            return customer.CustomerId;
        }
    }
    #endregion

    #region Update Customer
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public CustomerType Type { get; set; } = CustomerType.Regular;
        public decimal CreditLimit { get; set; }
        public bool IsBlacklisted { get; set; }
    }

    public class UpdateCustomerHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateCustomerCommand, bool>
    {
        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await context.Customers.FindAsync(request.CustomerId);
            if (customer == null) return false;

            customer.Name = request.Name;
            customer.Phone = request.Phone;
            customer.Email = request.Email;
            customer.Address = request.Address;
            customer.Type = request.Type;
            customer.CreditLimit = request.CreditLimit;
            customer.IsBlacklisted = request.IsBlacklisted;
            customer.ModifiedBy = loggedInUser.Id;
            customer.ModifiedOn = DateTime.Now;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    #region Delete Customer
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public int CustomerId { get; set; }
    }

    public class DeleteCustomerHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeleteCustomerCommand, bool>
    {
        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await context.Customers.FindAsync(request.CustomerId);
            if (customer == null) return false;
            customer.IsDeleted = true;
            customer.ModifiedOn = DateTime.Now;
                
            context.Customers.Remove(customer);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    #endregion

    #region Get Customer By Id
    public class GetCustomerByIdQuery : IRequest<Customer?>
    {
        public int CustomerId { get; set; }
    }

    public class GetCustomerByIdHandler(ERP_DbContext context) : IRequestHandler<GetCustomerByIdQuery, Customer?>
    {
        public async Task<Customer?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);
        }
    }
    #endregion

    #region Get Customer List with Pagable
    public class GetCustomersQuery : IRequest<GetCustomersResponse>
    {
        public string? SearchText { get; set; }
        public int? LastCustomerId { get; set; } // Cursor Pagination
        public int PageSize { get; set; } = 20;
    }

    public class GetCustomersResponse
    {
        public List<CustomerDto> Customers { get; set; } = new();
        public int? LastCustomerId { get; set; }
    }

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsBlacklisted { get; set; }
    }

    public class GetCustomersHandler(ERP_DbContext context) : IRequestHandler<GetCustomersQuery, GetCustomersResponse>
    {
        public async Task<GetCustomersResponse> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var query = context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(c => c.Name.Contains(request.SearchText) || c.Phone.Contains(request.SearchText));
            }

            if (request.LastCustomerId.HasValue)
            {
                query = query.Where(c => c.CustomerId > request.LastCustomerId.Value);
            }

            var customers = await query
                .OrderBy(c => c.CustomerId)
                .Take(request.PageSize)
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Phone = c.Phone,
                    Email = c.Email,
                    Address = c.Address,
                    IsBlacklisted = c.IsBlacklisted
                })
                .ToListAsync(cancellationToken);

            return new GetCustomersResponse
            {
                Customers = customers,
                LastCustomerId = customers.LastOrDefault()?.CustomerId
            };
        }
    }
    #endregion
}
