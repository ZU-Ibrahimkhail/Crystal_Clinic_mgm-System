namespace Crystal_Clinic_Mgm.Application.General.News.Queries.NewsDashboard
{
    public class NewsDashboardModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime NewsDate { get; set; }
        public string Speaker { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ShowNotification { get; set; } = string.Empty;
        public string AttachmentPath { get; set; } = string.Empty;

    }
}
