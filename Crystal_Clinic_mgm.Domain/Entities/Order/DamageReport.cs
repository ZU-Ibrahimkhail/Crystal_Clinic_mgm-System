namespace Crystal_Clinic_Mgm.Domain.Entities.Order
{
    public class DamageReport
    {
        public int DamageReportId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal RepairCost { get; set; }
        public string Images { get; set; } = string.Empty; // JSON paths
        public int ReturnItemId { get; set; }
    }
}
