using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Dashboards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.CrystalClinic
{
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves aggregated metrics for the clinic dashboard.
        /// </summary>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Dashboard metrics including visits, sessions, dues, patients, and employees.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboard(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving dashboard metrics with filters: StartDate={StartDate}, EndDate={EndDate}", startDate, endDate);
                var query = new DashboardQuery
                {
                    StartDate = startDate,
                    EndDate = endDate
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard metrics");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }


        /// <summary>
        /// Retrieves a chart of visits grouped by doctor over time.
        /// </summary>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Chart data for visits by doctor.</returns>
        [HttpGet("visits-by-doctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVisitsByDoctorChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving visits by doctor chart with filters: StartDate={StartDate}, EndDate={EndDate}", startDate, endDate);
                var query = new VisitsByDoctorChartQuery
                {
                    StartDateTime = startDate,
                    EndDateTime = endDate
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving visits by doctor chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." + ex.Message});
            }
        }

        /// <summary>
        /// Retrieves a chart of service sessions grouped by employee over time.
        /// </summary>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Chart data for sessions by employee.</returns>
        [HttpGet("sessions-by-employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSessionsByEmployeeChart(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving sessions by employee chart with filters: StartDate={StartDate}, EndDate={EndDate}", startDate, endDate);
                var query = new SessionsByEmployeeChartQuery
                {
                    StartDate = startDate,
                    EndDate = endDate
                };
                var result = await Mediator.Send(query, cancellationToken);
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sessions by employee chart");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." + ex.Message });
            }
        }
    }
}