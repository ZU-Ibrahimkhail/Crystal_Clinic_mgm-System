using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class SalesEstimateLine : AuditableEntity
    {
        public int Id { get; set; }
        public int SalesEstimateId { get; set; }
        public SalesEstimate? SalesEstimate { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? ItemId { get; set; } // For inventory items
        public Item? Item { get; set; }
    }
}