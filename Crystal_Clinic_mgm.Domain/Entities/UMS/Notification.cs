namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class Notification : AuditableEntity
    {
        public int ID { get; set; }
        public int? TrackingId { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public Guid FromUserId { get; set; }
        public ApplicationUser? FromUser { get; set; }
        public Guid ToUserId { get; set; }
        public ApplicationUser? ToUser { get; set; }
        public bool IsRead { get; set; } = false;
        public int ApplicationId { get; set; }
        public Applications? Application { get; set; }
        public int? BranchId { get; set; }
        //public Branch? Branch { get; set; }
        public int NotificationMessageId { get; set; }
        public NotificationMessage? NotificationMessage { get; set; }
    }
}
