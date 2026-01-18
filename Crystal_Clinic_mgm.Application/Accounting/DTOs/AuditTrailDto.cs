namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class AuditTrailDto
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

    public class CreateAuditTrailDto
    {
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string Action { get; set; } = string.Empty;
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

    public class AuditTrailFilterRequest
    {
        public string? EntityType { get; set; }
        public int? EntityId { get; set; }
        public string? Action { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
