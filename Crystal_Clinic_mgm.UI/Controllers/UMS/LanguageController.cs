using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.Languages.Queries.GetLanguageList;
using Crystal_Clinic_Mgm.Application.UMS.Languages.Queries.GetLanguagesDDL;

namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    [RBAC]
    [Authorize]
    public class LanguageController : BaseController
    {
        // -/// <summary>
        // -/// Create a Language
        // -/// </summary>
        // -/// <param name="command"></param>
        // -/// <returns></returns>
        //[HttpPost]
        //[DisableRBAC]
        //public async Task<IActionResult> Create(CreateLanguageCommand command)
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

        //-/// <summary>
        //-/// Update an Exist Language
        //-/// </summary>
        //-/// <param name="command"></param>
        //-/// <param name="Id"></param>
        //-/// <returns></returns>
        //[HttpPut("{Id:int}")]
        //[DisableRBAC]
        //public async Task<IActionResult> Update(UpdateLanguageCommand command, int Id)
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
        //}

        ///// <summary>
        ///// Delete a Language
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[HttpDelete("{Id:int}")]
        //[DisableRBAC]
        //public async Task<IActionResult> Delete(DeleteLanguageCommand command, int Id)
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
        /// Get Langage List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("Get-Language-List")]
        [DisableRBAC]
        public async Task<ResponseDataTable<GetDropDownGeneralModel>> GetList(GetLanguageListQuery query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Get Language Dropdown
        /// </summary>
        /// <returns></returns>
        /// 

        [DisableRBAC]
        [HttpGet("Get-Language-DDL")]
        public async Task<List<GetDropDownGeneralModel>> GetLanguageDDL()
        {
            var Lang = new GetLanguageDDLQuery();
            var result = await Mediator.Send(Lang);
            return result;
        }
    }
}
