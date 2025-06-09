using Crystal_Clinic_Mgm.Application.Look.BranchDetail;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Query
{
    public class GetOrderDetailsQuery : IRequest<OrderDetailsDto>
    {
        public int OrderId { get; set; }
    }

    public class GetOrderDetailsHandler(ERP_DbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetOrderDetailsQuery, OrderDetailsDto>
    {
        public async Task<OrderDetailsDto> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            var order = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Branch)
                .Include(o => o.Employee)
                .Include(o => o.OrderItems).ThenInclude(i => i.Item)
                .Include(o => o.OrderItems).ThenInclude(i => i.Unit)
                .Include(o => o.OrderServices).ThenInclude(s => s.Service)
                .Include(o => o.Payments)
                .Include(o => o.Adjustments)
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException("Order not found");
            Localization localization = new Localization(httpContextAccessor);
            return new OrderDetailsDto
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                ScheduledDate = order.ScheduledDate,
                OrderType = order.OrderType.ToString(),
                Status = order.Status.ToString(),
                OriginalTotal = order.OriginalTotal,
                AdjustedTotal = order.AdjustedTotal,
                PaidAmount = order.PaidAmount,
                CustomerName = order.Customer?.Name ?? "N/A",
                BranchName = localization.GetName(order.Branch),
                BranchDetail = context.BranchDetails.Where(x=>x.Id== order.BranchDetailId).Select(x=>new BranchDetailsDto(x)).FirstOrDefault(),
                EmployeeName = localization.GetEmployeeLocalizedName(order.Employee, false),
                DeliveryNotes = order.DeliveryNotes,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item?.Name ?? "",
                    Quantity = i.Quantity,
                    originalPrice = i.IsRental ? i.Unit?.DailyRentalPrice : i.Unit?.SellingPrice,
                    ActualPrice = i.ActualPrice,
                    RentalDays = i.RentalDays,
                    TotalPrice = i.TotalPrice,
                    IsRental = i.IsRental,
                    UnitId = i.UnitId,
                    UnitName = i.Unit?.UnitName ?? "",
                    ReturnedQuantity = i.ReturnedQuantity,
                    DamagedQuantity = i.DamagedQuantity,
                    ManualPriceOverride = i.ActualPrice
                   
                }).ToList(),
                Services = order.OrderServices.Select(s => new OrderServiceDto
                {
                    ServiceId = s.ServiceId,
                    ServiceName = s.Service?.Name ?? "",
                    Quantity = s.Quantity,
                    Duration = s.Duration,
                    DurationType = s.DurationType,
                    Rate = s.ActualRate,
                    ManualPriceOverride = s.ActualRate,

                }).ToList(),
                Payments = order.Payments.Select(p => new OrderPaymentDto
                {
                    PaymentDate = p.PaymentDate,
                    Amount = p.Amount,
                    Method = p.Method.ToString(),
                    Reference = p.TransactionReference
                }).ToList(),
                Adjustments = order.Adjustments.Select(a => new OrderAdjustmentDto
                {
                    Reason = a.Reason,
                    AdjustmentAmount = a.Amount,
                }).ToList()
            };
        }
    }

    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal OriginalTotal { get; set; }
        public decimal AdjustedTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string DeliveryNotes { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public BranchDetailsDto? BranchDetail { get; set; }
        public List<OrderItemDto> Items { get; set; } = [];
        public List<OrderServiceDto> Services { get; set; } = [];
        public List<OrderPaymentDto> Payments { get; set; } = [];
        public List<OrderAdjustmentDto> Adjustments { get; set; } = [];
    }

   

    public class OrderItemDto
    {
        public int ItemId { get; set; } 
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsRental { get; set; }
        public int? UnitId { get; set; } 
        public string UnitName { get; set; } = string.Empty;
        public decimal ReturnedQuantity { get; set; }
        public decimal DamagedQuantity { get; set; }
        public decimal? originalPrice { get; set; }
        public int? RentalDays { get; set; }
        public decimal? ManualPriceOverride { get; set; }
    }

    public class OrderServiceDto
    {
        public string ServiceName { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public int ServiceId { get; set; }
        public decimal Quantity { get; set; }
        public int Duration { get; set; }
        public DurationType DurationType { get; set; }
        public decimal ManualPriceOverride { get; set; }
    }

    public class OrderPaymentDto
    {
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }

    public class OrderAdjustmentDto
    {
        public string Reason { get; set; } = string.Empty;
        public decimal AdjustmentAmount { get; set; }
    }

}
