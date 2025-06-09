using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDetail;
using Crystal_Clinic_Mgm.Application.UMS.UserReport.RolesInfo.Queries;
using Crystal_Clinic_Mgm.Application.UMS.UserReport.UserAccountList.Queries;
using Crystal_Clinic_Mgm.Application.UMS.UserReport.UserRolePermission.Queries;
using Crystal_Clinic_Mgm.Common.Constants;

namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    [Authorize]
    [RBAC]
    public class UserReportController : BaseController
    {
        /// <summary>
        /// Get User Accounts Short Information 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetUserReports")]
        public async Task<JsonResult> GetReports(UserReportQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get User with Role Information Reports(Search by UserID,Branch,Active User and Manager)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetUserRolePermissionReports")]
        public async Task<JsonResult> GetUserRolePermissionReports(UserRolePermissionQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// GetRole Information Reports-Search by Role Name and Application Type(ID)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetRoleInfoReports")]
        public async Task<JsonResult> GetRoleInfoReports(GetRoleInfoListQuery query)
        {
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get User Profile with Allow Roles  Report
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetUserProfileReports")]
        public async Task<JsonResult> GetUserProfileReports(GetUserDetailQuery query)
        {
            return await Mediator.Send(query);
        }

    }
}
