using Crystal_Clinic_Mgm.Application.BranchStock.Stock;
using Crystal_Clinic_Mgm.Application.StockManagement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    public class StockController : BaseController
    {

        // Get all stock items with pagination and search
        [HttpGet("list")]
        public async Task<IActionResult> GetStockList([FromQuery] GetStockQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        // Create a new stock item
        [HttpPost("create")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockCommand command)
        {
            var stockId = await Mediator.Send(command);
            return Ok(new { StockId = stockId });
        }

        // Update an existing stock item
        [HttpPut("update")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockCommand command)
        {
            var stockId = await Mediator.Send(command);
            return Ok(new { UpdatedStockId = stockId });
        }

        // Delete a stock item
        [HttpDelete("delete/{stockId}")]
        public async Task<IActionResult> DeleteStock(int stockId)
        {
            var success = await Mediator.Send(new DeleteStockCommand { StockId = stockId });
            return success ? Ok("Stock item deleted.") : NotFound("Stock item not found.");
        }

        [HttpPatch("AdjustStockQuantity")]
        public async Task<IActionResult> AdjustStockQuantity([FromBody] AdjustStockQuantityCommand command)
        {
            var stockId = await Mediator.Send(command);
            return Ok(new { StockId = stockId });
        }
        [HttpPatch("UpdateStockPrices")]
        public async Task<IActionResult> UpdateStockPrices([FromBody] UpdateStockPricesCommand command)
        {
            var stockId = await Mediator.Send(command);
            return Ok(new { StockId = stockId });
        }
    }
}
