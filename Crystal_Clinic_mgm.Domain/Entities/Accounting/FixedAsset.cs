using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class FixedAsset : AuditableEntity
    {
        public int Id { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal PurchaseValue { get; set; }
        public decimal ResidualValue { get; set; }
        public int UsefulLifeMonths { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public decimal AccumulatedDepreciation { get; set; } = 0;
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public bool IsActive { get; set; } = true;
        public string Description { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
    }
}
