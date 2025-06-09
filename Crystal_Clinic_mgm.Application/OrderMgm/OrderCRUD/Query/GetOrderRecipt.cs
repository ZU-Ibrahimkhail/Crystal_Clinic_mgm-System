using Crystal_Clinic_Mgm.Application.Look.BranchDetail;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Query
{
    public class GetOrderReciptQuery : IRequest<GetOrderReciptDto>
    {
        public int OrderId { get; set; }
    }

    public class GetOrderReciptHandler(ERP_DbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetOrderReciptQuery, GetOrderReciptDto>
    {
        public async Task<GetOrderReciptDto> Handle(GetOrderReciptQuery request, CancellationToken cancellationToken)
        {
            var order = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Branch)
                .Include(o => o.Employee)
                .Include(o => o.OrderItems).ThenInclude(i => i.Item)
                .Include(o => o.OrderItems).ThenInclude(i => i.Unit)
                .Include(o => o.OrderServices).ThenInclude(s => s.Service)
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException("Order not found");
            Localization localization = new Localization(httpContextAccessor);
            return new GetOrderReciptDto
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
                RemainAmount = order.AdjustedTotal - order.PaidAmount,
                CustomerName = order.Customer?.Name ?? "N/A",
                BranchName = localization.GetName(order.Branch),
                BranchDetail = context.BranchDetails.Where(x => x.Id == order.BranchDetailId).Select(x => new BranchDetailsDto(x)).FirstOrDefault(),
                EmployeeName = localization.GetEmployeeLocalizedName(order.Employee, false),
                DeliveryNotes = order.DeliveryNotes,
                RentalItems = order.OrderItems.Where(r => r.IsRental).Select(i => new OrderItemDto
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item?.Name ?? "",
                    Quantity = i.Quantity,
                    originalPrice = i.Unit?.DailyRentalPrice,
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
                SaleItems = order.OrderItems.Where(r => !r.IsRental).Select(i => new OrderItemDto
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item?.Name ?? "",
                    Quantity = i.Quantity,
                    originalPrice = i.Unit?.SellingPrice,
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

                }).ToList()
            };
        }
    }

    public class GetOrderReciptDto
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
        public List<OrderItemDto> RentalItems { get; set; } = [];
        public List<OrderItemDto> SaleItems { get; set; } = [];
        public List<OrderServiceDto> Services { get; set; } = [];
        public decimal RemainAmount { get;  set; }
    }

}
