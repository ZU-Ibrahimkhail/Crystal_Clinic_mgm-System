using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class AttendanceRecord : AuditableEntity
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }

        public int? ShiftId { get; set; }
        public Shift? Shift { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
        public decimal WorkingHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public string? Notes { get; set; }

        public DateTime AttendanceDate { get; set; }
    }
}
