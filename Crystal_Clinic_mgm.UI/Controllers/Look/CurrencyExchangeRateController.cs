using Crystal_Clinic_Mgm.Application.Look.CurrencyExchangeRates;
using Crystal_Clinic_Mgm.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Crystal_Clinic_Mgm.UI.Controllers.Look
{
    public class CurrencyExchangeRateController : BaseController
    {


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyExchangeRateCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { currencyExchangeRateId = result.CurrencyExchangeRateId }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{currencyExchangeRateId}")]
        public async Task<IActionResult> GetById(int currencyExchangeRateId)
        {
            try
            {
                var result = await Mediator.Send(new GetCurrencyExchangeRateByIdQuery { CurrencyExchangeRateId = currencyExchangeRateId });
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await Mediator.Send(new GetAllCurrencyExchangeRatesQuery());
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCurrencyExchangeRateCommand command)
        {
            try
            {
                var success = await Mediator.Send(command);
                return success ? Ok("Exchange rate updated successfully.") : BadRequest("Failed to update exchange rate.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{currencyExchangeRateId}")]
        public async Task<IActionResult> Delete(int currencyExchangeRateId)
        {
            try
            {
                var success = await Mediator.Send(new DeleteCurrencyExchangeRateCommand { CurrencyExchangeRateId = currencyExchangeRateId });
                return success ? Ok("Exchange rate deleted successfully.") : BadRequest("Failed to delete exchange rate.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}