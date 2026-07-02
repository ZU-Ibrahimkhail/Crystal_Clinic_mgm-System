using Crystal_Clinic_Mgm.Application.BranchStock.Suppliers;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class SupplierController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierCommand command)
        {
            var supplierId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetSupplierById), new { id = supplierId }, new { supplierId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await Mediator.Send(new GetSupplierByIdQuery { Id = id });
            if (supplier == null)
                return NotFound();
            return Ok(supplier);
        }

        [HttpGet]
        [DisableRBAC]
        public async Task<IActionResult> GetAllSuppliers([FromQuery] GetAllSuppliersQuery query)
        {
            var suppliers = await Mediator.Send(query);
            return Ok(suppliers);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierCommand command)
        {
            if (id != command.Id)
                return BadRequest("Supplier ID mismatch");

            var result = await Mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var result = await Mediator.Send(new DeleteSupplierCommand { Id = id });
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}