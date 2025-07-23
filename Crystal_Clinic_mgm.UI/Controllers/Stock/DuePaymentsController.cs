using Crystal_Clinic_Mgm.Application.BranchStock.Suppliers;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class DuePaymentsController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CreateDuePayment([FromForm] CreateDuePaymentCommand command)
        {
            var duePaymentId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetDuePaymentById), new { duePaymentId }, new { duePaymentId });
        }

        [HttpGet("{duePaymentId}")]
        public async Task<IActionResult> GetDuePaymentById(int duePaymentId)
        {
            var duePayment = await Mediator.Send(new GetDuePaymentByIdQuery { DuePaymentId = duePaymentId });
            if (duePayment == null)
                return NotFound();
            return Ok(duePayment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDuePayments([FromQuery] GetAllDuePaymentsQuery query)
        {
            var duePayments = await Mediator.Send(query);
            return Ok(duePayments);
        }

        [HttpPut("{duePaymentId}")]
        public async Task<IActionResult> UpdateDuePayment(int duePaymentId, [FromForm] UpdateDuePaymentCommand command)
        {
            if (duePaymentId != command.DuePaymentId)
                return BadRequest("DuePayment ID mismatch");

            var result = await Mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{duePaymentId}")]
        public async Task<IActionResult> DeleteDuePayment(int duePaymentId)
        {
            var result = await Mediator.Send(new DeleteDuePaymentCommand { DuePaymentId = duePaymentId });
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}