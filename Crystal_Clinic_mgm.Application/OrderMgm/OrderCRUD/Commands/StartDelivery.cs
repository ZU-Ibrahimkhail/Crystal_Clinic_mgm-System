// Final Clean StartDeliveryHandler
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands
{
    public class StartDeliveryCommand : IRequest<JsonResult>
    {
        public int OrderId { get; set; }
        public string? DeliveryNotes { get; set; } = string.Empty;

    }

    public class StartDeliveryHandler(ERP_DbContext _context, IMessage _message) : IRequestHandler<StartDeliveryCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(StartDeliveryCommand request, CancellationToken ct)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Unit)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId, ct);

            if (order == null)
                return _message.RecordNotFound("Order not found.");

            foreach (var item in order.OrderItems)
            {
                var conversion = item.Unit?.ConversionFactor ?? 1;
                var stockQty = item.Quantity * conversion;

                if (!item.IsRental)
                {
                    item.Item!.CurrentStock -= stockQty;
                    item.Item!.RealTimeAvailableStock -= stockQty;

                    _context.StockMovements.Add(new StockMovement
                    {
                        ItemId = item.ItemId,
                        Quantity = -stockQty,
                        MovementType = MovementType.Out,
                        Reason = MovementReason.SaleDeduction,
                        Date = DateTime.Now,
                        OrderId = order.OrderId,
                        SourceBranchId = item.SourceBranchId,
                        ReferenceId = $"Order-{order.OrderId}"
                    });
                }
                item.Item!.RealTimeAvailableStock -= stockQty;

            }

            order.Status = OrderStatus.InProgress;
            order.DeliveryNotes = request.DeliveryNotes ?? string.Empty;
            await _context.SaveChangesAsync(ct);

            return _message.Saved("Delivery started and sellable items deducted.");
        }
    }
}
