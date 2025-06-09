// PaymentsController for managing OrderPayments
using Crystal_Clinic_Mgm.Application.OrderMgm.OrderPayments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.OrderMGM
{
    [Authorize]
    public class PaymentsController : BaseController
    {


        /// <summary>
        /// Create a new payment for an order.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderPaymentCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { PaymentId = id });
        }

        /// <summary>
        /// Delete an existing payment.
        /// </summary>
        [HttpDelete("{paymentId:int}")]
        public async Task<IActionResult> Delete(int paymentId)
        {
            var success = await Mediator.Send(new DeleteOrderPaymentCommand { PaymentId = paymentId });
            return success ? Ok("Payment deleted successfully.") : NotFound("Payment not found.");
        }
        /// <summary>
        /// Get all payments for a specific order.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            var payments = await Mediator.Send(new GetOrderPaymentsByOrderIdQuery { OrderId = orderId });
            return Ok(payments);
        }

        /// <summary>
        /// List all payments (admin view with cursor pagination).
        /// </summary>
        [HttpGet("payments")]
        public async Task<IActionResult> ListAll([FromQuery] ListAllOrderPaymentsQuery query)
        {
            var payments = await Mediator.Send(query);
            return Ok(payments);
        }
    }
}
