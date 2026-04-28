using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.BranchStock.ItemAndCategory;
using Crystal_Clinic_Mgm.Application.Common.RBAC;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class ItemCategoryController : BaseController
    {
        // ----- Item Category Endpoints -----

        // GET: api/Stock/ItemCategory/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await Mediator.Send(new GetItemCategoriesQuery());
            return Ok(categories);
        }

        // POST: api/Stock/ItemCategory/category/create
        [HttpPost("category/create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateItemCategoryCommand command)
        {
            var categoryId = await Mediator.Send(command);
            return Ok(new { CategoryId = categoryId });
        }

        // PUT: api/Stock/ItemCategory/category/update
        [HttpPut("category/update")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateItemCategoryCommand command)
        {
            var updatedId = await Mediator.Send(command);
            if (updatedId == 0) return NotFound("Category not found or deleted.");
            return Ok(new { UpdatedCategoryId = updatedId });
        }

        // DELETE: api/Stock/ItemCategory/category/delete/{categoryId}
        [HttpDelete("category/delete/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var success = await Mediator.Send(new DeleteItemCategoryCommand { CategoryId = categoryId });
            return success ? Ok("Category deleted.") : NotFound("Category not found or already deleted.");
        }

        // ----- Item Endpoints -----

        // GET: api/Stock/ItemCategory/items
        // Accepts query params mapped to GetItemsQuery (SearchText, BranchId, PageSize, LastItemId, CategoryId)
        [HttpGet("items")]
        public async Task<IActionResult> GetItems([FromQuery] GetItemsQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        // POST: api/Stock/ItemCategory/item/create
        [HttpPost("item/create")]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command)
        {
            var itemId = await Mediator.Send(command);
            return Ok(new { ItemId = itemId });
        }

        // PUT: api/Stock/ItemCategory/item/update
        [HttpPut("item/update")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemCommand command)
        {
            var itemId = await Mediator.Send(command);
            if (itemId == 0) return NotFound("Item not found or deleted.");
            return Ok(new { ItemId = itemId });
        }

        // POST: api/Stock/ItemCategory/item/add-image
        // Use multipart/form-data with fields ItemId and FormFile
        [HttpPost("item/add-image")]
        public async Task<IActionResult> AddImageToItem([FromForm] AddImageToItemCommand command)
        {
            var itemId = await Mediator.Send(command);
            if (itemId == 0) return NotFound("Item not found or deleted.");
            return Ok(new { ItemId = itemId });
        }

        // DELETE: api/Stock/ItemCategory/item/delete/{itemId}
        [HttpDelete("item/delete/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var success = await Mediator.Send(new DeleteItemCommand { ItemId = itemId });
            return success ? Ok("Item soft-deleted.") : NotFound("Item not found.");
        }
    }
}
