using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Commands.Create;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.FilerPermissions;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetAllPermissions;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionControllers;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionDDL;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionForEdit;

namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    [RBAC]
    [Authorize]
    //[Tags("UMS")]
    public class PermissionController : BaseController
    {

        /// <summary>
        /// Get Permissions
        /// </summary>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        [HttpGet("ControllerByApplication")]
        public async Task<JsonResult> ControllerByApplicationDropDown(int ApplicationId)
        {

            var data = new GetPermissionControllerQuery();
            data.ApplicationId = ApplicationId;
            return await Mediator.Send(data);
        }

        /// <summary>
        /// filter Permissions
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        [HttpGet("FilterPermissions")]
        public async Task<JsonResult> FilterPermissionsDropDown(string ControllerName, int ApplicationId)
        {


            var data = new FilterPermissions();
            data.ControllerName = ControllerName;
            data.ApplicationId = ApplicationId;
            return await Mediator.Send(data);
        }

        /// <summary>
        /// get List of Permissions
        /// </summary>
        /// <param name="PermissionsListQuery"></param>
        /// <returns></returns>
        [HttpPost("List")]
        public async Task<DataTableResponse> GetAll(GetPermissionsListQuery PermissionsListQuery)
        {
            return await Mediator.Send(PermissionsListQuery);
        }

        /// <summary>
        /// Get Details Of a Permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<JsonResult> GetDetails(int id)
        {
            var data = new GetPermissionsDetailsQuery();
            data.Id = id;
            return await Mediator.Send(data);

        }

        /// <summary>
        /// Permission for edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("PermissionForEdit/{id}")]
        public async Task<JsonResult> GetPermissionForEdit(int id)
        {
            var data = new GetPermissionForEditQuery();
            data.Id = id;
            return await Mediator.Send(data);

        }


        /// <summary>
        /// Permission Dropdown List
        /// </summary>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        /// 

        [DisableRBAC]
        [HttpGet("Permission-DropDownList/{ApplicationId:int}")]
        public async Task<JsonResult> GetPermationDDL(int ApplicationId)
        {
            GetPermissionDDLQuery data = new()
            {
                ApplicationId = ApplicationId
            };
            return await Mediator.Send(data);

        }


        #region Other Actions

        /// <summary>
        /// Get Application Dropdown List
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get-Application-Dropdown")]
        public IActionResult GetApplicationDropDown()
        {
            var list = RolePermissionChecker.GetApplication();
            return Ok(list);
        }

        /// <summary>
        /// Get Controllers dropdown
        /// </summary>
        /// <param name="ApplicationName"></param>
        /// <returns></returns>
        [HttpGet("Get-Controllers-Dropdown/{ApplicationName}")]
        public IActionResult GetControllersDropDown(string ApplicationName)
        {
            var list = RolePermissionChecker.GetControllers(ApplicationName);
            return Ok(list);
        }

        /// <summary>
        /// Get Application Of controller
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <returns></returns>
        [HttpGet("Get-ActionOfController-Dropdown/{ControllerName}")]
        public IActionResult GetActionOfControllerDropDown(string ControllerName)
        {
            var list = RolePermissionChecker.GetActionOfController(ControllerName);
            return Ok(list);
        }

        #endregion
    }
}
