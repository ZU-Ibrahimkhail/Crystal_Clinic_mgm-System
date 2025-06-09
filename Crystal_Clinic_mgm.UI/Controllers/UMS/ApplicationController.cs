
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.Application.Command.Create;
using Crystal_Clinic_Mgm.Application.UMS.Application.Command.Delete;
using Crystal_Clinic_Mgm.Application.UMS.Application.Command.Update;
using Crystal_Clinic_Mgm.Application.UMS.Application.Queries.GetApplicationDDL;
using Crystal_Clinic_Mgm.Application.UMS.Application.Queries.GetApplicationList;
namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    [RBAC]
    [Authorize]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class ApplicationController : BaseController
    {
        ///// <summary>
        ///// Create Application
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> Create(CreateApplicationCommand command)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return await Mediator.Send(command);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
        /// <summary>
        /// Get All Applications
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllApplication")]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetApplicatonListQuery());
            return Ok(result);
        }
        ///// <summary>
        ///// Delete Application
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[HttpDelete("{Id:int}")]
        //public async Task<IActionResult> Delete(DeleteApplicationCommand command, int Id)
        //{
        //    if (Id <= 0)
        //        return BadRequest("Not valid Id");
        //    else
        //    {
        //        command.ID = Id;
        //        return await Mediator.Send(command);
        //    }
        //}
        ///// <summary>
        ///// update Application
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[HttpPut("{Id:int}")]
        //public async Task<IActionResult> Update(UpdateApplicationCommand command, int Id)
        //{
        //    if (Id <= 0)
        //    {
        //        return BadRequest("Not valid Id");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        command.ID = Id;
        //        return await Mediator.Send(command);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
        /// <summary>
        /// Get Card Dropdown List
        /// </summary>
        /// <returns></returns>
        /// 

        [DisableRBAC]
        [HttpGet("GetApplicationDDL")]
        public async Task<IActionResult> GetApplicationDDL()
        {
            var application = new GetApplicationDDLQuery();
            var result = await Mediator.Send(application);
            return Ok(result);
        }
        /// <summary>
        /// Get Application List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("Get-Application-List")]
        public async Task<ResponseDataTable<GetApplicationListLookupModel>> GetList(GetApplicatonListQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
