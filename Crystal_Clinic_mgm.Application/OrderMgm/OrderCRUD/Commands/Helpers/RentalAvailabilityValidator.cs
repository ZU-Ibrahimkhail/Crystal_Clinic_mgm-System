// RentalAvailabilityValidator.cs
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands.Helpers
{
    public class RentalAvailabilityValidator(ERP_DbContext context)
    {
        public async Task<bool> IsRentalAvailableAsync(int itemId, int branchId, decimal requestedQuantity, DateTime startDate, DateTime endDate)
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.ItemId == itemId && i.BranchId == branchId);
            if (item == null)
                throw new Exception($"Item {itemId} not found in Branch {branchId}");

            var totalReserved = await context.RentalReservations
                .Where(r => r.ItemId == itemId
                            && r.BranchId == branchId
                            && r.RentalStartDate <= endDate
                            && r.RentalEndDate >= startDate)
                .SumAsync(r => (decimal?)r.ReservedQuantity) ?? 0;

            var availableQuantity = item.CurrentStock - totalReserved;

            return availableQuantity >= requestedQuantity;
        }
    }
}
