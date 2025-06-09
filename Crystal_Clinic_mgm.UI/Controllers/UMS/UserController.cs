using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.ActiveUser;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.ChangePassword;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.CreateUser;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.DeleteUser;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.PasswordReset;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.UpdateUser;
using Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDDL;
using Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDetail;
using Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserList;
using Crystal_Clinic_Mgm.Application.UMS.UsersAudit.GetList;
using Crystal_Clinic_Mgm.Common.Constants;

namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{

    [Authorize]
    [RBAC]
    ////[Tags("UMS")]
    public class UserController : BaseController
    {

        /// <summary>
        /// Create a User
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public async Task<JsonResult> Create(CreateUserCommand data)
        {
            return await Mediator.Send(data);
        }

        /// <summary>
        /// Update a User Details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        public async Task<JsonResult> Update(UpdateUserCommand data)
        {
            return await Mediator.Send(data);
        }

        /// <summary>
        /// Get List of Users
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("GetUsers")]
        public async Task<DataTableResponse> GetAll(GetUserListQuery data)
        {
            data.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(data);
        }

        /// <summary>
        /// Get Details of User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetUserByID/{Id:Guid}")]
        public async Task<IActionResult> GetDetails(Guid Id)
        {


            return await Mediator.Send(
                new GetUserDetailQuery
                {
                    Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                    Id = Id
                });
        }

        /// <summary>
        /// Get User by id name Email
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("GetUserBy")]
        public async Task<IActionResult> GetUserBy(GetUserDetailQuery data)
        {
            data.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(data);

        }


        /// <summary>
        /// Users Dropdown List filtered by branch Id
        /// </summary>
        /// <param name="BranchId"></param>
        /// <param name="ShowLoginUser"></param>
        /// <returns></returns>
        [HttpGet("UsersDropdown/{BranchId:int}/{ShowLoginUser:bool?}")]
        [DisableRBAC]
        public async Task<JsonResult> UsersDropdown(int BranchId, bool? ShowLoginUser = false)
        {
            if (BranchId > 0)
            {
                var data = new GetUserDDLQuery
                {
                    Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                    BranchId = BranchId,
                    ShowLoginUser = ShowLoginUser ?? false
                };
                return await Mediator.Send(data);
            }
            else
            {
                return new JsonResult(null);
            }
        }





        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete(DeleteUserCommand command, Guid Id)
        {

            command.Id = Id;
            return await Mediator.Send(command);

        }

        /// <summary>
        /// For Active a user 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("UserActivation")]
        public async Task<JsonResult> Activation(ActiveUserCommand data)
        {
            return await Mediator.Send(data);

        }

        /// <summary>
        /// User Password Reset
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("PasswordReset")]
        public async Task<JsonResult> PasswordReset(UserPasswordResetCommand data)
        {
            if (ModelState.IsValid)
            {
                return await Mediator.Send(data);
            }
            else
            {
                return new JsonResult("Please check the Model", data);
            }
        }

        /// <summary>
        /// User Change Password
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword")]
        [DisableRBAC]
        public async Task<JsonResult> ChangePassword(UserChangePasswordCommand data)
        {
            if (ModelState.IsValid)
            {
                return await Mediator.Send(data);
            }
            else
            {
                return new JsonResult("Please check the Model", data);
            }
        }

        [HttpPost("UserLog")]
        public async Task<ResponseDataTable<GetUserAuditListModel>> UserLog(GetUserAuditListQuery data)
        {
            return await Mediator.Send(data);
        }


    }
}
