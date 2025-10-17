using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices;
using Crystal_Clinic_Mgm.Application.CrystalClinic.Doctors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [Authorize]
    [RBAC]
    public class CallListsController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CreateCallList([FromBody] CreateCallListCommand command)
        {
            var callListId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetCallListById), new { id = callListId }, new { callListId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCallListById(int id)
        {
            var callList = await Mediator.Send(new GetCallListByIdQuery { Id = id });
            if (callList == null)
                return NotFound();
            return Ok(callList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCallLists([FromQuery] GetAllCallListsQuery query)
        {
            var callLists = await Mediator.Send(query);
            return Ok(callLists);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCallList(int id, [FromBody] UpdateCallListCommand command)
        {
            if (id != command.Id)
                return BadRequest("CallList ID mismatch");

            var result = await Mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCallList(int id)
        {
            var result = await Mediator.Send(new DeleteCallListCommand { Id = id });
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/assign-employee")]
        public async Task<IActionResult> AssignEmployee(int id, [FromBody] AssignCallListEmployeeCommand command)
        {
            if (id != command.Id)
                return BadRequest("CallList ID mismatch");

            var result = await Mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/call-response")]
        public async Task<IActionResult> EnterCallResponse(int id, [FromBody] EnterCallResponseCommand command)
        {
            if (id != command.Id)
                return BadRequest("CallList ID mismatch");

            var result = await Mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetCallListReport([FromQuery] GetCallListReportQuery query)
        {
            var callLists = await Mediator.Send(query);
            return Ok(callLists);
        }
    }
}
