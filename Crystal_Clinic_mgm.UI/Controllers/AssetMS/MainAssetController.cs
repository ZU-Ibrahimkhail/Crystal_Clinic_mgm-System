using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.AssetMS.AccountTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Create;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Delete;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.DepositToUser;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Update;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetChildDDl;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetList;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.AssetMS
{
    [Authorize]
    [RBAC]
    public class MainAccountController : BaseController
    {

        /// <summary>
        /// Create MainAccount
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Json Record</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateMainAccountCommand command)
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
        /// Update MainAccount By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update(UpdateMainAccountCommand command, Guid Id)
        {
            if (ModelState.IsValid)
            {
                command.ID = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }/// <summary>
         /// Delete MainAccount by ID
         /// </summary>
         /// <param name="command"></param>
         /// <param name="Id"> ID is require</param>
         /// <returns></returns>
        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete(DeleteMainAccountCommand command, Guid Id)
        {
            command.ID = Id;
            return await Mediator.Send(command);
        }
        /// <summary>
        ///  Get MainAccount by filtration List/Record
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetMainAccountDetailModel>> GetList(GetMainAccountListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get MainAccount Detail by ID
        /// </summary>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDetail/{Id:Guid}")]
        public async Task<IActionResult> GetDetail(Guid Id)
        {
            var model = new GetMainAccountDetailQuery
            {
                Id = Id,
            };
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Deposit Main Asset To a User
        /// </summary>
        /// <returns></returns> 
        [HttpPost("DepositToUser")]
        public async Task<IActionResult> DepositToUser(DepositMainAccountToUserCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Get the list of Asset Tracking By User Id, description, debit,credit ,balance 
        /// </summary>
        /// <returns></returns> 
        [HttpPost("GetAccountTrackingList/{MainAccountId:Guid}")]
        public async Task<ResponseDataTable<GetAccountTrackingListModel>> GetAccountTrackingList(GetAccountTrackingListQuery query, Guid MainAccountId)
        {
            query.MainAccountId = MainAccountId;
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Get MainAccount Detail by ID
        /// </summary>
        /// <param name="UserId">a query string user id to filter main assets </param>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDropDownList")]
        public async Task<IActionResult> GetDropDownList(Guid? UserId)
        {
            var model = new GetMainAccountDDLQuery
            {
                UserId = UserId
            };
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Get Child Assets Dropdown List
        /// </summary>
        /// <param name="MainAccountId">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetChildAssetsDDL/{MainAccountId:Guid}")]
        public async Task<IActionResult> GetChildAssetsDDL(Guid MainAccountId)
        {
            var model = new GetMainAccountChildDDLQuery
            {
                MainAccountId = MainAccountId
            };
            return await Mediator.Send(model);
        }
    }
}
