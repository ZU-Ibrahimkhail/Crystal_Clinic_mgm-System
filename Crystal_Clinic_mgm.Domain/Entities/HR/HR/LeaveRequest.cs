using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class LeaveRequest : AuditableEntity
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }

        public int LeaveTypeId { get; set; }
        public LeaveType? LeaveType { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }

        public string? Reason { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public int? ApprovedById { get; set; }
        public EmployeeProfile? ApprovedBy { get; set; }

        public DateTime? ApprovalDate { get; set; }
        public string? ApprovalNotes { get; set; }

        public string? AttachmentPath { get; set; }
    }
}
