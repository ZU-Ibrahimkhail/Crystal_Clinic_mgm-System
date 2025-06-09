using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Create;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Delete;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Update;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetList;


namespace Crystal_Clinic_Mgm.UI.Controllers.HR.HRLooks
{
    [Authorize]
    [RBAC]
    public class PositionTitleController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionTitleCommand command)
        {
            if (ModelState.IsValid)
            {
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdatePositionTitleCommand command, int Id)
        {
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
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeletePositionTitleCommand command, int Id)
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

        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetPositionTitleListModel>> GetAll(GetPositionTitleListQuery query)
        {
            return await Mediator.Send(query);

        }
        [DisableRBAC]
        [HttpGet("GetDropDownList/{BranchId:int}")]
        public async Task<IActionResult> GetDropDownList(int BranchId)
        {
            var command = new GetPositionTitleDDLQuery
            {
                BranchId = BranchId
            };
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
