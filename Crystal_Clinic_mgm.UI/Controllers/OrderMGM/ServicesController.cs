// ServicesController for CRUD operations
using Crystal_Clinic_Mgm.Application.Crystal_ClinicServices;
using Crystal_Clinic_Mgm.Application.Inventory.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.OrderMGM
{

    [Authorize]
    public class ServicesController : BaseController
    {

        /// <summary>
        /// Create a new service.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { ServiceId = id });
        }

        /// <summary>
        /// Update an existing service.
        /// </summary>
        [HttpPut("{serviceId}")]
        public async Task<IActionResult> Update(int serviceId, [FromBody] UpdateServiceCommand command)
        {
            command.ServiceId = serviceId;
            var success = await Mediator.Send(command);
            return success ? Ok("Service updated successfully.") : NotFound("Service not found.");
        }

        [HttpPut("image/add-or-update-image")]
        public async Task<IActionResult> UpdateService([FromForm] AddImageToServiceCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(new { UpdatedItemId = id });
        }


        /// <summary>
        /// Delete a service.
        /// </summary>
        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> Delete(int serviceId)
        {
            var success = await Mediator.Send(new DeleteServiceCommand { ServiceId = serviceId });
            return success ? Ok("Service deleted successfully.") : NotFound("Service not found.");
        }

        /// <summary>
        /// Get a service by ID.
        /// </summary>
        [HttpGet("{serviceId}")]
        public async Task<IActionResult> GetById(int serviceId)
        {
            var service = await Mediator.Send(new GetServiceByIdQuery { ServiceId = serviceId });
            return service != null ? Ok(service) : NotFound("Service not found.");
        }

        /// <summary>
        /// List all services with optional search and pagination.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ListAll([FromQuery] ListAllServicesQuery query)
        {
            var services = await Mediator.Send(query);
            return Ok(services);
        }
    }
}
