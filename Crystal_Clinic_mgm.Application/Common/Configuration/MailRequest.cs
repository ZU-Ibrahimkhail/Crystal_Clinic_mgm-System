namespace Crystal_Clinic_Mgm.Application.Common.Configuration
{
    public class MailRequest
    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        //public List<IFormFile>? Attachments { get; set; }
    }
}
