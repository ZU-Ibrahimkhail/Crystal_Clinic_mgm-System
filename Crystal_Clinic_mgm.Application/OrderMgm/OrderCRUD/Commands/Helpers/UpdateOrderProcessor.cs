// Updated UpdateOrderProcessor (Cleaned)
using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Domain.Entities.Order.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands.Helpers
{
    public class UpdateOrderProcessor(ERP_DbContext context)
    {
        public async Task<OrderProcessResult> ProcessOrderAsync(UpdateOrderCommand request, Orders existingOrder)
        {
            var originalTotal = 0m;
            var adjustments = new List<OrderAdjustment>();

            var updatedItemIds = request.Items.Select(i => i.ItemId).ToHashSet();
            var updatedServiceIds = request.Services.Select(s => s.ServiceId).ToHashSet();

            // Update or Add OrderItems
            foreach (var itemDto in request.Items)
            {
                var orderItem = existingOrder.OrderItems.FirstOrDefault(oi => oi.ItemId == itemDto.ItemId);
                if (orderItem != null)
                {
                    var unite = context.ItemUnits.FirstOrDefault(x => x.unitId == itemDto.UnitId && x.ItemId == itemDto.ItemId) ?? throw new Exception($"Unit not found with Unit ID : {itemDto.UnitId} for item ID : {itemDto.ItemId}");
                    decimal orignalPrice = Convert.ToDecimal(itemDto.IsRental ? unite.DailyRentalPrice : unite.SellingPrice);
                    orderItem.Quantity = itemDto.Quantity;
                    orderItem.RentalDays = itemDto.RentalDays;
                    orderItem.IsRental = itemDto.IsRental;
                    orderItem.OriginalPrice = itemDto.ManualPriceOverride ?? orignalPrice;
                    orderItem.ActualPrice = itemDto.ManualPriceOverride ?? orignalPrice;
                    orderItem.TotalPrice = orderItem.Quantity * orderItem.ActualPrice;
                    originalTotal += orderItem.TotalPrice;
                    context.OrderItems.Update(orderItem);
                    if (itemDto.IsRental)
                    {
                        var reservation = context.RentalReservations.FirstOrDefault(x => x.ItemId == orderItem.ItemId && x.OrderId == orderItem.OrderId && x.BranchId == orderItem.SourceBranchId);
                        if (reservation != null)
                        {
                            reservation.ReservedQuantity = orderItem.Quantity;
                            context.RentalReservations.Update(reservation);
                        }
                        else
                        {
                            context.RentalReservations.Add(new RentalReservation
                            {
                                ItemId = orderItem.ItemId,
                                OrderId = existingOrder.OrderId,
                                ReservedQuantity = itemDto.Quantity,
                                RentalStartDate = existingOrder.ScheduledDate!.Value,
                                RentalEndDate = existingOrder.ScheduledDate!.Value.AddDays(itemDto.RentalDays ?? 1),
                                BranchId = orderItem.SourceBranchId ?? orderItem.Item!.BranchId
                            });
                        }
                    }
                }
                else
                {
                    var item = await context.Items.Include(x => x.Units).FirstOrDefaultAsync(x => x.ItemId == itemDto.ItemId);
                    if (item == null) continue;
                    var unite = item.Units.FirstOrDefault(x => x.unitId == itemDto.UnitId) ?? throw new Exception($"Unit not found for Unit ID : {itemDto.UnitId}");
                    decimal orignalPrice = Convert.ToDecimal(itemDto.IsRental ? unite.DailyRentalPrice : unite.SellingPrice);
                    var newOrderItem = new OrderItem
                    {
                        ItemId = item.ItemId,
                        OrderId = existingOrder.OrderId,
                        Quantity = itemDto.Quantity,
                        RentalDays = itemDto.RentalDays,
                        IsRental = itemDto.IsRental,
                        OriginalPrice = orignalPrice,
                        ActualPrice = itemDto.ManualPriceOverride ?? orignalPrice,
                        RentalStartDate = itemDto.IsRental ? existingOrder.ScheduledDate : null,
                        RentalEndDate = itemDto.IsRental ? existingOrder.OrderItems.FirstOrDefault()?.RentalEndDate : null,
                        SourceBranchId = item.BranchId,

                    };
                    newOrderItem.TotalPrice = newOrderItem.ActualPrice * itemDto.Quantity;
                    existingOrder.OrderItems.Add(newOrderItem);
                    originalTotal += newOrderItem.TotalPrice;
                    context.OrderItems.Add(newOrderItem);
                    if (itemDto.IsRental)
                    {
                        context.RentalReservations.Add(new RentalReservation
                        {
                            ItemId = item.ItemId,
                            OrderId = existingOrder.OrderId,
                            ReservedQuantity = itemDto.Quantity,
                            RentalStartDate = existingOrder.ScheduledDate!.Value,
                            RentalEndDate = existingOrder.ScheduledDate!.Value.AddDays(itemDto.RentalDays ?? 1),
                            BranchId = item.BranchId
                        });
                    }
                }
            }

            // Remove missing OrderItems
            var removedItems = existingOrder.OrderItems.Where(oi => !updatedItemIds.Contains(oi.ItemId)).ToList();
            foreach (var removed in removedItems)
            {
                if (!removed.IsRental && existingOrder.Status == OrderStatus.InProgress)
                {
                    var item = await context.Items.FindAsync(removed.ItemId);
                    if (item != null)
                    {
                        item.CurrentStock += removed.Quantity;
                    }
                }
                context.OrderItems.Remove(removed);
                context.RemoveRange(context.RentalReservations.Where(x => x.OrderId == existingOrder.OrderId && x.ItemId == removed.ItemId));
            }

            // Update or Add OrderServices
            foreach (var serviceDto in request.Services)
            {
                var orderService = existingOrder.OrderServices.FirstOrDefault(os => os.ServiceId == serviceDto.ServiceId);
                if (orderService != null)
                {
                    orderService.Quantity = serviceDto.Quantity;
                    orderService.Duration = serviceDto.Duration;
                    orderService.DurationType = serviceDto.DurationType;
                    orderService.OriginalRate = serviceDto.ManualPriceOverride ?? orderService.OriginalRate;
                    orderService.ActualRate = serviceDto.ManualPriceOverride ?? orderService.OriginalRate;
                    originalTotal += serviceDto.Quantity * serviceDto.Duration * orderService.ActualRate;
                }
                else
                {
                    var service = await context.Services.FindAsync(serviceDto.ServiceId);
                    if (service == null) continue;

                    var newService = new OrderService
                    {
                        ServiceId = service.ServiceId,
                        Quantity = serviceDto.Quantity,
                        Duration = serviceDto.Duration,
                        DurationType = serviceDto.DurationType,
                        OriginalRate = serviceDto.ManualPriceOverride ?? 0,
                        ActualRate = serviceDto.ManualPriceOverride ?? 0,
                        StartDate = existingOrder.ScheduledDate
                    };
                    existingOrder.OrderServices.Add(newService);
                    originalTotal += newService.Quantity * newService.Duration * newService.ActualRate;
                }
            }

            // Remove missing OrderServices
            var removedServices = existingOrder.OrderServices.Where(os => !updatedServiceIds.Contains(os.ServiceId)).ToList();
            foreach (var removed in removedServices)
            {
                context.OrderServices.Remove(removed);
            }

            var finalTotal = request.ManualTotalOverride ?? originalTotal;
            await context.SaveChangesAsync();
            return new OrderProcessResult
            {
                OriginalTotal = originalTotal,
                FinalTotal = finalTotal,
                Adjustments = adjustments
            };
        }
    }

    public class OrderProcessResult
    {
        public decimal OriginalTotal { get; set; }
        public decimal FinalTotal { get; set; }
        public List<OrderAdjustment> Adjustments { get; set; } = new();
    }
}
