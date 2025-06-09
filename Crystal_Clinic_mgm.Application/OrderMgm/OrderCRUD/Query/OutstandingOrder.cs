// GetOutstandingOrdersQuery.cs
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.Services.Repositories;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Query
{
    public class GetOutstandingOrdersQuery : IRequest<List<OutstandingOrderDto>> { }

    public class OutstandingOrderDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal AdjustedTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingBalance => AdjustedTotal - PaidAmount;
    }

    public class GetOutstandingOrdersHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<GetOutstandingOrdersQuery, List<OutstandingOrderDto>>
    {
        public async Task<List<OutstandingOrderDto>> Handle(GetOutstandingOrdersQuery request, CancellationToken cancellationToken)
        {
            return await context.Orders
                .Where(o => o.AdjustedTotal > o.PaidAmount && (loggedInUser.IsSuperAdmin || o.BranchId == loggedInUser.BranchId))
                .Include(o => o.Customer)
                .Select(o => new OutstandingOrderDto
                {
                    OrderId = o.OrderId,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.Customer!.Name,
                    AdjustedTotal = o.AdjustedTotal,
                    PaidAmount = o.PaidAmount
                })
                .ToListAsync(cancellationToken);
        }
    }
}
