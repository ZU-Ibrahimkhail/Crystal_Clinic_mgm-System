using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly ILoggedInUser _loggedInUser;

        public ExpenseController(
            IExpenseService expenseService,
            ILoggedInUser currentUserService)
        {
            _expenseService = expenseService;
            _loggedInUser = currentUserService;
        }

        /// <summary>
        /// Create a new expense
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto request)
        {
            var result = await _expenseService.CreateExpenseAsync(request, _loggedInUser.Id);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Update an existing expense
        /// </summary>
        [HttpPut("{expenseId}")]
        public async Task<IActionResult> UpdateExpense(int expenseId, [FromBody] UpdateExpenseDto request)
        {
            request.ExpenseId = expenseId;
            var result = await _expenseService.UpdateExpenseAsync(request);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Submit expense for approval
        /// </summary>
        [HttpPost("{expenseId}/submit")]
        public async Task<IActionResult> SubmitExpense(int expenseId)
        {
            var result = await _expenseService.SubmitExpenseAsync(expenseId, _loggedInUser.Id);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Approve an expense
        /// </summary>
        [HttpPost("{expenseId}/approve")]
        public async Task<IActionResult> ApproveExpense(int expenseId)
        {
            var result = await _expenseService.ApproveExpenseAsync(expenseId, _loggedInUser.Id);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Reject an expense
        /// </summary>
        [HttpPost("{expenseId}/reject")]
        public async Task<IActionResult> RejectExpense(int expenseId, [FromBody] RejectExpenseRequest request)
        {
            var result = await _expenseService.RejectExpenseAsync(expenseId, _loggedInUser.Id, request.Reason);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get expense by ID
        /// </summary>
        [HttpGet("{expenseId}")]
        public async Task<IActionResult> GetExpense(int expenseId)
        {
            var result = await _expenseService.GetExpenseAsync(expenseId);
            if (result.IsSuccess)
                return Ok(result.Data);
            return NotFound(result.Error);
        }

        /// <summary>
        /// Get all expenses with filtering
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetExpenses(
            [FromQuery] ExpenseStatus? status,
            [FromQuery] int? branchId,
            [FromQuery] int? categoryId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _expenseService.GetAllExpensesAsync(
                status, branchId, categoryId, fromDate, toDate, page, pageSize);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get pending expense approvals
        /// </summary>
        [HttpGet("pending-approvals")]
        public async Task<IActionResult> GetPendingApprovals(
            [FromQuery] int? branchId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _expenseService.GetPendingApprovalsAsync(branchId, page, pageSize);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }
    }

    public class RejectExpenseRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}