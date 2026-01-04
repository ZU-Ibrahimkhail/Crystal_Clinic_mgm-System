namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class LabTestTemplate : AuditableEntity
    {
        public int Id { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal NormalRangeMin { get; set; }
        public decimal NormalRangeMax { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? ExpectedResult { get; set; }
        public bool IsActive { get; set; } = true;
        public string Description { get; set; } = string.Empty;
        public ICollection<LabOrderLine> OrderLines { get; set; } = new List<LabOrderLine>();
    }
}
