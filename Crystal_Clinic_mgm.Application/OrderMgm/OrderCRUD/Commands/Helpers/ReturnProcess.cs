using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands.Helpers
{

    #region Processor Service
    public class ReturnProcessor(ERP_DbContext context, ILoggedInUser loggedInUser)
    {

        public async Task<ResultOfProcessReturn> ProcessReturnAsync(
                    Orders order,
                    List<ReturnItemDto> returnedItems,
                    decimal manualDamageFee)
        {
            // Dictionary to map ordered quantities
            var orderedQtyMap = order.OrderItems.ToDictionary(i => i.ItemId, i => i.Quantity);

            var stockMovements = new List<StockMovement>();
            decimal totalReturnRefund = 0;

            // Initially setting order status to Completed
            order.Status = OrderStatus.Completed;

            var totalQtyOrdered = order.OrderItems.Sum(i => i.Quantity);
            var totalQtyReturned = 0;

            // Iterate over the returned items and process them
            foreach (var returnItem in returnedItems)
            {
                var orderItem = order.OrderItems.FirstOrDefault(oi => oi.ItemId == returnItem.ItemId);
                if (orderItem == null || orderItem.ReturnedQuantity + orderItem.DamagedQuantity >= orderItem.Quantity)
                    continue;

                // Process rental or sale returns
                var isRental = orderItem.IsRental;
                var processResult = isRental
                    ? await ProcessRentalReturn(orderItem, returnItem)  // Handle rental returns
                    : await ProcessSaleReturn(orderItem, returnItem);    // Handle sale returns

                stockMovements.AddRange(processResult.StockMovements);
                totalReturnRefund += processResult.RefundAmount;
                totalQtyReturned += returnItem.CleanReturnedQuantity + (returnItem.DamagedQuantity ?? 0);
            }

            // Create a return record with the appropriate return status
            var returnRecord = new Return
            {
                OrderId = order.OrderId,
                ReturnDate = DateTime.Now,
                DamageFee = manualDamageFee,
                DepositRefund = order.AdjustedTotal,
                EmployeeId = loggedInUser.EmployeeId
            };
            order.PaidAmount += manualDamageFee;
            // Adjust order status based on returned items
            if (totalQtyReturned >= totalQtyOrdered)
            {
                order.Status = OrderStatus.Completed;
                returnRecord.Status = OrderStatus.Completed.ToString();
            }
            else if (totalQtyReturned > 0)
            {
                order.Status = OrderStatus.PartiallyReturned;
                returnRecord.Status = OrderStatus.PartiallyReturned.ToString();
            }



            // Save the return record and apply stock movements
            context.StockMovements.AddRange(stockMovements);

            context.Returns.Add(returnRecord);
            context.Orders.Update(order);
            await context.SaveChangesAsync();

            // Return processed data
            return new ResultOfProcessReturn
            {
                ReturnRecord = returnRecord,
                StockMovements = stockMovements
            };
        }

        private async Task<(List<StockMovement> StockMovements, decimal RefundAmount)> ProcessRentalReturn(OrderItem orderItem, ReturnItemDto returnItem)
        {
            return await Task.Run(() =>
           {
               var stockMovements = new List<StockMovement>();
               decimal extendedCost = 0;
               decimal refundAmount = 0;

               var unit = orderItem.Unit;
               var factor = unit?.ConversionFactor ?? 1;
               var cleanQty = returnItem.CleanReturnedQuantity;
               var cleanQtyInStockUnit = cleanQty * factor;

               if (returnItem.ExtendedDays > 0)
               {
                   var extendedDays = returnItem.ExtendedDays.Value;
                   var rentalRate = orderItem.ActualPrice;
                   extendedCost = extendedDays * returnItem.KeptForExtraDaysQuantity * rentalRate;

                   // Add extended cost to adjusted total
                   orderItem.TotalPrice += extendedCost;
                   orderItem.Remarks = $"Extended {returnItem.KeptForExtraDaysQuantity} items for {extendedDays} days";

                   // Create rental reservation record
                   context.RentalReservations.Add(new RentalReservation
                   {
                       ItemId = returnItem.ItemId,
                       OrderId = orderItem.OrderId,
                       ReservedQuantity = returnItem.KeptForExtraDaysQuantity,
                       RentalStartDate = orderItem.RentalEndDate ?? DateTime.Now,
                       RentalEndDate = (orderItem.RentalEndDate ?? DateTime.Now).AddDays(extendedDays),
                       BranchId = orderItem.SourceBranchId ?? 1,
                   });
               }

               if (returnItem.DamagedQuantity > 0)
               {
                   var damagedQtyInStockUnit = returnItem.DamagedQuantity.Value * factor;
                   stockMovements.Add(new StockMovement
                   {
                       ItemId = orderItem.ItemId,
                       Quantity = -damagedQtyInStockUnit,
                       MovementType = MovementType.Out,
                       Reason = MovementReason.DamageWriteOff,
                       OrderId = orderItem.OrderId,
                       ReferenceId = $"Return-{orderItem.OrderId}",
                       Date = DateTime.Now,
                       Notes = "Rental item damaged"
                   });
                   orderItem.DamagedQuantity += returnItem.DamagedQuantity.Value;
               }

               // Clean rental returns are NOT added back to stock — system tracks them via reservations
               stockMovements.Add(new StockMovement
               {
                   ItemId = orderItem.ItemId,
                   Quantity = -cleanQtyInStockUnit,
                   MovementType = MovementType.Out,
                   Reason = MovementReason.RentalReturn,
                   OrderId = orderItem.OrderId,
                   ReferenceId = $"Return-{orderItem.OrderId}",
                   Date = DateTime.Now,
                   Notes = "Rental item returned"
               });

               // Calculate refund for rental return (not including extended days)
               //refundAmount = cleanQty * orderItem.ActualPrice;

               return (stockMovements, refundAmount);
           });
        }

        private async Task<(List<StockMovement> StockMovements, decimal RefundAmount)> ProcessSaleReturn(OrderItem orderItem, ReturnItemDto returnItem)
        {
            return await Task.Run(() =>
            {

                var stockMovements = new List<StockMovement>();
                decimal refundAmount = 0;

                var unit = orderItem.Unit;
                var factor = unit?.ConversionFactor ?? 1;
                var cleanQty = returnItem.CleanReturnedQuantity;
                var cleanQtyInStockUnit = cleanQty * factor;

                // Add returned quantity back to stock
                stockMovements.Add(new StockMovement
                {
                    ItemId = orderItem.ItemId,
                    Quantity = cleanQtyInStockUnit,
                    MovementType = MovementType.In,
                    Reason = MovementReason.SaleReturn,
                    OrderId = orderItem.OrderId,
                    ReferenceId = $"Return-{orderItem.OrderId}",
                    Date = DateTime.Now,
                    Notes = "Sale item returned"
                });

                // Calculate refund for returned sale item
                refundAmount = cleanQty * orderItem.ActualPrice;

                return (stockMovements, refundAmount);
            });
        }

    }
    #endregion
}
