using Crystal_Clinic_Mgm.Application.BranchStock;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class StockMovementsController(IInventoryService inventoryService, ILoggedInUser loggedInUser) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> RegisterMovement([FromBody] MovementRequest request)
        {
            request.ProcessedBy = loggedInUser.Id;
            request.BranchId = loggedInUser.BranchId;
            var result = await inventoryService.RegisterMovementAsync(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovementHistory(
            [FromQuery] int? itemId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await inventoryService.GetMovementHistoryAsync(itemId, loggedInUser.BranchId, startDate, endDate);
            return Ok(result);
        }
    }
}
