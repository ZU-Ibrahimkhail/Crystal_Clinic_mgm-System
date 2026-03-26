using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProcurementController : ControllerBase
    {
        private readonly IProcurementService _procurementService;

        public ProcurementController(IProcurementService procurementService)
        {
            _procurementService = procurementService;
        }

        /// <summary>
        /// Create a purchase order
        /// </summary>
        [HttpPost("po")]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderDto request)
        {
            var result = await _procurementService.CreatePurchaseOrderAsync(request);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Receive items for a purchase order
        /// </summary>
        [HttpPost("po/{purchaseOrderId}/receive")]
        public async Task<IActionResult> ReceivePurchaseOrder(int purchaseOrderId)
        {
            var result = await _procurementService.ReceivePurchaseOrderAsync(purchaseOrderId);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get all purchase orders with filtering
        /// </summary>
        [HttpGet("po")]
        public async Task<IActionResult> GetPurchaseOrders(
            [FromQuery] int? vendorId,
            [FromQuery] int? status,
            [FromQuery] int? branchId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _procurementService.GetAllPurchaseOrdersAsync(
                vendorId, status, branchId, pageNumber, pageSize);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get purchase order by ID
        /// </summary>
        [HttpGet("po/{id}")]
        public async Task<IActionResult> GetPurchaseOrder(int id)
        {
            var result = await _procurementService.GetPurchaseOrderByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result.Data);
            return NotFound(result.Error);
        }

        /// <summary>
        /// Create a vendor bill
        /// </summary>
        [HttpPost("bill")]
        public async Task<IActionResult> CreateVendorBill([FromBody] CreateVendorBillDto request)
        {
            var result = await _procurementService.CreateVendorBillAsync(request);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Mark vendor bill as paid
        /// </summary>
        [HttpPost("bill/{vendorBillId}/pay")]
        public async Task<IActionResult> MarkBillAsPaid(int vendorBillId, decimal paymentAmount, int paymentMethodId, string reference)
        {
            var result = await _procurementService.PayVendorBillAsync(vendorBillId, paymentAmount, paymentMethodId, reference);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get all vendor bills with filtering
        /// </summary>
        [HttpGet("bills")]
        public async Task<IActionResult> GetVendorBills(
            [FromQuery] int? vendorId,
            [FromQuery] int? status,
            [FromQuery] int? branchId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _procurementService.GetAllVendorBillsAsync(
                vendorId, status, branchId, pageNumber, pageSize);
            if (result.IsSuccess)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }
    }
}