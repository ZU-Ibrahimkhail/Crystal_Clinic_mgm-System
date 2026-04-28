using Crystal_Clinic_Mgm.Application.BranchStock;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class InventoryController(IInventoryService inventoryService, ILoggedInUser loggedInUser) : BaseController
    {
        [HttpGet("stock/{itemId}")]
        public async Task<IActionResult> GetStockLevel(int itemId)
        {
            var result = await inventoryService.GetStockLevelAsync(itemId, loggedInUser.BranchId);
            return Ok(result);
        }

        [HttpPost("stocktake")]
        public async Task<IActionResult> PerformStockTake([FromBody] StockTakeRequest request)
        {
            request.ProcessedBy = loggedInUser.Id;
            var result = await inventoryService.PerformStockTakeAsync(request);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Error);
        }

        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiringStock([FromQuery] int daysUntilExpiry = 30)
        {
            var result = await inventoryService.GetExpiringStockAsync(daysUntilExpiry, loggedInUser.BranchId);
            return Ok(result);
        }

        [HttpGet("valuation")]
        public async Task<IActionResult> GetValuationReport([FromQuery] DateTime? asOfDate = null)
        {
            var result = await inventoryService.GetStockValuationReportAsync(asOfDate, loggedInUser.BranchId);
            return Ok(result);
        }
    }
}
