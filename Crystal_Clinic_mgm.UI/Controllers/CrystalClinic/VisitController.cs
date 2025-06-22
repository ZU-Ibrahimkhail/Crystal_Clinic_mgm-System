using Crystal_Clinic_Mgm.Application.CrystalClinic.Visits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [Authorize]
    public class VisitController : BaseController
    {
        #region Create Visit
        [HttpPost("create")]
        public async Task<IActionResult> CreateVisit([FromBody] CreateVisitCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
        #endregion

        #region Update Visit
        [HttpPut("update")]
        public async Task<IActionResult> UpdateVisit([FromBody] UpdateVisitCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
        #endregion

        #region Delete Visit
        [HttpDelete("delete/{visitId:int}")]
        public async Task<IActionResult> DeleteVisit(int visitId)
        {
            var success = await Mediator.Send(new DeleteVisitCommand { VisitId = visitId });
            return success ? Ok("Visit deleted.") : NotFound("Visit not found.");
        }
        #endregion

        #region Get Visit Details
        [HttpGet("details/{visitId:int}")]
        public async Task<IActionResult> GetVisitDetails(int visitId)
        {
            var visitDetails = await Mediator.Send(new GetVisitDetailsQuery { VisitId = visitId });
            return Ok(visitDetails);
        }
        #endregion

        #region Get Visit List
        [HttpGet("list")]
        public async Task<IActionResult> GetVisitList([FromQuery] GetVisitListQuery query)
        {
            var visitList = await Mediator.Send(query);
            return Ok(visitList);
        }
        #endregion

        #region Add Medications to Visit
        [HttpPost("add-medications")]
        public async Task<IActionResult> AddVisitMedications([FromBody] AddVisitMedicationCommand command)
        {
            var success = await Mediator.Send(command);
            return success ? Ok("Medications added to visit.") : BadRequest("Failed to add medications.");
        }
        #endregion

        #region Add Services to Visit
        [HttpPost("add-services")]
        public async Task<IActionResult> AddVisitServices([FromBody] AddVisitServiceCommand command)
        {
            var success = await Mediator.Send(command);
            return success ? Ok("Services added to visit.") : BadRequest("Failed to add services.");
        }
        #endregion

        #region Record Payment
        [HttpPost("record-payment")]
        public async Task<IActionResult> RecordVisitPayment([FromBody] RecordVisitPaymentCommand command)
        {
            var visitDto = await Mediator.Send(command);
            return Ok(visitDto);
        }
        #endregion
    }
}