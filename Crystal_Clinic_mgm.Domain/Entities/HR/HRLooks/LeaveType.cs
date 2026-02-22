namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class LeaveType : LookAndAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public int MaxDaysPerYear { get; set; }
        public bool IsPaid { get; set; } = true;
        public bool AllowCarryover { get; set; } = false;
        public int MaxCarryoverDays { get; set; } = 0;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<HR.LeaveRequest> LeaveRequests { get; set; } = new List<HR.LeaveRequest>();
    }
}
