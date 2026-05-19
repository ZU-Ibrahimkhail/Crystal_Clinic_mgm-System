using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices;
using Crystal_Clinic_Mgm.Application.CrystalClinic.Visits;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [Authorize]
    [RBAC]
    public class VisitInstrumentsController : BaseController
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateVisitInstrument([FromBody] VisitInstrumentCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetVisitInstrumentList([FromQuery] GetVisitInstrumentListQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("detail/{id:int}")]
        public async Task<IActionResult> GetVisitInstrumentDetail(int id)
        {
            var result = await Mediator.Send(new GetVisitInstrumentDetailQuery { Id = id });
            return Ok(result);
        }
    }
}
