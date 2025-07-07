using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices;
using Crystal_Clinic_Mgm.Application.CrystalClinic.Visits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [Authorize]
    [RBAC]
    public class VisitController : BaseController
    {
        // Create Visit
        [HttpPost("create")]
        public async Task<IActionResult> CreateVisit([FromBody] CreateVisitCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }


        // Update Visit
        [HttpPut("update")]
        public async Task<IActionResult> UpdateVisit([FromBody] UpdateVisitCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }


        // Delete Visit
        [HttpDelete("delete/{visitId:int}")]
        public async Task<IActionResult> DeleteVisit(int visitId)
        {
            var success = await Mediator.Send(new DeleteVisitCommand { VisitId = visitId });
            return success ? Ok("Visit deleted.") : NotFound("Visit not found.");
        }


        // Get Visit Details
        [HttpGet("details/{visitId:int}")]
        public async Task<IActionResult> GetVisitDetails(int visitId)
        {
            var visitDetails = await Mediator.Send(new GetVisitDetailsQuery { VisitId = visitId });
            return Ok(visitDetails);
        }


        // Get Visit List
        [HttpGet("list")]
        public async Task<IActionResult> GetVisitList([FromQuery] GetVisitListQuery query)
        {
            var visitList = await Mediator.Send(query);
            return Ok(visitList);
        }


        // Add Medications to Visit
        [HttpPost("add-medications")]
        public async Task<IActionResult> AddVisitMedications([FromBody] AddVisitMedicationCommand command)
        {
            var success = await Mediator.Send(command);
            return success ? Ok("Medications added to visit.") : BadRequest("Failed to add medications.");
        }


        // Add Services to Visit
        [HttpPost("add-services")]
        public async Task<IActionResult> AddVisitServices([FromBody] AddVisitServiceCommand command)
        {
            var success = await Mediator.Send(command);
            return success ? Ok("Services added to visit.") : BadRequest("Failed to add services.");
        }


        // Change visit status
        [HttpPut("status")]
        public async Task<IActionResult> ChangeVisitStatus([FromBody] ChangeVisitStatusCommand command)
        {
            try
            {
                var success = await Mediator.Send(command);
                return success ? Ok("Visit status updated successfully.") : BadRequest("Failed to update visit status.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Postpone a visit
        [HttpPut("postpone")]
        public async Task<IActionResult> PostponeVisit([FromBody] PostponeVisitCommand command)
        {
            try
            {
                var success = await Mediator.Send(command);
                return success ? Ok("Visit postponed successfully.") : BadRequest("Failed to postpone visit.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Remove a payment, medication, or service
        [HttpDelete("item")]
        public async Task<IActionResult> RemoveVisitItem([FromBody] RemoveVisitItemCommand command)
        {
            try
            {
                var success = await Mediator.Send(command);
                return success ? Ok("Item removed successfully.") : BadRequest("Failed to remove item.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
