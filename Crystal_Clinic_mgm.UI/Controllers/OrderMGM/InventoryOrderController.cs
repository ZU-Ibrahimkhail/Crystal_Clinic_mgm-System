using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.OrderMGM
{
    [Authorize]
    public class InventoryOrderController : BaseController
    {
        [HttpGet("orders/outstanding")]
        public async Task<IActionResult> GetOutstandingOrders()
        {
            var result = await Mediator.Send(new GetOutstandingOrdersQuery());
            return Ok(result);
        }

        [HttpGet("items/check-stock")]
        public async Task<IActionResult> CheckStockByDate([FromQuery] int itemId, [FromQuery] DateTime targetDate)
        {
            var result = await Mediator.Send(new CheckItemStockByDateQuery { ItemId = itemId, TargetDate = targetDate });
            return Ok(result);
        }

        [HttpGet("items/rental-availability")]
        public async Task<IActionResult> ListRentalAvailability(
       [FromQuery] DateTime targetDate,
       [FromQuery] int branchId,
       [FromQuery] int? categoryId,
       [FromQuery] int? lastItemId,
       [FromQuery] int pageSize = 20)
        {
            var result = await Mediator.Send(new ListAvailableRentalItemsQuery
            {
                TargetDate = targetDate,
                BranchId = branchId,
                CategoryId = categoryId,
                LastItemId = lastItemId,
                PageSize = pageSize
            });

            return Ok(result);
        }

    }
}
