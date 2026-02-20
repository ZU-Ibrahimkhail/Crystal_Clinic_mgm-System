namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class PayrollAdjustment : AuditableEntity
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }

        public PayrollAdjustmentType Type { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime AdjustmentDate { get; set; }

        public string? Description { get; set; }
        public string? ReferenceNumber { get; set; }
        public bool IsProcessed { get; set; } = false;
    }
}
