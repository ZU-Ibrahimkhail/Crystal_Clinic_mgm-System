using Crystal_Clinic_Mgm.Application.BranchStock.Suppliers;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Stock
{
    [Authorize]
    [RBAC]
    public class SupplierDueReportController : BaseController
    {


        /// <summary>
        /// Retrieves a paginated supplier dues report with optional filters.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="supplierName">Optional supplier name filter.</param>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="currencyTypeId">Optional currency type ID filter.</param>
        /// <param name="pageSize">Number of records per page (1-100).</param>
        /// <param name="lastId">Last ID for pagination.</param>
        /// <returns>The supplier dues report.</returns>
        [HttpGet("dues-report")]
        [ProducesResponseType(typeof(SupplierDuesReportResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSupplierDuesReport([FromQuery] SupplierDuesReportQuery query)
        {
            try
            {
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Retrieves a paginated due payments report with optional filters.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="supplierName">Optional supplier name filter.</param>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="currencyTypeId">Optional currency type ID filter.</param>
        /// <param name="pageSize">Number of records per page (1-100).</param>
        /// <param name="lastId">Last ID for pagination.</param>
        /// <returns>The due payments report.</returns>
        [HttpGet("due-payments-report")]
        [ProducesResponseType(typeof(List<DuePaymentReportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDuePaymentsReport(
            [FromQuery] DuePaymentsReportQuery query)
        {
            try
            {
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred." + ex.Message });
            }
        }
    }
}