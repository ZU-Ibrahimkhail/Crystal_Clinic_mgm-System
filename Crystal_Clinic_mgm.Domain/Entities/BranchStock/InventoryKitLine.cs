using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class InventoryKitLine : AuditableEntity
    {
        public int Id { get; set; }
        public int KitId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }

        // Navigation properties
        public InventoryKit? Kit { get; set; }
        public Item? Item { get; set; }
    }
}