using Crystal_Clinic_Mgm.Application.BranchStock;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class ReservationsController(IInventoryService inventoryService, ILoggedInUser loggedInUser) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> ReserveItems([FromBody] ReservationRequest request)
        {
            request.RequestedBy = loggedInUser.Id;
            var result = await inventoryService.ReserveItemsAsync(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("{reservationId}/commit")]
        public async Task<IActionResult> CommitReservation(int reservationId)
        {
            var result = await inventoryService.CommitReservationAsync(reservationId);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Error);
        }

        [HttpPost("{reservationId}/release")]
        public async Task<IActionResult> ReleaseReservation(int reservationId)
        {
            var result = await inventoryService.ReleaseReservationAsync(reservationId);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Error);
        }

        [HttpGet("{reservationId}")]
        public async Task<IActionResult> GetReservationDetails(int reservationId)
        {
            var result = await inventoryService.GetReservationDetailsAsync(reservationId);
            if (result.Id == 0)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> ListActiveReservations()
        {
            var result = await inventoryService.ListActiveReservationsAsync(loggedInUser.BranchId);
            return Ok(result);
        }
    }
}
