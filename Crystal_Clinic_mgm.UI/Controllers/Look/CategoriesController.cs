using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.BranchStock.ItemAndCategory;
using Crystal_Clinic_Mgm.Application.Common.RBAC;

namespace Crystal_Clinic_Mgm.UI.Controllers.Look
{
    [Authorize]
    [RBAC]
    public class ItemCategoryController : BaseController
    {
        /// <summary>
        /// Create a new item category
        /// POST: api/Stock/ItemCategory/category/create
        /// </summary>
        [HttpPost("category")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateItemCategoryCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryId = await Mediator.Send(command);
            return Ok(new { CategoryId = categoryId });
        }

        /// <summary>
        /// Update an existing item category
        /// PUT: api/Stock/ItemCategory/category/update
        /// </summary>
        [HttpPut("category/update")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateItemCategoryCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedId = await Mediator.Send(command);
            if (updatedId == 0) return NotFound("Category not found or already deleted.");
            return Ok(new { UpdatedCategoryId = updatedId });
        }

        /// <summary>
        /// Soft-delete an item category
        /// DELETE: api/Stock/ItemCategory/category/delete/{categoryId}
        /// </summary>
        [HttpDelete("category/delete/{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            var success = await Mediator.Send(new DeleteItemCategoryCommand { CategoryId = categoryId });
            return success ? Ok("Category deleted.") : NotFound("Category not found or already deleted.");
        }
    }
}
