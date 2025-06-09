using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Create;
using Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Delete;
using Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Update;
using Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetDetial;
using Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetList;

namespace Crystal_Clinic_Mgm.UI.Controllers.HR
{
    [Authorize]
    [RBAC]
    public class ContractDetailsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateContractDetailsCommand command)
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
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdateContractDetailsCommand command, int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                command.ID = Id;
                return await Mediator.Send(command);
            }
        }
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteContractDetailsCommand command, int Id)
        {
            if (Id <= 0)
                return BadRequest("Not valid Id");
            if (ModelState.IsValid)
            {
                command.ID = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// Search is done by
        /// ContractType
        /// PositionTitle
        /// JobPosition
        /// Branch
        /// </summary>
        /// <param name="query"></param>
        /// <param name="EmployeeId">The ID of Employee</param>
        /// <returns></returns>
        [HttpPost("GetList/{EmployeeId:int}")]
        public async Task<ResponseDataTable<GetContractDetailsListModel>> GetList(GetContractDetailsListQuery query, int EmployeeId)
        {
            query.EmployeeProfileId = EmployeeId;
            return await Mediator.Send(query);

        }

        [HttpGet("GetEmployeeCurrentContract/{EmployeeProfileId:int}")]
        public async Task<GetContractDetailsListModel> GetEmployeeCurrentContract(int EmployeeProfileId)
        {
            GetEmployeeCurrentContractQuery query = new()
            {
                EmployeeProfileId = EmployeeProfileId
            };
            return await Mediator.Send(query);

        }
    }
}
