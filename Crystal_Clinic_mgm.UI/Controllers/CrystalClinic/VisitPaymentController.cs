using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices;
using Crystal_Clinic_Mgm.Application.CrystalClinic.Visits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [Authorize]
    [RBAC]
    public class VisitPaymentController : BaseController
    {
       
        // Get Visit Details
        [HttpGet("get-bill/{visitId:int}")]
        public async Task<IActionResult> getBill(int visitId)
        {
            var visitDetails = await Mediator.Send(new GenerateBillQuery { VisitId = visitId });
            return Ok(visitDetails);
        }

        // Record Payment
        [HttpPost("record-payment")]
        public async Task<IActionResult> RecordVisitPayment([FromBody] RecordVisitPaymentCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result); // Returns VisitDto
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

        // Pay for medications
        [HttpPost("pay-medication")]
        public async Task<IActionResult> PayVisitMedication([FromBody] PayVisitMedicationCommand command)
        {
            try
            {
                var success = await Mediator.Send(command);
                return success ? Ok("Medication payment recorded successfully.") : BadRequest("Failed to record medication payment.");
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
