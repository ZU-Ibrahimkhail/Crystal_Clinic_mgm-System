using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Create;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Delete;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Update;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.AssetMS
{
    [Authorize]
    [RBAC]
    public class ExpenseTrackingController : BaseController
    {

        /// <summary>
        /// Create ExpenseTracking
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Json Record</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseTrackingCommand command)
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
        /// Update ExpenseTracking By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdateExpenseTrackingCommand command, int Id)
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
        }/// <summary>
         /// Delete ExpenseTracking by ID
         /// </summary>
         /// <param name="command"></param>
         /// <param name="Id"> ID is require</param>
         /// <returns></returns>
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteExpenseTrackingCommand command, int Id)
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
        ///  Get ExpenseTracking by filtration List/Record
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetExpenseTrackingListModel>> GetList(GetExpenseTrackingListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get ExpenseTracking Detail by ID
        /// </summary>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetExpenseTrackingDetailQuery
            {
                Id = Id,
            };
            return await Mediator.Send(model);
        }
    }
}
