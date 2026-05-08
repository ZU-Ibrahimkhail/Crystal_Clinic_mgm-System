using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Finance
{
    [Authorize]
    [Route("api/Finance/[controller]")]
    [ApiController]
    public class ProcurementController : BaseController
    {
        private readonly IProcurementService _procurementService;

        public ProcurementController(IProcurementService procurementService)
        {
            _procurementService = procurementService;
        }

        #region Purchase Order Endpoints

        [HttpPost("PurchaseOrder")]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _procurementService.CreatePurchaseOrderAsync(dto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetPurchaseOrderById), new { id = result.Data }, result) : BadRequest(result);
        }

        [HttpGet("PurchaseOrder")]
        public async Task<IActionResult> GetAllPurchaseOrders(
            [FromQuery] int? vendorId = null,
            [FromQuery] int? status = null,
            [FromQuery] int? branchId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _procurementService.GetAllPurchaseOrdersAsync(vendorId, status, branchId, pageNumber, pageSize);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("PurchaseOrder/{id}")]
        public async Task<IActionResult> GetPurchaseOrderById(int id)
        {
            var result = await _procurementService.GetPurchaseOrderByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("PurchaseOrder/{id}")]
        public async Task<IActionResult> UpdatePurchaseOrder(int id, [FromBody] UpdatePurchaseOrderDto dto)
        {
            dto.Id = id; // Ensure the ID from the URL is set in the DTO for validation
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data.", errors = ModelState });

            if (dto.Id != id)
                return BadRequest(new { success = false, message = "ID mismatch: URL ID does not match request body ID." });

            var result = await _procurementService.UpdatePurchaseOrderAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(new { success = false, error = result.Error });
        }

        [HttpPost("PurchaseOrder/{id}/Receive")]
        public async Task<IActionResult> ReceivePurchaseOrder(int id)
        {
            var result = await _procurementService.ReceivePurchaseOrderAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Vendor Bill Endpoints

        [HttpPost("VendorBill")]
        public async Task<IActionResult> CreateVendorBill([FromBody] CreateVendorBillDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _procurementService.CreateVendorBillAsync(dto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetVendorBillById), new { id = result.Data }, result) : BadRequest(result);
        }

        [HttpGet("VendorBill")]
        public async Task<IActionResult> GetAllVendorBills(
            [FromQuery] int? vendorId = null,
            [FromQuery] int? status = null,
            [FromQuery] int? branchId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _procurementService.GetAllVendorBillsAsync(vendorId, status, branchId, pageNumber, pageSize);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("VendorBill/{id}")]
        public async Task<IActionResult> GetVendorBillById(int id)
        {
            var result = await _procurementService.GetVendorBillByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPut("VendorBill/{id}")]
        public async Task<IActionResult> UpdateVendorBill(int id, [FromBody] UpdateVendorBillDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data.", errors = ModelState });

            if (dto.Id != id)
                return BadRequest(new { success = false, message = "ID mismatch: URL ID does not match request body ID." });

            var result = await _procurementService.UpdateVendorBillAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(new { success = false, error = result.Error });
        }

        //[HttpPost("VendorBill/{id}/MarkAsPaid")]
        //public async Task<IActionResult> MarkVendorBillAsPaid(int id)
        //{
        //    var result = await _procurementService.MarkVendorBillAsPaidAsync(id);
        //    return result.IsSuccess ? Ok(result) : BadRequest(result);
        //}

        #endregion
    }
}
