using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class JournalEntryController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJournalEntryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateJournalEntryCommand { Dto = dto };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] JournalEntryStatus? status = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetAllJournalEntriesQuery
            {
                Status = status,
                FromDate = fromDate,
                ToDate = toDate,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetJournalEntryByIdQuery { Id = id };
            var result = await Mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id}/Post")]
        public async Task<IActionResult> Post(int id, [FromQuery] bool dryRun = false)
        {
            if (dryRun)
            {
                return Ok(new { message = "Validation successful", success = true });
            }

            var command = new PostJournalEntryCommand { JournalEntryId = id };
            var result = await Mediator.Send(command);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Void")]
        public async Task<IActionResult> Void(int id, [FromBody] VoidJournalEntryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new VoidJournalEntryCommand
            {
                JournalEntryId = id,
                Reason = request.Reason
            };

            var result = await Mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        //[HttpPost("{id}/Attachment")]
        //[Consumes("multipart/form-data")] 
        //public async Task<IActionResult> AttachDocument(int id, [FromForm] IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file provided");

        //    return Ok(new { message = "Document attached successfully", journalEntryId = id });
        //}
    }

    public class VoidJournalEntryRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}
