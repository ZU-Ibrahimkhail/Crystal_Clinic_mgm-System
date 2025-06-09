using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Commands.Create;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Commands.Delete;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Commands.Update;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetList;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.Look
{
    [Authorize]
    [RBAC]
    public class CurrencyTypeController : BaseController
    {

        ///// <summary>
        ///// Create CurrencyType
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns>Json Record</returns>
        //[HttpPost]
        //public async Task<IActionResult> Create(CreateCurrencyTypeCommand command)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return await Mediator.Send(command);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
        ///// <summary>
        ///// Update CurrencyType By ID
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="Id">ID is require</param>
        ///// <returns></returns>
        //[HttpPut("{Id:int}")]
        //public async Task<IActionResult> Update(UpdateCurrencyTypeCommand command, int Id)
        //{
        //    if (Id <= 0)
        //    {
        //        return BadRequest("Not valid Id");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        command.ID = Id;
        //        return await Mediator.Send(command);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}/// <summary>
        // /// Delete CurrencyType by ID
        // /// </summary>
        // /// <param name="command"></param>
        // /// <param name="Id"> ID is require</param>
        // /// <returns></returns>
        //[HttpDelete("{Id:int}")]
        //public async Task<IActionResult> Delete(DeleteCurrencyTypeCommand command, int Id)
        //{
        //    if (Id <= 0)
        //        return BadRequest("Not valid Id");
        //    if (ModelState.IsValid)
        //    {
        //        command.ID = Id;
        //        return await Mediator.Send(command);
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
        /// <summary>
        ///  Get CurrencyType by filtration List/Record
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetCurrencyTypeDetailModel>> GetList(GetCurrencyTypeListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get CurrencyType Detail by ID
        /// </summary>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetCurrencyTypeDetailQuery
            {
                Id = Id,
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Get CurrencyType Dropdown List
        /// </summary>
        /// <returns></returns> 
        [DisableRBAC]
        [HttpGet("GetCurrencyTypeDDL")]
        public async Task<IActionResult> GetCurrencyTypeDDL()
        {
            var branch = new GetCurrencyTypeDDLQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            var result = await Mediator.Send(branch);
            return Ok(result);
        }
    }
}
