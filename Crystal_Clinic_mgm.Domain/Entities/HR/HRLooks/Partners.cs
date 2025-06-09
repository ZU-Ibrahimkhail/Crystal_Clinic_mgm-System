namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class Partners : AuditableEntity
    {
        public int ID { get; set; }
        public string NameInEnglish { get; set; } = string.Empty;
        public string NameInPashto { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
