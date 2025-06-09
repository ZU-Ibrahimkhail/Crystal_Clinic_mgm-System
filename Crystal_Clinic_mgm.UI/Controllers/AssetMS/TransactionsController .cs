using Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.AssetMS
{
    [Authorize]
    public class TransactionsController : BaseController
    {

        // Initiate a transaction
        [HttpPost("initiate")]
        public async Task<IActionResult> InitiateTransfer([FromBody] InitiateTransferCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }

        // Update a transaction
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTransaction([FromBody] UpdateTransactionCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }

        // Approve a transaction
        [HttpPost("approve")]
        public async Task<IActionResult> ApproveTransfer([FromBody] ApproveTransferCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }

        // Confirm a transaction
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmTransfer([FromBody] ConfirmTransferCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }

        // Get transaction by ID (for demonstration purposes)
        [HttpPost("GetPendingTransaction")]
        public async Task<IActionResult> GetPendingTransaction(GetPendingTransactionQuery query)
        {
            var transaction = await Mediator.Send(query);
            if (transaction == null)
                return NotFound("Transaction not found.");

            return Ok(transaction);
        }
        // Get transaction by ID (for demonstration purposes)
        [HttpPost("GetApprovedTransaction")]
        public async Task<IActionResult> GetApprovedTransaction(GetApprovedTransactionsQuery query)
        {
            var transaction = await Mediator.Send(query);
            if (transaction == null)
                return NotFound("Transaction not found.");

            return Ok(transaction);
        }
    }
}