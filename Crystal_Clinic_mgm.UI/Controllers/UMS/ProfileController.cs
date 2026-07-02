using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.ChangePassword;
using Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDetail;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Application.Common.RBAC;


namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private readonly ILoggedInUser _loggedInUser;
        public ProfileController(ILoggedInUser loggedInUser)
        {
            _loggedInUser = loggedInUser;
        }
        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [DisableRBAC]
        [HttpPost("Change-Password")]
        public async Task<JsonResult> ChangePassword(UserChangePasswordCommand data)
        {
            if (ModelState.IsValid)
            {
                data.ID ??= _loggedInUser.Id.ToString();
                return await Mediator.Send(data);
            }
            else
            {
                return new JsonResult("Please check the Model", data);
            }
        }

        /// <summary>
        ///User Profile: This is API will return all user Related Information, User Info and related Roles 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [DisableRBAC]
        public async Task<JsonResult> Profile()
        {
            var command = new GetUserDetailQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                Id = _loggedInUser.Id
            };
            return await Mediator.Send(command);

        }

    }
}
