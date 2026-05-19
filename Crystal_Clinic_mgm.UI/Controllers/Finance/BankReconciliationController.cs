using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [RBAC]
    [Authorize]
    [Route("api/Finance/BankReconciliation")]
    [ApiController]
    public class BankReconciliationController : BaseController
    {
        private readonly IBankReconciliationService _bankReconciliationService;

        public BankReconciliationController(IBankReconciliationService bankReconciliationService)
        {
            _bankReconciliationService = bankReconciliationService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadBankStatement([FromBody] CreateBankStatementImportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bankReconciliationService.UploadBankStatementAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{importId}")]
        public async Task<IActionResult> GetBankStatementImport(int importId)
        {
            var result = await _bankReconciliationService.GetBankStatementImportAsync(importId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBankStatementImports(
            [FromQuery] int? bankAccountId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _bankReconciliationService.GetAllBankStatementImportsAsync(
                bankAccountId, fromDate, toDate, page, pageSize);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{importId}/AutoMatch")]
        public async Task<IActionResult> AutoMatchTransactions(
            int importId,
            [FromQuery] decimal amountTolerance = 0.01m,
            [FromQuery] int dateTolerance = 5)
        {
            var result = await _bankReconciliationService.AutoMatchTransactionsAsync(
                importId, amountTolerance, dateTolerance);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ManualMatch")]
        public async Task<IActionResult> ManualMatchTransaction([FromBody] CreateBankMatchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bankReconciliationService.ManualMatchTransactionAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{importId}/UnmatchedTransactions")]
        public async Task<IActionResult> GetUnmatchedTransactions(int importId)
        {
            var result = await _bankReconciliationService.GetUnmatchedTransactionsAsync(importId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{importId}/Summary")]
        public async Task<IActionResult> GetReconciliationSummary(int importId)
        {
            var result = await _bankReconciliationService.GetReconciliationSummaryAsync(importId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{importId}/Close")]
        public async Task<IActionResult> CloseBankReconciliation(int importId)
        {
            var result = await _bankReconciliationService.CloseBankReconciliationAsync(importId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
