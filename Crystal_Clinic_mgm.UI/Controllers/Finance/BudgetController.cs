using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Microsoft.AspNetCore.Authorization;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [RBAC]
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class BudgetController : BaseController
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBudgetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _budgetService.CreateBudgetAsync(dto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data }, result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? fiscalYear = null,
            [FromQuery] int? status = null,
            [FromQuery] int? branchId = null)
        {
            var result = await _budgetService.GetAllBudgetsAsync(fiscalYear, status, branchId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _budgetService.GetBudgetVarianceAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id}/Approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _budgetService.ApproveBudgetAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _budgetService.ActivateBudgetAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}/Variance")]
        public async Task<IActionResult> GetVariance(int id)
        {
            var result = await _budgetService.GetBudgetVarianceAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}/VarianceReport")]
        public async Task<IActionResult> GetVarianceReport(
            int id,
            [FromQuery] decimal? varianceThresholdAmount = null,
            [FromQuery] decimal? varianceThresholdPercentage = null,
            [FromQuery] int? accountType = null,
            [FromQuery] string? sortBy = "AccountCode",
            [FromQuery] bool sortDescending = false,
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null)
        {
            var query = new GetBudgetVarianceQuery
            {
                BudgetId = id,
                VarianceThresholdAmount = varianceThresholdAmount,
                VarianceThresholdPercentage = varianceThresholdPercentage,
                AccountType = accountType.HasValue ? (AccountType)accountType.Value : null,
                SortBy = sortBy,
                SortDescending = sortDescending,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
