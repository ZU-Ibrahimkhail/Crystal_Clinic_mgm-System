// CreateOrderProcessor.cs
using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands.Helpers
{
    public class CreateOrderProcessor(ERP_DbContext context)
    {
        public async Task<ProcessOrderResult> ProcessOrderAsync(CreateOrderCommand request)
        {
            var result = new ProcessOrderResult();
            decimal calculatedTotal = 0m;

            // Process Items
            foreach (var itemDto in request.Items)
            {
                var item = await context.Items.Include(i => i.Units)
                    .FirstOrDefaultAsync(x => x.ItemId == itemDto.ItemId);
                if (item == null) throw new Exception($"Item not found: {itemDto.ItemId}");

                var unit = item.Units.FirstOrDefault(u => u.unitId == itemDto.UnitId);
                if (unit == null) throw new Exception($"Unit not found for item: {itemDto.UnitId}");

                decimal unitPrice = itemDto.ManualPriceOverride ?? (itemDto.IsRental ? unit.DailyRentalPrice ?? 0 : unit.SellingPrice);
                decimal itemTotal = unitPrice * itemDto.Quantity;
                calculatedTotal += itemTotal;

                var orderItem = new OrderItem
                {
                    ItemId = item.ItemId,
                    Quantity = itemDto.Quantity,
                    UnitId = itemDto.UnitId,
                    IsRental = itemDto.IsRental,
                    RentalDays = itemDto.RentalDays,
                    RentalStartDate = itemDto.IsRental ? request.RentalStartDate : null,
                    RentalEndDate = itemDto.IsRental ? request.RentalEndDate : null,
                    OriginalPrice = itemDto.IsRental ? unit.DailyRentalPrice ?? 0 : unit.SellingPrice,
                    ActualPrice = unitPrice,
                };

                result.OrderItems.Add(orderItem);

                // Create RentalReservation if it's a rental
                if (itemDto.IsRental)
                {
                    result.RentalReservations.Add(new RentalReservation
                    {
                        ItemId = item.ItemId,
                        ReservedQuantity = itemDto.Quantity,
                        RentalStartDate = request.RentalStartDate!.Value,
                        RentalEndDate = request.RentalEndDate!.Value,
                        BranchId = item.BranchId
                    });
                }
            }

            // Process Services
            foreach (var serviceDto in request.Services)
            {
                var service = await context.Services.FirstOrDefaultAsync(x => x.ServiceId == serviceDto.ServiceId);
                if (service == null) throw new Exception($"Service not found: {serviceDto.ServiceId}");

                decimal serviceRate = serviceDto.ManualPriceOverride ?? (serviceDto.DurationType == DurationType.Days ? service.DailyRate : service.HourlyRate);
                decimal serviceTotal = serviceRate * serviceDto.Quantity * serviceDto.Duration;
                calculatedTotal += serviceTotal;

                var orderService = new OrderService
                {
                    ServiceId = service.ServiceId,
                    Quantity = serviceDto.Quantity,
                    Duration = serviceDto.Duration,
                    DurationType = serviceDto.DurationType,
                    OriginalRate = serviceDto.DurationType == DurationType.Days ? service.DailyRate : service.HourlyRate,
                    ActualRate = serviceRate,
                    StartDate = request.RentalStartDate
                };

                result.OrderServices.Add(orderService);
            }

            // Total adjustment
            result.OriginalTotal = calculatedTotal;
            result.FinalTotal = request.ManualTotalOverride ?? calculatedTotal;
            return result;
        }
    }

    public class ProcessOrderResult
    {
        public decimal OriginalTotal { get; set; }
        public decimal FinalTotal { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public List<OrderService> OrderServices { get; set; } = new();
        public List<RentalReservation> RentalReservations { get; set; } = new();
    }
}
