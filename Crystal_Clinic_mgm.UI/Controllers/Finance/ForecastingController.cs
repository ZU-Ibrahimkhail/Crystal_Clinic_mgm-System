using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [RBAC]
    [Authorize]
    [Route("api/Finance/Forecasting")]
    [ApiController]
    public class ForecastingController : BaseController
    {
        private readonly IForecastingService _forecastingService;
        private readonly ILoggedInUser _loggedInUser;

        public ForecastingController(IForecastingService forecastingService, ILoggedInUser loggedInUser)
        {
            _forecastingService = forecastingService;
            _loggedInUser = loggedInUser;
        }


        [HttpPost]
        public async Task<IActionResult> CreateForecast([FromBody] CreateForecastSnapshotDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _forecastingService.CreateForecastAsync(dto, _loggedInUser.Id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForecast(int id, [FromBody] UpdateForecastSnapshotDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.Id = id;
            var result = await _forecastingService.UpdateForecastAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForecast(int id)
        {
            var result = await _forecastingService.GetForecastAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForecasts(
            [FromQuery] string? scenario = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _forecastingService.GetAllForecastsAsync(scenario, page, pageSize);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Scenario/{scenario}")]
        public async Task<IActionResult> GetForecastByScenario(string scenario)
        {
            var result = await _forecastingService.GetForecastByScenarioAsync(scenario);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id}/Submit")]
        public async Task<IActionResult> SubmitForecast(int id)
        {
            var result = await _forecastingService.SubmitForecastAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Approve")]
        public async Task<IActionResult> ApproveForecast(int id)
        {
            var result = await _forecastingService.ApproveForecastAsync(id, _loggedInUser.Id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Export")]
        public async Task<IActionResult> ExportForecast(int id, [FromQuery] string format = "Excel")
        {
            var result = await _forecastingService.GetForecastAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(new { message = "Forecast export initiated", status = "pending", format, data = result.Data });
        }
    }
}
