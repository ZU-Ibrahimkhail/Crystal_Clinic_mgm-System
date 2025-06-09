using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HR.Dashboard.ActiveEmployeePieChart;
using Crystal_Clinic_Mgm.Application.HR.HR.Dashboard.ActiveUsersSpiderChart;
using Crystal_Clinic_Mgm.Common.Constants;

namespace Crystal_Clinic_Mgm.UI.Controllers.HR
{
    [Authorize]
    [RBAC]
    public class HRDashboardController : BaseController
    {
        [HttpGet("Dashboard-ActiveEmplyeesPieChart")]
        public async Task<List<SpiderData>> ActiveEmployeesPieChart()
        {
            ActiveEmployeePieChartQuery Query = new()
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
            };
            return await Mediator.Send(Query);
        }

        [HttpGet("Dashboard-EmpHasUserSpiderChart")]
        public async Task<JsonResult> EmpHasUserSpiderChart()
        {
            ActiveUsersSpiderChartQuery Query = new()
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
            };
            return await Mediator.Send(Query);
        }
    }
}
