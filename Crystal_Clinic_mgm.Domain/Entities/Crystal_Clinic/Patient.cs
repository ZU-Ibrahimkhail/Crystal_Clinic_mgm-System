namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class Patient : AuditableEntity
    {
        public int patientId { get; set; }
        public string name { get; set; } = string.Empty;
        public string contactInfo { get; set; } = string.Empty;
        public string? email { get; set; }
        public decimal? age { get; set; }
        public string? gender { get; set; }
    }
}
