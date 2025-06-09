namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class EmailHistory : AuditableEntity
    {
        public int ID { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string ToEmail { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
    }
}
