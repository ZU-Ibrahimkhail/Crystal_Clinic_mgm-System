using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Create;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Delete;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Update;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleDetail;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleForEdit;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRolesDropdown;


namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    [RBAC]
    [Authorize]
    //[Tags("UMS")]
    public class RoleController : BaseController
    {
        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="createRoleCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleCommand createRoleCommand)
        {
            return await Mediator.Send(createRoleCommand);
        }

        /// <summary>
        /// role for Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("RoleForEdit/{id}")]
        public async Task<JsonResult> GetRoleForEdit(int id)
        {
            var data = new GetRoleForEditQuery();
            data.Id = id;
            return await Mediator.Send(data);

        }

        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateRoleCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRoleCommand updateRoleCommand)
        {
            updateRoleCommand.RequestId = id;
            return await Mediator.Send(updateRoleCommand);
        }

        /// <summary>
        /// Get Details Of a role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<JsonResult> GetDetails(int id)
        {
            var data = new GetRoleDetailQuery();
            data.Id = id;
            return await Mediator.Send(data);
        }

        /// <summary>
        /// Get All roles
        /// </summary>
        /// <param name="roleListQuery"></param>
        /// <returns></returns>
        [HttpPost("List")]
        public async Task<DataTableResponse> GetAll(GetRoleListQuery roleListQuery)
        {
            return await Mediator.Send(roleListQuery);
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteRoleCommand command, int Id)
        {

            if (Id <= 0)
                return BadRequest("Not valid Id");
            if (ModelState.IsValid)
            {
                command.Id = Id;
                return await Mediator.Send(command);

            }
            else

            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Get roles Dropdown List
        /// </summary>
        /// <returns></returns>
        [HttpGet("RolesDropdown")]
        [DisableRBAC]
        public async Task<JsonResult> RolesDropdown()
        {
            var data = new GetRolesDropdownQuery();
            return await Mediator.Send(data);
        }

    }
}