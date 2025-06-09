using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Create;
using Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Delete;
using Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Update;
using Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Queries.GetList;

namespace Crystal_Clinic_Mgm.UI.Controllers.HR
{
    [Authorize]
    [RBAC]
    public class AdvancePaymentController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvancePaymentCommand command)
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
        public async Task<IActionResult> Update(UpdateAdvancePaymentCommand command, int Id)
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
        public async Task<IActionResult> Delete(DeleteAdvancePaymentCommand command, int Id)
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
        /// Search Advance Payment List filter by employee and pay type
        /// Branch
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetAdvancePaymentListModel>> GetList(GetAdvancePaymentListQuery query)
        {
            return await Mediator.Send(query);
        }
   
    }
}
