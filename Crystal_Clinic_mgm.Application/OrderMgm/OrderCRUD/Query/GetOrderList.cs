using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Query
{
    public class ListOrdersQuery : IRequest<PaginatedOrderResult>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
    }


    public class ListOrdersHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ListOrdersQuery, PaginatedOrderResult>
    {
        public async Task<PaginatedOrderResult> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            var query = context.Orders.Where(x => (x.BranchId == loggedInUser.BranchId || loggedInUser.IsSuperAdmin))
                .Include(o => o.Customer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(o =>
                    o.OrderNumber.Contains(request.Search) ||
                    o.Customer!.Name.Contains(request.Search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize)
                .Select(o => new OrderSummaryDto
                {
                    OrderId = o.OrderId,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.Customer!.Name,
                    OrderDate = o.OrderDate,
                    ScheduledDate = o.ScheduledDate,
                    Status = o.Status,
                    AdjustedTotal = o.AdjustedTotal,
                    PaidAmount = o.PaidAmount,
                    isPaymentCompleted = o.AdjustedTotal == o.PaidAmount,

                })
                .ToListAsync(cancellationToken);

            return new PaginatedOrderResult
            {
                Orders = orders,
                TotalCount = totalCount
            };
        }
    }


    public class PaginatedOrderResult
    {
        public List<OrderSummaryDto> Orders { get; set; } = [];
        public int TotalCount { get; set; }
    }

    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal AdjustedTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public bool isPaymentCompleted { get; set; }
    }

}
