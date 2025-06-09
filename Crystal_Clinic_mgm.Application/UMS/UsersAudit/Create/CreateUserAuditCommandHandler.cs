using MediatR;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Shyjus.BrowserDetection;
namespace Crystal_Clinic_Mgm.Application.UMS.UsersAudit.Create
{
    public class CreateUserAuditCommandHandler : IRequestHandler<CreateUserAuditCommand, int>
    {
        private readonly UMS_DbContext _UMSDbcontext;
        private readonly IBrowserDetector _browserDetector;
        public CreateUserAuditCommandHandler(UMS_DbContext uMSDbcontext, IBrowserDetector browserDetector)
        {
            _UMSDbcontext = uMSDbcontext;
            _browserDetector = browserDetector;
        }
        public async Task<int> Handle(CreateUserAuditCommand request, CancellationToken cancellationToken)
        {
            try
            {
                System.Net.IPAddress ipaddress = System.Net.Dns.GetHostEntry("").AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
                string hostname = System.Net.Dns.GetHostEntry(ipaddress).HostName;
                var browser = _browserDetector.Browser;
                UserAudit useraudit = new();
                if (browser != null)
                {
                    useraudit.BrowserName = browser.Name.ToString();
                    useraudit.BrowserVersion = browser.Version.ToString();
                    useraudit.Os = browser.OS.ToString();
                    useraudit.DeviceType = browser.DeviceType.ToString();
                }
                useraudit.UserName = request.UserName;
                useraudit.UserId = request.UserId;
                useraudit.Action = request.Action;
                useraudit.ActionOn = DateTime.Now;
                useraudit.Message = request.Message;
                useraudit.Result = request.Result;
                useraudit.DeviceName = System.Net.Dns.GetHostName();
                useraudit.IpAddress = ipaddress.ToString();
                _UMSDbcontext.UserAudits.Add(useraudit);
                var result = await _UMSDbcontext.SaveChangesAsync(cancellationToken);
                if (result > 0)
                {
                    return useraudit.Id;
                }
                else
                    return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
