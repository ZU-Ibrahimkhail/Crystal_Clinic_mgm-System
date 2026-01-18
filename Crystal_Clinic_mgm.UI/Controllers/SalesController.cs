using Crystal_Clinic_Mgm.Application.BranchStock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        /// <summary>
        /// Create a sales estimate (quote)
        /// </summary>
        [HttpPost("estimate")]
        public async Task<IActionResult> CreateEstimate([FromBody] CreateEstimateRequest request)
        {
            var result = await _salesService.CreateEstimateAsync(request);
            if (result.Succeeded)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Convert an estimate to a sales invoice
        /// </summary>
        [HttpPost("estimate/{estimateId}/convert")]
        public async Task<IActionResult> ConvertEstimateToInvoice(int estimateId)
        {
            var result = await _salesService.ConvertEstimateToInvoiceAsync(estimateId);
            if (result.Succeeded)
                return Ok(result.Data);
            return BadRequest(result.Error);
        }

        /// <summary>
        /// Get available tax rates
        /// </summary>
        [HttpGet("tax-rates")]
        public async Task<IActionResult> GetTaxRates()
        {
            var taxRates = await _salesService.GetTaxRatesAsync();
            return Ok(taxRates);
        }

        /// <summary>
        /// Calculate tax for an amount
        /// </summary>
        [HttpPost("calculate-tax")]
        public async Task<IActionResult> CalculateTax([FromBody] TaxCalculationRequest request)
        {
            var taxAmount = await _salesService.CalculateTaxAsync(request.Amount, request.TaxId);
            return Ok(new { TaxAmount = taxAmount, TotalAmount = request.Amount + taxAmount });
        }
    }

    public class TaxCalculationRequest
    {
        public decimal Amount { get; set; }
        public int TaxId { get; set; }
    }
}