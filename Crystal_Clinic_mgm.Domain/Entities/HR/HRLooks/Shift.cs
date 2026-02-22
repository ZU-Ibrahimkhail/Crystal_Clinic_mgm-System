namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class Shift : LookAndAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int GracePeriodMinutes { get; set; } = 5;
        public bool IsActive { get; set; } = true;

        public ICollection<HR.AttendanceRecord> AttendanceRecords { get; set; } = new List<HR.AttendanceRecord>();
    }
}
