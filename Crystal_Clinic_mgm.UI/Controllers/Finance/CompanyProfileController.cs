using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Crystal_Clinic_Mgm.UI.Controllers.Finance;

[Authorize]
[Route("api/Finance/[controller]")]
[ApiController]
public class CompanyProfileController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([System.Web.Http.FromBody] CompanyProfileDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new SetupCompanyCommand { Dto = dto };
        var result = await Mediator.Send(command);

        return Ok("Company profile created successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> Update([System.Web.Http.FromBody] CompanyProfileDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new UpdateCompanyProfileCommand { Dto = dto };
        var result = await Mediator.Send(command);

        return Ok("Company profile Updated successfully.");
    }
}
