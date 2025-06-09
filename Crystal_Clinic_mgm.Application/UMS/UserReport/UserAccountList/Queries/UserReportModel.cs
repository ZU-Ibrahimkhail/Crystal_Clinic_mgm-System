namespace Crystal_Clinic_Mgm.Application.UMS.UserReport.UserAccountList.Queries
{
    public class UserReportModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Branch { get; set; }

    }
}
