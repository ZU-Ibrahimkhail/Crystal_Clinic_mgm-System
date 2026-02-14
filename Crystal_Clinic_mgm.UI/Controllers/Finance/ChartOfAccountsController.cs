using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class ChartOfAccountsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChartOfAccountsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateChartOfAccountsCommand { Dto = dto };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data }, result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool? isActive = null,
            [FromQuery] int? accountType = null,
            [FromQuery] string? searchText = null)
        {
            var query = new GetAllChartOfAccountsQuery
            {
                IsActive = isActive,
                AccountType = accountType,
                SearchText = searchText
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetChartOfAccountsByIdQuery { Id = id };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("ByType/{accountType}")]
        public async Task<IActionResult> GetByType(int accountType)
        {
            var query = new GetAccountsByTypeQuery { AccountType = accountType };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateChartOfAccountsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Id != id)
                return BadRequest("ID mismatch");

            var command = new UpdateChartOfAccountsCommand { Dto = dto };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var dto = new UpdateChartOfAccountsDto { Id = id, IsActive = false };
            var command = new UpdateChartOfAccountsCommand { Dto = dto };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteChartOfAccountsCommand { Id = id };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Move")]
        public async Task<IActionResult> MoveAccount(int id, [FromQuery] int? newParentAccountId)
        {
            var command = new MoveChartOfAccountCommand { AccountId = id, NewParentAccountId = newParentAccountId };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Hierarchy")]
        public async Task<IActionResult> GetHierarchy([FromQuery] int? parentAccountId = null)
        {
            var query = new GetAccountHierarchyQuery { ParentAccountId = parentAccountId };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
