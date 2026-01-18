using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class InventoryKit : AuditableEntity
    {
        public int Id { get; set; }
        public string KitName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsFreeForPatient { get; set; } // If true, the cost is internal expense
        public bool IsActive { get; set; } = true;
        public int BranchId { get; set; }

        // Navigation properties
        public ICollection<InventoryKitLine> KitLines { get; set; } = new List<InventoryKitLine>();
    }
}