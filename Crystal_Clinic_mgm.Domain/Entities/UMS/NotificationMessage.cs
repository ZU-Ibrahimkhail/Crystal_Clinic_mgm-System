namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class NotificationMessage : AuditableEntity
    {
        public int ID { get; set; }
        public string EnglishTitle { get; set; } = string.Empty;
        public string DariTitle { get; set; } = string.Empty;
        public string PashtoTitle { get; set; } = string.Empty;

        public string EnglishMessage { get; set; } = string.Empty;
        public string DariMessage { get; set; } = string.Empty;
        public string PashtoMessage { get; set; } = string.Empty;

        public string ApplicationName { get; set; } = string.Empty;
        public string EnglishDescription { get; set; } = string.Empty;
        public string DariDescription { get; set; } = string.Empty;

        public string PashtoDescription { get; set; } = string.Empty;
        public string? ControllerLink { get; set; }
        public string? ActionLink { get; set; }
    }
}
