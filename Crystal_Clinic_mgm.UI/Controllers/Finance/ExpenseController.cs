using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [RBAC]
    [Authorize]
    [Route("api/Finance/Expense")]
    [ApiController]
    public class ExpenseController : BaseController
    {
        private readonly IExpenseService _expenseService;
        private readonly ILoggedInUser _loggedInUser;
        public ExpenseController(IExpenseService expenseService, ILoggedInUser loggedInUser)
        {
            _expenseService = expenseService;
            _loggedInUser = loggedInUser;
        }


        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _expenseService.CreateExpenseAsync(dto, _loggedInUser.Id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.Id = id;
            var result = await _expenseService.UpdateExpenseAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var result = await _expenseService.GetExpenseAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses(
            [FromQuery] ExpenseStatus? status = null,
            [FromQuery] int? branchId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _expenseService.GetAllExpensesAsync(
                status, branchId, fromDate, toDate, page, pageSize);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Submit")]
        public async Task<IActionResult> SubmitExpense(int id)
        {
            var result = await _expenseService.SubmitExpenseAsync(id, _loggedInUser.Id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Approve")]
        public async Task<IActionResult> ApproveExpense(int id)
        {
            var result = await _expenseService.ApproveExpenseAsync(id, _loggedInUser.Id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Reject")]
        public async Task<IActionResult> RejectExpense(int id, [FromBody] RejectExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _expenseService.RejectExpenseAsync(id, _loggedInUser.Id, dto.RejectionReason);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Pending")]
        public async Task<IActionResult> GetPendingApprovals(
            [FromQuery] int? branchId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _expenseService.GetPendingApprovalsAsync(branchId, page, pageSize);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
