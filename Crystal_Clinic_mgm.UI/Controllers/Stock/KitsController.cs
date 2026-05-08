using Crystal_Clinic_Mgm.Application.BranchStock;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class KitsController(IKitService ikitService, IInventoryService inventoryService, ILoggedInUser loggedInUser) :BaseController
    {
        /// <summary>
        /// Create Inventory Kit
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateKitRequest request)
        {
            var result = await ikitService.CreateKitAsync(request);
            if (result.Succeeded) return Ok(result);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get available inventory kits
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAvailableKits([FromQuery] int pagesize, [FromQuery] int pagenumber)
        {
            var kits = await ikitService.GetAvailableKitsAsync(loggedInUser.BranchId, pagesize, pagenumber);
            return Ok(kits);
        }
        /// <summary>
        /// Detail of Inventory Kit
        /// </summary>
        [HttpGet("{KitId}")]
        public async Task<IActionResult> GetKitById([FromRoute] int KitId)
        {
            var kits = await ikitService.GetKitByIdAsync(KitId);
            return Ok(kits);
        }

        /// <summary>
        /// Consume an inventory kit
        /// </summary>
        [HttpPost("{kitId}/consume")]
        public async Task<IActionResult> ConsumeKit(int kitId, [FromBody] KitConsumptionRequest request)
        {
            var result = await ikitService.ConsumeKitAsync(kitId, request.Quantity, request.ReferenceId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("/visits/{visitId}/add-kit")]
        public async Task<IActionResult> AddKitToVisit(int visitId, [FromBody] AddKitToVisitRequest request)
        {
            var result = await inventoryService.AddKitToVisitAsync(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Update Inventory Kit
        /// </summary>
        [HttpPut("{KitId}")]
        public async Task<IActionResult> Update(int KitId, [FromBody] UpdateKitRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.Id != KitId)
                return BadRequest("ID mismatch");

            var result = await ikitService.UpdateKitAsync(request);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Delete Inventory Kit by Id
        /// </summary>
        [HttpDelete("{KitId}")]
        public async Task<IActionResult> Delete(int KitId)
        {
            var result = await ikitService.DeleteKitAsync(KitId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

    }

    public class KitConsumptionRequest
    {
        public int Quantity { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
    }
}
