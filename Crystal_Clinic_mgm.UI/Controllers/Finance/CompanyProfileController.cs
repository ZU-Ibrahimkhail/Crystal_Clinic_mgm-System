using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance;

[RBAC]
[Authorize]
[Route("api/Finance/[controller]")]
[ApiController]
public class CompanyProfileController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CompanyProfileDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new SetupCompanyCommand { Dto = dto };
        var result = await Mediator.Send(command);

        return Ok("Company profile created successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = new GetCompanyProfileQuery();
        var result = await Mediator.Send(query);

        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CompanyProfileDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new UpdateCompanyProfileCommand { Dto = dto };
        var result = await Mediator.Send(command);

        return Ok("Company profile Updated successfully.");
    }
}
