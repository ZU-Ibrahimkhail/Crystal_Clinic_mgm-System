using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class PositionTitle : LookAndAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public bool IsActive { get; set; } = true;

        // Phase 1 Enhancements
        public int? ReportsToId { get; set; }
        public PositionTitle? ReportsTo { get; set; }
        public string? JobGrade { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }

        public ICollection<HR.PayrollContract> PayrollContracts { get; set; } = new List<HR.PayrollContract>();
    }
}
