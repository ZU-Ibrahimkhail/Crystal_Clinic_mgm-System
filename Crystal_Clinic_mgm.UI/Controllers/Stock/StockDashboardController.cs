using Crystal_Clinic_Mgm.Application.BranchStock.Dashboards;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [RBAC]
    [Authorize]
    [Route("api/Stock/Dashboard")]
    [ApiController]
    public class StockDashboardController : BaseController
    {
        private readonly ILogger<StockDashboardController> _logger;

        public StockDashboardController(ILogger<StockDashboardController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Bar chart: Stock movement (In vs Out) by quantity and value grouped by Day/Week/Month/Year.
        /// </summary>
        [HttpGet("stock-movement")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockMovementBarChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            [FromQuery] int? categoryId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving stock movement bar chart");
                var query = new StockMovementBarChartQuery
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    BranchId = branchId,
                    CategoryId = categoryId
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock movement bar chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Line chart: Stock purchase value and sell value trend over time.
        /// </summary>
        [HttpGet("stock-value-trend")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockValueTrendLineChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving stock value trend line chart");
                var query = new StockValueTrendLineChartQuery
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
                _logger.LogError(ex, "Error retrieving stock value trend line chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Pie chart: Stock distribution by item category (by quantity, by value, and by expiry status).
        /// </summary>
        [HttpGet("stock-by-category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockByCategoryPieChart(
            [FromQuery] int? branchId,
            [FromQuery] bool includeExpired = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving stock by category pie chart");
                var query = new StockByCategoryPieChartQuery
                {
                    BranchId = branchId,
                    IncludeExpired = includeExpired
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock by category pie chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Spider chart: Supplier performance metrics (due amounts, paid, remaining, stock supplied).
        /// </summary>
        [HttpGet("supplier-performance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSupplierPerformanceSpiderChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving supplier performance spider chart");
                var query = new SupplierPerformanceSpiderChartQuery
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
                _logger.LogError(ex, "Error retrieving supplier performance spider chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }
    }
}
