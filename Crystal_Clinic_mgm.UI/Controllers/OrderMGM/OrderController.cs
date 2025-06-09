// OrderController.cs
using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands;
using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands.Helpers;
using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.OrderMGM
{
    [Authorize]
    public class OrderController : BaseController
    {
        

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="command">Order creation data</param>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        /// <summary>
        /// Update an existing order by ID.
        /// </summary>
        /// <param name="command">Order update data</param>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        /// <summary>
        /// Delete an order by ID if not completed or in progress.
        /// </summary>
        /// <param name="command">Delete command with OrderId</param>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteOrderCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        /// <summary>
        /// Start delivery process for a rental order.
        /// </summary>
        /// <param name="command">Start delivery command</param>
        [HttpPost("start-delivery")]
        public async Task<IActionResult> StartDelivery([FromBody] StartDeliveryCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        /// <summary>
        /// Process return for a rental order.
        /// </summary>
        /// <param name="command">Return processing data</param>
        [HttpPost("process-return")]
        public async Task<JsonResult> ProcessReturn([FromBody] ProcessReturnCommand command)
        {
            return await Mediator.Send(command);
        }
        /// <summary>
        ///  The details of an Order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("details/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var result = await Mediator.Send(new GetOrderDetailsQuery { OrderId = orderId });
            return Ok(result);
        }
        /// <summary>
        ///  The details of an Order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("orderRecipt/{orderId}")]
        public async Task<IActionResult> GetOrderRecipt(int orderId)
        {
            var result = await Mediator.Send(new GetOrderReciptQuery { OrderId = orderId });
            return Ok(result);
        }

        /// <summary>
        /// List of orders
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> ListOrders([FromQuery] ListOrdersQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
