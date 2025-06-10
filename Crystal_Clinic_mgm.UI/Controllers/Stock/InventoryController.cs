// InventoryController.cs
using Crystal_Clinic_Mgm.Application.BranchStock.ItemAndCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.OrderMGM
{
    [Authorize]
    public class InventoryController : BaseController
    {


        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await Mediator.Send(new GetItemCategoriesQuery());
            return Ok(result);
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems([FromQuery] GetItemsQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateItemCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { CategoryId = id });
        }

        [HttpPut("categories")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateItemCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { UpdatedCategoryId = id });
        }

        [HttpDelete("categories/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var success = await Mediator.Send(new DeleteItemCategoryCommand { CategoryId = categoryId });
            return success ? Ok("Category deleted.") : NotFound("Category not found.");
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { ItemId = id });
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { UpdatedItemId = id });
        }

        [HttpPut("items/add-or-update-image")]
        public async Task<IActionResult> UpdateItemImage([FromForm] AddImageToItemCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { UpdatedItemId = id });
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var success = await Mediator.Send(new DeleteItemCommand { ItemId = itemId });
            return success ? Ok("Item deleted.") : NotFound("Item not found.");
        }

    }
}
