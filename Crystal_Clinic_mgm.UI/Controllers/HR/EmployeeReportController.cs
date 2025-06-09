using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.HR.HR.Report.Queries;
using Crystal_Clinic_Mgm.Common.Constants;

namespace Crystal_Clinic_Mgm.UI.Controllers.HR
{
    [Authorize]
    [RBAC]
    public class EmployeeReportController : BaseController
    {
        [HttpPost("Get-Employee-Reports")]
        public async Task<JsonResult> GetReports(EmployeeReportQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }

    }
}
