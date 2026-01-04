using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class AccountsReceivableController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountsReceivableDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateAccountsReceivableCommand { Dto = dto };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] ARStatus? status = null,
            [FromQuery] int? customerId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetAllAccountsReceivableQuery
            {
                Status = status,
                CustomerId = customerId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAccountsReceivableByIdQuery { Id = id };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id}/RecordPayment")]
        public async Task<IActionResult> RecordPayment(int id, [FromBody] CreateReceiptDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.AccountsReceivableId = id;
            var command = new RecordReceiptCommand { Dto = dto };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Issue")]
        public async Task<IActionResult> Issue(int id)
        {
            var command = new IssueAccountsReceivableCommand { AccountsReceivableId = id };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Overdue")]
        public async Task<IActionResult> GetOverdue([FromQuery] int? branchId = null)
        {
            var query = new GetOverdueReceivablesQuery { BranchId = branchId };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("Aging")]
        public async Task<IActionResult> GetAging([FromQuery] DateTime? asOf = null)
        {
            var effectiveDate = asOf ?? DateTime.UtcNow;
            var query = new GetAccountsReceivableAgingQuery { AsOfDate = effectiveDate };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        //[HttpPost("{id}/Attachment")]
        //[Consumes("multipart/form-data")] 
        //public async Task<IActionResult> AttachDocument(int id, [FromForm] IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file provided");

        //    return Ok(new { message = "Document attached successfully", accountsReceivableId = id });
        //}
    }
}
