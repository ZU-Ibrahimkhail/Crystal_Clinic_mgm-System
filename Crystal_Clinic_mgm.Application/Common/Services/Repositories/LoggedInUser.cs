using Microsoft.AspNetCore.Http;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Common.Services.Repositories
{
    public class LoggedInUser : ILoggedInUser
    {

        private readonly UMS_DbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedInUser(UMS_DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            _context = context;
        }
        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public Guid Id => Guid.Parse(_httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? string.Empty);

        public string UserName => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? string.Empty;
        public string EmpEnglishName => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "ITEmployeeName")?.Value ?? string.Empty;

        public string EmpDariName => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "ITEmployeeDariName")?.Value ?? string.Empty;

        public string Email => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value ?? string.Empty;

        public string PhotoPath => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "PhotoPath")?.Value ?? string.Empty;
        public int BranchId => _context.Users.SingleOrDefault(u => u.Id == Id)?.BranchId ?? 0;
        public bool IsSuperAdmin => _context.Users.SingleOrDefault(u => u.Id == Id)?.IsSuperAdmin ?? false;
        public string Path => new HttpContextAccessor().HttpContext?.Request.Path.ToString() ?? string.Empty;
        public string Host => new HttpContextAccessor().HttpContext?.Request.Host.ToString() ?? string.Empty;

        public string? AllowedBranch => _context.UserAllowedDocTypesSecurityLevels.Where(u => u.UserId == Id).FirstOrDefault()?.AllowedBranchId;

        public int EmployeeId => _context.Users.SingleOrDefault(u => u.Id == Id)?.EmployeeId ?? 0;

        public bool IsBranchAdmin => _context.Users.SingleOrDefault(u => u.Id == Id)?.IsBranchAdmin ?? false;
    }
}
