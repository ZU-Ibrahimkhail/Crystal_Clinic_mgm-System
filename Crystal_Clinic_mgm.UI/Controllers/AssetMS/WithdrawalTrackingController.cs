using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Create;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Delete;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Deposit;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Update;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.UpdateDeposit;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.AssetMS
{
    [Authorize]
    [RBAC]
    public class WithdrawalTrackingController : BaseController
    {

        /// <summary>
        /// Create WithdrawalTracking
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Json Record</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateWithdrawalTrackingCommand command)
        {
            if (ModelState.IsValid)
            {
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// Create Deposit WithdrawalTracking
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Json Record</returns>
        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit(CreateDepositCommand command)
        {
            if (ModelState.IsValid)
            {
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// Update WithdrawalTracking By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdateWithdrawalTrackingCommand command, int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Not valid Id");
            }
            if (ModelState.IsValid)
            {
                command.ID = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// Update WithdrawalTracking By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("UpdateDeposit/{Id:int}")]
        public async Task<IActionResult> UpdateDeposit(UpdateDepositCommand command, int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Not valid Id");
            }
            if (ModelState.IsValid)
            {
                command.ID = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// Delete WithdrawalTracking by ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id"> ID is require</param>
        /// <returns></returns>
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteWithdrawalTrackingCommand command, int Id)
        {
            if (Id <= 0)
                return BadRequest("Not valid Id");
            if (ModelState.IsValid)
            {
                command.ID = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        ///  Get WithdrawalTracking by filtration List/Record
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetWithdrawalTrackingListModel>> GetList(GetWithdrawalTrackingListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get WithdrawalTracking Detail by ID
        /// </summary>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetWithdrawalTrackingDetailQuery
            {
                Id = Id,
            };
            return await Mediator.Send(model);
        }
    }
}
