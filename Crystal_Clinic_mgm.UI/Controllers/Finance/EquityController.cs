using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Microsoft.AspNetCore.Authorization;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class EquityController : BaseController
    {
        private readonly IEquityService _equityService;

        public EquityController(IEquityService equityService)
        {
            _equityService = equityService;
        }

        #region Shareholder Management

        [HttpPost("Shareholder")]
        public async Task<IActionResult> CreateShareholder([FromBody] CreateShareholderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _equityService.CreateShareholderAsync(dto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetShareholderById), new { id = result.Data }, result) : BadRequest(result);
        }

        [HttpGet("Shareholder")]
        public async Task<IActionResult> GetAllShareholders(
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _equityService.GetAllShareholdersAsync(isActive, pageNumber, pageSize);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("Shareholder/{id}")]
        public async Task<IActionResult> GetShareholderById(int id)
        {
            var result = await _equityService.GetShareholderByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("Shareholder/{id}")]
        public async Task<IActionResult> UpdateShareholder(int id, [FromBody] UpdateShareholderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Id != id)
                return BadRequest("ID mismatch");

            var result = await _equityService.UpdateShareholderAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Equity Transactions

        [HttpPost("Transaction")]
        public async Task<IActionResult> RecordTransaction([FromBody] CreateEquityTransactionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _equityService.RecordEquityTransactionAsync(dto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetTransactions), new { shareholderId = dto.ShareholderId }, result) : BadRequest(result);
        }

        [HttpGet("Transaction")]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] int? shareholderId = null,
            [FromQuery] int? type = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _equityService.GetEquityTransactionsAsync(shareholderId, type, pageNumber, pageSize);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        #endregion

        #region Reporting

        [HttpGet("Report")]
        public async Task<IActionResult> GetEquityReport([FromQuery] DateTime? asOfDate = null)
        {
            var result = await _equityService.GetEquityReportAsync(asOfDate);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        #endregion
    }
}
