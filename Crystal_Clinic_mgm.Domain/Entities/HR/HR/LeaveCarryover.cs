using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class LeaveCarryover : AuditableEntity
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }

        public int LeaveTypeId { get; set; }
        public LeaveType? LeaveType { get; set; }

        public int CarryoverYear { get; set; }
        public decimal RemainingDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal ExpirationDays { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Notes { get; set; }
    }
}
