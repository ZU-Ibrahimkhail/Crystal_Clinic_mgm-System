using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Command.Create;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Command.Delete;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Command.Update;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Queries.GetList;


namespace Crystal_Clinic_Mgm.UI.Controllers.HR.HRLooks
{
    [Authorize]
    [RBAC]
    public class ContractTypeController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateContractTypeCommand command)
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
        public async Task<IActionResult> Update(UpdateContractTypeCommand command, int Id)
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
        public async Task<IActionResult> Delete(DeleteContractTypeCommand command, int Id)
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
        public async Task<ResponseDataTable<GeneralLookListModel>> GetAll(GetContractTypeListQuery query)
        {
            return await Mediator.Send(query);

        }

        [HttpGet("GetDropDownList")]
        [DisableRBAC]
        public async Task<IActionResult> GetDropDownList()
        {
            var command = new GetContractTypeDDLQuery();
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
