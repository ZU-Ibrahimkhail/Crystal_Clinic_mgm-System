using Crystal_Clinic_Mgm.Application.Customers.Commands;
using Crystal_Clinic_Mgm.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Api.Controllers
{
    [Authorize]
    public class CustomersController : BaseController
    {


        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            var customerId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, null);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await Mediator.Send(new DeleteCustomerCommand { CustomerId = id });
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await Mediator.Send(new GetCustomerByIdQuery { CustomerId = id });
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetCustomers([FromQuery] GetCustomersQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }
    }
}