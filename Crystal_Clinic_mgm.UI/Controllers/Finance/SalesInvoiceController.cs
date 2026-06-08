using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [RBAC]
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class SalesInvoiceController : BaseController
    {
        private readonly ISalesInvoiceService _salesInvoiceService;

        public SalesInvoiceController(ISalesInvoiceService salesInvoiceService)
        {
            _salesInvoiceService = salesInvoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSalesInvoiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _salesInvoiceService.CreateSalesInvoiceAsync(dto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data }, result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? customerId = null,
            [FromQuery] int? status = null,
            [FromQuery] int? branchId = null,
            [FromQuery] int? visitId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _salesInvoiceService.GetAllSalesInvoicesAsync(visitId,customerId, status, branchId, pageNumber, pageSize);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _salesInvoiceService.GetSalesInvoiceByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id}/Issue")]
        public async Task<IActionResult> Issue(int id)
        {
            var result = await _salesInvoiceService.IssueSalesInvoiceAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Receipt")]
        public async Task<IActionResult> RecordReceipt(int id, [FromBody] CreateSalesReceiptDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.SalesInvoiceId != id)
                return BadRequest("Sales Invoice ID mismatch");

            var result = await _salesInvoiceService.RecordSalesReceiptAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}/Receipts")]
        public async Task<IActionResult> GetReceipts(int id)
        {
            var result = await _salesInvoiceService.GetSalesReceiptsByInvoiceAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _salesInvoiceService.DeleteSalesInvoiceAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Void")]
        public async Task<IActionResult> Void(int id, [FromBody] VoidSalesInvoiceRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _salesInvoiceService.VoidSalesInvoiceAsync(id, dto.Reason);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{id}/Refund")]
        public async Task<IActionResult> Refund(int id, [FromBody] RefundSalesInvoiceRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.RefundAmount <= 0)
                return BadRequest("Refund amount must be greater than zero.");

            var result = await _salesInvoiceService.RefundSalesInvoiceAsync(
                id, 
                dto.RefundAmount, 
                dto.Reason, 
                dto.PaymentMethod);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
