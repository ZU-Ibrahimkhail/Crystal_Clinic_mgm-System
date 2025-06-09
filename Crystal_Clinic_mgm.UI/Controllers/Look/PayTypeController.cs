using Crystal_Clinic_Mgm.Application.Look.PayTypes.Commands.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.PayTypes.Commands.Create;
using Crystal_Clinic_Mgm.Application.Look.PayTypes.Commands.Delete;
using Crystal_Clinic_Mgm.Application.Look.PayTypes.Commands.Update;
using Crystal_Clinic_Mgm.Application.Look.PayTypes.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.Look.PayTypes.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Look.PayTypes.Queries.GetList;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.Look
{
    [Authorize]
    [RBAC]
    public class PayTypeController : BaseController
    {

        /// <summary>
        /// Create PayType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Json Record</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreatePayTypeCommand command)
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
        /// Update PayType By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdatePayTypeCommand command, int Id)
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
         /// Delete PayType by ID
         /// </summary>
         /// <param name="command"></param>
         /// <param name="Id"> ID is require</param>
         /// <returns></returns>
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeletePayTypeCommand command, int Id)
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
        ///  Get PayType by filtration List/Record
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetPayTypeDetailModel>> GetList(GetPayTypeListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get PayType Detail by ID
        /// </summary>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetPayTypeDetailQuery
            {
                Id = Id,
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Get PayType Dropdown List
        /// </summary>
        /// <returns></returns> 
        [DisableRBAC]
        [HttpGet("GetPayTypeDDL")]
        public async Task<IActionResult> GetPayTypeDDL()
        {
            var branch = new GetPayTypeDDLQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            var result = await Mediator.Send(branch);
            return Ok(result);
        }
    }
}
