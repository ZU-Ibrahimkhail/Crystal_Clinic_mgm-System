using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices;
using Crystal_Clinic_Mgm.Application.CrystalClinic.Visits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [Authorize]
    [RBAC]
    public class ServiceSesssionsController : BaseController
    {
        // add service-session
        [HttpPut("add-service-session")]
        public async Task<IActionResult> AddServiceSession([FromBody] AddServiceSessionCommand command)
        {
            try
            {
                var success = await Mediator.Send(command);
                return success ? Ok("service session added successfully.") : BadRequest("Failed to add service session.");
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

        // implement a service-session
        [HttpPut("implement-service-session")]
        public async Task<IActionResult> ImplementServiceSession([FromBody] ServiceSessionImpCommand command)
        {
            try
            {
                var success = await Mediator.Send(command);
                return success ? Ok("service session Implemented successfully.") : BadRequest("Failed to add service session.");
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
        // Get service-session Details
        [HttpGet("get-service-session-by-id/{sessionId:int}")]
        public async Task<IActionResult> getServiceSessionById(int sessionId)
        {
            var visitDetails = await Mediator.Send(new GetServiceSessionByIdQuery { sessionId = sessionId });
            return Ok(visitDetails);
        }

        // Get service-session List
        [HttpGet("get-service-session-list")]
        public async Task<IActionResult> getServiceSessionList([FromQuery] ListAllServicesSessionsQuery query)
        {
            var visitList = await Mediator.Send(query);
            return Ok(visitList);
        }

    }
}
