using Crystal_Clinic_Mgm.Application.BranchStock;
using Crystal_Clinic_Mgm.Application.BranchStock.ItemAndCategory;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.ModelBinding;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class InventoryController(IInventoryService inventoryService, ILoggedInUser loggedInUser) : BaseController
    {
        [HttpPost("Categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateItemCategoryCommand command)
        {
            var categoryId = await Mediator.Send(command);
            return Ok(new { CategoryId = categoryId });
        }

        [HttpPut("Categories")]
        public async Task<IActionResult> UpdateCategory([FromQuery] int categoryId, [FromBody] UpdateItemCategoryCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            command.ItemCategoryId = categoryId;

            var updatedId = await Mediator.Send(command);
            if (updatedId == 0) return NotFound("Category not found or deleted.");
            return Ok(new { UpdatedCategoryId = updatedId });
        }


        [HttpDelete("Categories/{Id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var success = await Mediator.Send(new DeleteItemCategoryCommand { CategoryId = Id });
            return success ? Ok("Category deleted.") : NotFound("Category not found or already deleted.");
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await Mediator.Send(new GetItemCategoriesQuery());
            return Ok(categories);
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategoriesList()
        {
            var categories = await Mediator.Send(new GetItemCategoriesQuery());
            return Ok(categories);
        }

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

        // POST: api/Stock/ItemCategory/item/create
        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command)
        {
            var itemId = await Mediator.Send(command);
            return Ok(new { ItemId = itemId });
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems([FromQuery] GetItemsQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }


        // PUT: api/Stock/ItemCategory/item/update
        [HttpPut("{ItemId}")]
        public async Task<IActionResult> UpdateItem(int ItemId,[FromBody] UpdateItemCommand command)
        {
            if (ItemId != command.ItemId)
                return BadRequest("Item ID mismatch");
            var itemId = await Mediator.Send(command);
            if (itemId == 0) return NotFound("Item not found or deleted.");
            return Ok(new { ItemId = itemId });
        }

        // POST: api/Stock/ItemCategory/item/add-image
        // Use multipart/form-data with fields ItemId and FormFile
        [HttpPost("items/add-image")]
        public async Task<IActionResult> AddImageToItem([FromForm] AddImageToItemCommand command)
        {
            var itemId = await Mediator.Send(command);
            if (itemId == 0) return NotFound("Item not found or deleted.");
            return Ok(new { ItemId = itemId });
        }

        // DELETE: api/Stock/ItemCategory/item/delete/{itemId}
        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var success = await Mediator.Send(new DeleteItemCommand { ItemId = itemId });
            return success ? Ok("Item soft-deleted.") : NotFound("Item not found.");
        }
    }
}
