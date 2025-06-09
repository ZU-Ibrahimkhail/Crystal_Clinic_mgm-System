// CheckItemStockByDateQuery.cs
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Query
{
    public class CheckItemStockByDateQuery : IRequest<ItemStockAvailabilityDto>
    {
        public int ItemId { get; set; }
        public DateTime TargetDate { get; set; }
    }

    public class ItemStockAvailabilityDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal AvailableQuantity => CurrentStock - ReservedQuantity;
    }

    public class CheckItemStockByDateHandler(ERP_DbContext context) : IRequestHandler<CheckItemStockByDateQuery, ItemStockAvailabilityDto>
    {
        public async Task<ItemStockAvailabilityDto> Handle(CheckItemStockByDateQuery request, CancellationToken cancellationToken)
        {
            var item = await context.Items
                .FirstOrDefaultAsync(i => i.ItemId == request.ItemId, cancellationToken);

            if (item == null)
            {
                return new ItemStockAvailabilityDto
                {
                    ItemId = request.ItemId,
                    ItemName = "Item not found",
                    CurrentStock = 0,
                    ReservedQuantity = 0
                };
            }

            var reservedQty = await context.OrderItems
                .Where(oi => oi.ItemId == request.ItemId
                             && oi.RentalStartDate <= request.TargetDate
                             && oi.RentalEndDate >= request.TargetDate)
                .SumAsync(oi => oi.Quantity, cancellationToken);

            return new ItemStockAvailabilityDto
            {
                ItemId = item.ItemId,
                ItemName = item.Name,
                CurrentStock = item.CurrentStock,
                ReservedQuantity = reservedQty
            };
        }
    }
}
