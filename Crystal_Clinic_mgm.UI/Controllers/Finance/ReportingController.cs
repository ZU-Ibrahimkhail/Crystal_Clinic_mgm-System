using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/Reporting")]
    [ApiController]
    public class ReportingController : BaseController
    {
        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard([FromQuery] DateTime? asOf = null)
        {
            var effectiveDate = asOf ?? DateTime.UtcNow;

            var trialBalanceQuery = new GetTrialBalanceQuery
            {
                AsOfDate = effectiveDate
            };

            var trialBalance = await Mediator.Send(trialBalanceQuery);

            return Ok(new
            {
                asOfDate = effectiveDate,
                trialBalance = trialBalance.Data,
                kpis = new
                {
                    totalAssets = 0m,
                    totalLiabilities = 0m,
                    totalEquity = 0m,
                    netIncome = 0m
                }
            });
        }

        [HttpGet("TrialBalance")]
        public async Task<IActionResult> GetTrialBalance(
            [FromQuery] DateTime? asOf = null,
            [FromQuery] DateTime? compareTo = null,
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

        [HttpPost("TrialBalance/Export")]
        public async Task<IActionResult> ExportTrialBalance([FromBody] TrialBalanceExportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { message = "Trial balance export initiated", status = "pending", format = request.Format });
        }

        [HttpGet("BalanceSheet")]
        public async Task<IActionResult> GetBalanceSheet(
            [FromQuery] DateTime? asOf = null,
            [FromQuery] int? branchId = null)
        {
            var effectiveDate = asOf ?? DateTime.UtcNow;

            var query = new GetBalanceSheetQuery
            {
                AsOfDate = effectiveDate,
                BranchId = branchId
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("BalanceSheet/Export")]
        public async Task<IActionResult> ExportBalanceSheet([FromBody] BalanceSheetExportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { message = "Balance sheet export initiated", status = "pending", format = request.Format });
        }

        [HttpGet("ARAgingReport")]
        public async Task<IActionResult> GetARAgingReport(
            [FromQuery] DateTime? asOf = null,
            [FromQuery] int? branchId = null)
        {
            var effectiveDate = asOf ?? DateTime.UtcNow;

            var query = new GetAccountsReceivableAgingQuery
            {
                AsOfDate = effectiveDate,
                BranchId = branchId
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ARAgingReport/Export")]
        public async Task<IActionResult> ExportARAgingReport([FromBody] ARAgingExportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { message = "AR aging report export initiated", status = "pending", format = request.Format });
        }

        [HttpGet("APAgingReport")]
        public async Task<IActionResult> GetAPAgingReport(
            [FromQuery] DateTime? asOf = null,
            [FromQuery] int? branchId = null)
        {
            var effectiveDate = asOf ?? DateTime.UtcNow;

            var query = new GetAccountsPayableAgingQuery
            {
                AsOfDate = effectiveDate,
                BranchId = branchId
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("APAgingReport/Export")]
        public async Task<IActionResult> ExportAPAgingReport([FromBody] APAgingExportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { message = "AP aging report export initiated", status = "pending", format = request.Format });
        }

        [HttpGet("IncomeStatement")]
        public async Task<IActionResult> GetIncomeStatement(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int? branchId = null)
        {
            var startDate = fromDate ?? DateTime.Now.AddMonths(-1);
            var endDate = toDate ?? DateTime.Now;

            var query = new GetIncomeStatementQuery
            {
                FromDate = startDate,
                ToDate = endDate,
                BranchId = branchId
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("IncomeStatement/Compare")]
        public async Task<IActionResult> CompareIncomeStatements(
            [FromQuery] DateTime fromDate1,
            [FromQuery] DateTime toDate1,
            [FromQuery] DateTime fromDate2,
            [FromQuery] DateTime toDate2,
            [FromQuery] int? branchId = null)
        {
            var query = new CompareIncomeStatementsQuery
            {
                FromDate1 = fromDate1,
                ToDate1 = toDate1,
                FromDate2 = fromDate2,
                ToDate2 = toDate2,
                BranchId = branchId
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("IncomeStatement/Export")]
        public async Task<IActionResult> ExportIncomeStatement([FromBody] IncomeStatementExportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { message = "Income statement export initiated", status = "pending", format = request.Format });
        }

        [HttpPost("Export")]
        public async Task<IActionResult> Export([FromBody] FinancialReportExportRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new { message = "Export initiated", status = "pending", format = request.Format, reportType = request.ReportType });
        }
    }

    public class IncomeStatementExportRequest
    {
        public string Format { get; set; } = "Excel";
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class TrialBalanceExportRequest
    {
        public string Format { get; set; } = "Excel";
        public DateTime? AsOfDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class BalanceSheetExportRequest
    {
        public string Format { get; set; } = "Excel";
        public DateTime? AsOfDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class ARAgingExportRequest
    {
        public string Format { get; set; } = "Excel";
        public DateTime? AsOfDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class APAgingExportRequest
    {
        public string Format { get; set; } = "Excel";
        public DateTime? AsOfDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class FinancialReportExportRequest
    {
        public string ReportType { get; set; } = "TrialBalance";
        public string Format { get; set; } = "Excel";
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
