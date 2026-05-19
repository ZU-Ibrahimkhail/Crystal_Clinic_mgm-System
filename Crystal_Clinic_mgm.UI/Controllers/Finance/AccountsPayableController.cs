using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [RBAC]
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class AccountsPayableController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountsPayableDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateAccountsPayableCommand { Dto = dto };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data }, result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] APStatus? status = null,
            [FromQuery] int? vendorId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetAllAccountsPayableQuery
            {
                Status = status,
                VendorId = vendorId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAccountsPayableByIdQuery { Id = id };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id}/Approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var command = new ApproveAccountsPayableCommand { AccountsPayableId = id };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/MarkForPayment")]
        public async Task<IActionResult> MarkForPayment(int id)
        {
            var command = new MarkAccountsPayableForPaymentCommand { AccountsPayableId = id };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/RecordPayment")]
        public async Task<IActionResult> RecordPayment(int id, [FromBody] CreatePaymentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.AccountsPayableId = id;
            var command = new RecordPaymentCommand { Dto = dto };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Overdue")]
        public async Task<IActionResult> GetOverdue([FromQuery] int? branchId = null)
        {
            var query = new GetOverduePayablesQuery { BranchId = branchId };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("Aging")]
        public async Task<IActionResult> GetAging([FromQuery] DateTime? asOf = null)
        {
            var effectiveDate = asOf ?? DateTime.UtcNow;
            var query = new GetAccountsPayableAgingQuery { AsOfDate = effectiveDate };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        //[HttpPost("{id}/Attachment")]
        //[Consumes("multipart/form-data")] 
        //public async Task<IActionResult> AttachDocument(int id, [FromForm] IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file provided");

        //    return Ok(new { message = "Document attached successfully", accountsPayableId = id });
        //}
    }
}
