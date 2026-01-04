using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class GeneralLedgerController : BaseController
    {
        [HttpGet("AccountLedger/{chartOfAccountId}")]
        public async Task<IActionResult> GetAccountLedger(
            int chartOfAccountId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var from = fromDate ?? DateTime.UtcNow.AddYears(-1);
            var to = toDate ?? DateTime.UtcNow;

            var query = new GetAccountLedgerQuery
            {
                ChartOfAccountId = chartOfAccountId,
                FromDate = from,
                ToDate = to
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("TrialBalance")]
        public async Task<IActionResult> GetTrialBalance(
            [FromQuery] DateTime? asOf = null,
            [FromQuery] int? branchId = null)
        {
            var effectiveDate = asOf ?? DateTime.UtcNow;

            var query = new GetTrialBalanceQuery
            {
                AsOfDate = effectiveDate,
                BranchId = branchId
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("AccountBalance/{chartOfAccountId}")]
        public async Task<IActionResult> GetAccountBalance(
            int chartOfAccountId,
            [FromQuery] DateTime? asOf = null)
        {
            var effectiveDate = asOf ?? DateTime.UtcNow;

            var query = new GetAccountBalanceQuery
            {
                ChartOfAccountId = chartOfAccountId,
                AsOfDate = effectiveDate
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("Export")]
        public async Task<IActionResult> Export([FromBody] GeneralLedgerExportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { message = "Export initiated", status = "pending", format = request.Format });
        }
    }

    public class GeneralLedgerExportRequest
    {
        public string Format { get; set; } = "Excel";
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? ChartOfAccountId { get; set; }
    }
}
