// Updated ListAvailableRentalItemsQuery according to new business logic
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Query
{
    public class ListAvailableRentalItemsQuery : IRequest<List<RentalItemAvailabilityDto>>
    {
        public DateTime TargetDate { get; set; }
        public int BranchId { get; set; }
        public int? CategoryId { get; set; }
        public int? LastItemId { get; set; }
        public int PageSize { get; set; } = 20;
    }

    public class RentalItemAvailabilityDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal AvailableQuantity => CurrentStock - ReservedQuantity;
    }

    public class ListAvailableRentalItemsHandler(ERP_DbContext context) : IRequestHandler<ListAvailableRentalItemsQuery, List<RentalItemAvailabilityDto>>
    {
        public async Task<List<RentalItemAvailabilityDto>> Handle(ListAvailableRentalItemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Items
                .Where(i => (i.IsRentable || i.IsSellable) && i.BranchId == request.BranchId);

            if (request.CategoryId.HasValue)
            {
                query = query.Where(i => i.CategoryId == request.CategoryId);
            }

            if (request.LastItemId.HasValue)
            {
                query = query.Where(i => i.ItemId > request.LastItemId.Value);
            }

            var items = await query
                .OrderBy(i => i.ItemId)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var itemIds = items.Select(i => i.ItemId).ToList();

            // Reserved quantities from RentalReservations for rental items
            var rentalReservations = await context.RentalReservations
                .Where(r => itemIds.Contains(r.ItemId)
                            && r.BranchId == request.BranchId
                            && r.RentalStartDate <= request.TargetDate
                            && r.RentalEndDate >= request.TargetDate)
                .GroupBy(r => r.ItemId)
                .Select(g => new { ItemId = g.Key, ReservedQty = g.Sum(r => r.ReservedQuantity) })
                .ToListAsync(cancellationToken);

            var rentalReservationDict = rentalReservations.ToDictionary(r => r.ItemId, r => r.ReservedQty);

            // Reserved quantities from OrderItems for sellable items
            var sellableReservations = await context.OrderItems
                .Where(oi => itemIds.Contains(oi.ItemId)
                             && oi.SourceBranchId == request.BranchId
                             && oi.Order.ScheduledDate.HasValue
                             && oi.Order.ScheduledDate.Value.Date == request.TargetDate.Date)
                .GroupBy(oi => oi.ItemId)
                .Select(g => new { ItemId = g.Key, ReservedQty = g.Sum(oi => oi.Quantity) })
                .ToListAsync(cancellationToken);

            var sellableReservationDict = sellableReservations.ToDictionary(r => r.ItemId, r => r.ReservedQty);

            var results = items.Select(item =>
            {
                decimal reservedQty = 0;

                if (item.IsRentable)
                    reservedQty += rentalReservationDict.ContainsKey(item.ItemId) ? rentalReservationDict[item.ItemId] : 0;

                if (item.IsSellable)
                    reservedQty += sellableReservationDict.ContainsKey(item.ItemId) ? sellableReservationDict[item.ItemId] : 0;

                return new RentalItemAvailabilityDto
                {
                    ItemId = item.ItemId,
                    ItemName = item.Name,
                    CurrentStock = item.CurrentStock,
                    ReservedQuantity = reservedQty
                };
            }).ToList();

            return results;
        }
    }
}
