using Crystal_Clinic_Mgm.Application.Look.AssetTypes.Commands.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.AssetTypes.Commands.Create;
using Crystal_Clinic_Mgm.Application.Look.AssetTypes.Commands.Delete;
using Crystal_Clinic_Mgm.Application.Look.AssetTypes.Commands.Update;
using Crystal_Clinic_Mgm.Application.Look.AssetTypes.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.Look.AssetTypes.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Look.AssetTypes.Queries.GetList;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.Look
{
    [Authorize]
    [RBAC]
    public class AssetTypeController : BaseController
    {

        /// <summary>
        /// Create AssetType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Json Record</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateAssetTypeCommand command)
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
        /// Update AssetType By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdateAssetTypeCommand command, int Id)
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
         /// Delete AssetType by ID
         /// </summary>
         /// <param name="command"></param>
         /// <param name="Id"> ID is require</param>
         /// <returns></returns>
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteAssetTypeCommand command, int Id)
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
        ///  Get AssetType by filtration List/Record
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetAssetTypeDetailModel>> GetList(GetAssetTypeListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get AssetType Detail by ID
        /// </summary>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetAssetTypeDetailQuery
            {
                Id = Id,
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Get AssetType Dropdown List
        /// </summary>
        /// <returns></returns> 
        [DisableRBAC]
        [HttpGet("GetAssetTypeDDL")]
        public async Task<IActionResult> GetAssetTypeDDL()
        {
            var branch = new GetAssetTypeDDLQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            var result = await Mediator.Send(branch);
            return Ok(result);
        }
    }
}
