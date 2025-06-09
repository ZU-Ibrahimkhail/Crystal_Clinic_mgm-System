using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.UMS.DashBoards.EachAndTotalUserInDepartmentDashBoard;
using Crystal_Clinic_Mgm.Application.UMS.DashBoards.UserInRolesDashBoard;
using Crystal_Clinic_Mgm.Application.UMS.DashBoards.UserLogInByDepartmentPieChart;
using Crystal_Clinic_Mgm.Common.Constants;

namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    [Authorize]
    [RBAC]
    //[Tags("UMS")]
    public class UserReportsAndDashboardController : BaseController
    {
        /// <summary>
        /// Show Dashboard by total or any user in Branch
        /// </summary>
        /// <returns></returns>
        [HttpGet("DashBoard-EachAndTotalUserInBranchDashBoard")]
        public async Task<IActionResult> EachAndTotalUserInBranchDashBoard()
        {

            EachAndTotalUserInBranchDashBoardCommand command = new()
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Show Dashboard by Users Roles   
        /// </summary>
        /// <returns></returns>
        [HttpGet("DashBoard-UserInRoleDashBoard")]
        public async Task<IActionResult> ByUserInRoleDashBoardDashboard()
        {

            UserInRoleCommand command = new()
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            return await Mediator.Send(command);
        }
        /// <summary>
        /// Shows logged in users count by branchs   
        /// </summary>
        /// <returns></returns>
        [HttpPost("DashBoard-UserLogInByBranchPieChart")]
        public async Task<IActionResult> UserLogInByBranchPieChart(UserLogInByBranchPieChartQuery command)
        {

            command.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(command);
        }



    }
}
