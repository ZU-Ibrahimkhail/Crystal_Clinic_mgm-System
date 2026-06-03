using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [RBAC]
    [Authorize]
    [Route("api/CrystalClinic/Dashboard")]
    [ApiController]
    public class ClinicServicesDashboardController : BaseController
    {
        private readonly ILogger<ClinicServicesDashboardController> _logger;

        public ClinicServicesDashboardController(ILogger<ClinicServicesDashboardController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Bar chart: Service revenue per service type grouped by Day/Week/Month/Year.
        /// </summary>
        [HttpGet("service-revenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetServiceRevenueBarChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving service revenue bar chart");
                var query = new ServiceRevenueBarChartQuery
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
                _logger.LogError(ex, "Error retrieving service revenue bar chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Line chart: Patient visit trend (visits, revenue, unique patients) over time.
        /// </summary>
        [HttpGet("visit-trend")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientVisitTrendLineChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving patient visit trend line chart");
                var query = new PatientVisitTrendLineChartQuery
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
                _logger.LogError(ex, "Error retrieving patient visit trend line chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Pie chart: Visit status distribution, revenue by status, payment completion, and session completion.
        /// </summary>
        [HttpGet("visit-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVisitStatusPieChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving visit status pie chart");
                var query = new VisitStatusPieChartQuery
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
                _logger.LogError(ex, "Error retrieving visit status pie chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Spider chart: Doctor performance metrics (visits, completed, pending, revenue, patients).
        /// </summary>
        [HttpGet("doctor-performance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctorPerformanceSpiderChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? branchId,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving doctor performance spider chart");
                var query = new DoctorPerformanceSpiderChartQuery
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
                _logger.LogError(ex, "Error retrieving doctor performance spider chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }
    }
}
