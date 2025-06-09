using MediatR;

namespace Crystal_Clinic_Mgm.Application.UMS.UsersAudit.Update
{
    public class UserAuditUpdateCommand : IRequest<bool>
    {
        public string IpAddress { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string BrowserName { get; set; } = string.Empty;
        public string BrowserVersion { get; set; } = string.Empty;
        public DateTime? ActionEnd { get; set; } = DateTime.Now;
    }
}
