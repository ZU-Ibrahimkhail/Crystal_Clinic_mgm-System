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

        // GET: api/Stock/ItemCategory/items
        // Accepts query params mapped to GetItemsQuery (SearchText, BranchId, PageSize, LastItemId, CategoryId)
        [HttpGet("items")]
        [DisableRBAC]
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
