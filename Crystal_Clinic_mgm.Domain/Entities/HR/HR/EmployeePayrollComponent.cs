using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class EmployeePayrollComponent : AuditableEntity
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }

        public int ComponentId { get; set; }
        public PayrollComponent? Component { get; set; }

        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? OverrideAmount { get; set; }
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
