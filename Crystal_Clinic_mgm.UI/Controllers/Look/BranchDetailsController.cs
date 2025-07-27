using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Look.BranchDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Crystal_Clinic_Mgm.Application.Look.BranchDetail.CreateBranchDetailsHandler;

namespace Crystal_Clinic_Mgm.UI.Controllers.Look
{
    [Authorize]
    [RBAC]

    public class BranchDetailsController : BaseController
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateBranchDetails([FromBody] CreateBranchDetailsCommand command)
        {
            var branchDetailsId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetBranchDetailsById), new { id = branchDetailsId }, null);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBranchDetails([FromBody] UpdateBranchDetailsCommand command,int id)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            if (!result) return NotFound();
            return Ok(result);
        }

        [HttpDelete("deactive/{id}")]
        public async Task<IActionResult> DeactiveBranchDetails(int id)
        {
            var result = await Mediator.Send(new DeleteBranchDetailsCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("active/{id}")]
        public async Task<IActionResult> GetBranchDetailsById(int id)
        {
            var branchDetails = await Mediator.Send(new ActivateBranchDetailsCommand { Id = id });
            return Ok(branchDetails);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetBranchDetails([FromQuery] GetBranchDetailsQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("get-one")]
        public async Task<IActionResult> GetBranchDetails([FromQuery] GetOneBranchDetailsQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }
    }
}