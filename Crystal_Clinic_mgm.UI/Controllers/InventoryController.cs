using Crystal_Clinic_Mgm.Application.BranchStock;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILoggedInUser _loggedInUser;

        public InventoryController(
            IInventoryService inventoryService,
            ILoggedInUser loggedInUser)
        {
            _inventoryService = inventoryService;
            _loggedInUser = loggedInUser;
        }

        /// <summary>
        /// Get stock level for a specific item
        /// </summary>
        [HttpGet("stock/{itemId}")]
        public async Task<IActionResult> GetStockLevel(int itemId)
        {
            var result = await _inventoryService.GetStockLevelAsync(itemId, _loggedInUser.BranchId);
            return Ok(result);
        }

        /// <summary>
        /// Register a manual stock movement (adjustment)
        /// </summary>
        [HttpPost("movement")]
        public async Task<IActionResult> RegisterMovement([FromBody] MovementRequest request)
        {
            request.ProcessedBy = _loggedInUser.Id;
            request.BranchId = _loggedInUser.BranchId;
            var result = await _inventoryService.RegisterMovementAsync(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Perform stock take adjustment
        /// </summary>
        [HttpPost("stocktake")]
        public async Task<IActionResult> PerformStockTake([FromBody] StockTakeRequest request)
        {
            request.ProcessedBy = _loggedInUser.Id;
            var result = await _inventoryService.PerformStockTakeAsync(request);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Reserve items for a clinic service
        /// </summary>
        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveItems([FromBody] ReservationRequest request)
        {
            request.RequestedBy = _loggedInUser.Id;
            var result = await _inventoryService.ReserveItemsAsync(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Commit a reservation (deduct the reserved quantities)
        /// </summary>
        [HttpPost("reservation/{reservationId}/commit")]
        public async Task<IActionResult> CommitReservation(int reservationId)
        {
            var result = await _inventoryService.CommitReservationAsync(reservationId);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Release a reservation without deduction
        /// </summary>
        [HttpPost("reservation/{reservationId}/release")]
        public async Task<IActionResult> ReleaseReservation(int reservationId)
        {
            var result = await _inventoryService.ReleaseReservationAsync(reservationId);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get reservation details by ID
        /// </summary>
        [HttpGet("reservation/{reservationId}")]
        public async Task<IActionResult> GetReservationDetails(int reservationId)
        {
            var result = await _inventoryService.GetReservationDetailsAsync(reservationId);
            if (result.Id == 0)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// List all active reservations for user's branch
        /// </summary>
        [HttpGet("reservations/active")]
        public async Task<IActionResult> ListActiveReservations()
        {
            var result = await _inventoryService.ListActiveReservationsAsync(_loggedInUser.BranchId);
            return Ok(result);
        }

        /// <summary>
        /// Get stock movement history with optional filters
        /// </summary>
        [HttpGet("movements")]
        public async Task<IActionResult> GetMovementHistory(
            [FromQuery] int? itemId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _inventoryService.GetMovementHistoryAsync(itemId, _loggedInUser.BranchId, startDate, endDate);
            return Ok(result);
        }

        /// <summary>
        /// Get expiring stock for user's branch
        /// </summary>
        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiringStock([FromQuery] int daysUntilExpiry = 30)
        {
            var result = await _inventoryService.GetExpiringStockAsync(daysUntilExpiry, _loggedInUser.BranchId);
            return Ok(result);
        }

        /// <summary>
        /// Get stock valuation report for user's branch
        /// </summary>
        [HttpGet("valuation")]
        public async Task<IActionResult> GetValuationReport([FromQuery] DateTime? asOfDate = null)
        {
            var result = await _inventoryService.GetStockValuationReportAsync(asOfDate, _loggedInUser.BranchId);
            return Ok(result);
        }
    }

    public class KitConsumptionRequest
    {
        public int Quantity { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
    }
}