namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class AuditTrail : AuditableEntity
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime AuditDate { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? BeforeValues { get; set; }
        public string? AfterValues { get; set; }
        public string? CorrelationId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public int? RelatedEntityId { get; set; }
        public string? RelatedEntityType { get; set; }
    }

    public enum AuditAction
    {
        Create,
        Update,
        Delete,
        Approve,
        Reject,
        Submit,
        Post,
        Void,
        Archive
    }
}
