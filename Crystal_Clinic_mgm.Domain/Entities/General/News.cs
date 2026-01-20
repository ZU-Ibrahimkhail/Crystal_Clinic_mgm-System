namespace Crystal_Clinic_Mgm.Domain.Entities.General
{
    public class News : AuditableEntity
    {

        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime NewsDate { get; set; }
        public string? Speaker { get; set; }
        public string? Location { get; set; }
        public bool? ShowNotification { get; set; }
        public List<string>? AttachmentPath { get; set; }




    }
}
