namespace Crystal_Clinic_Mgm.Application.UMS.UsersAudit.GetList
{
    public class GetUserAuditListModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime ActionOn { get; set; }
        public DateTime? ActionEnd { get; set; }
        //public int? BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public bool Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public string BrowserName { get; set; } = string.Empty;
        public string BrowserVersion { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
    }
}
