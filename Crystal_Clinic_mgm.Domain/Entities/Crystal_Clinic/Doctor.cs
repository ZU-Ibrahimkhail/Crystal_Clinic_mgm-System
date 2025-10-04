namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class Doctor : AuditableEntity
    {
        public int doctorId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string specialty { get; set; } = string.Empty;
        public string contactInfo { get; set; } = string.Empty;
        public string services { get; set; } = string.Empty;
        public int employeeId { get; set; } = 0;
        public bool isAvailable { get; set; }
    }
}
