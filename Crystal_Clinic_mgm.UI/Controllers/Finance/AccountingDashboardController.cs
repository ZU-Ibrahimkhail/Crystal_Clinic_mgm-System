using Crystal_Clinic_Mgm.Application.Accounting.Dashboards;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [RBAC]
    [Authorize]
    [Route("api/Finance/Dashboard")]
    [ApiController]
    public class AccountingDashboardController : BaseController
    {
        private readonly ILogger<AccountingDashboardController> _logger;

        public AccountingDashboardController(ILogger<AccountingDashboardController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Bar chart: Revenue vs Expenses comparison grouped by Day/Week/Month/Year.
        /// </summary>
        [HttpGet("revenue-vs-expenses")]
        [ProducesResponseType(typeof(MultiSeriesChartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRevenueVsExpensesBarChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving revenue vs expenses bar chart");
                var query = new AccountingRevenueExpensesBarChartQuery
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    BranchId = branchId
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving revenue vs expenses bar chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Line chart: Accounts Receivable vs Accounts Payable balance trend over time.
        /// </summary>
        [HttpGet("ar-vs-ap-trend")]
        [ProducesResponseType(typeof(MultiSeriesChartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetARvsAPLineChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving AR vs AP line chart");
                var query = new AccountingARvsAPLineChartQuery
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    BranchId = branchId
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving AR vs AP line chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Pie chart: Expense distribution by chart of account category and AP type.
        /// </summary>
        [HttpGet("expenses-by-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpensesByCategoryPieChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving expenses by category pie chart");
                var query = new AccountingExpensesByCategoryPieChartQuery
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    BranchId = branchId
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expenses by category pie chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Spider chart: Multi-axis financial KPIs (Revenue, Expenses, AR, AP, collected, paid).
        /// </summary>
        [HttpGet("financial-kpis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFinancialKPIsSpiderChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving financial KPIs spider chart");
                var query = new AccountingFinancialKPIsSpiderChartQuery
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    BranchId = branchId
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial KPIs spider chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }
    }
}
