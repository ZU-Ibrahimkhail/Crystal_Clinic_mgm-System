using Crystal_Clinic_Mgm.Application.BranchStock.Suppliers;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class SupplierDuesController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CreateSupplierDue([FromBody] CreateSupplierDueCommand command)
        {
            var supplierDueId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetSupplierDueById), new { id = supplierDueId }, new { supplierDueId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierDueById(int id)
        {
            var supplierDue = await Mediator.Send(new GetSupplierDueByIdQuery { Id = id });
            if (supplierDue == null)
                return NotFound();
            return Ok(supplierDue);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSupplierDues([FromQuery] GetAllSupplierDuesQuery query)
        {
            var supplierDues = await Mediator.Send(query);
            return Ok(supplierDues);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplierDue(int id, [FromBody] UpdateSupplierDueCommand command)
        {
            if (id != command.Id)
                return BadRequest("SupplierDue ID mismatch");

            var result = await Mediator.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplierDue(int id)
        {
            var result = await Mediator.Send(new DeleteSupplierDueCommand { Id = id });
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}