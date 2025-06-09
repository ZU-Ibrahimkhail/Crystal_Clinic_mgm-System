using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.General.News.Commands.Create;
using Crystal_Clinic_Mgm.Application.General.News.Commands.Delete;
using Crystal_Clinic_Mgm.Application.General.News.Commands.Update;
using Crystal_Clinic_Mgm.Application.General.News.Queries.GetList;
using Crystal_Clinic_Mgm.Application.General.News.Queries.NewsDashboard;
using Crystal_Clinic_Mgm.Common.Constants;

namespace Crystal_Clinic_Mgm.UI.Controllers.General
{
    [Authorize]
    [RBAC]
    public class NewsController : BaseController
    {


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateNewsCommand command)
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
        /// Update News By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update([FromForm] UpdateNewsCommand command, int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Not valid Id");
            }
            if (ModelState.IsValid)
            {
                command.Id = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }/// <summary>
         /// Delete News by ID
         /// </summary>
         /// <param name="command"></param>
         /// <param name="Id"> ID is require</param>
         /// <returns></returns>
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteNewsCommand command, int Id)
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
        /// Search by Title,Description and Speaker
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>



        [HttpPost("GetList")]
        [DisableRBAC]
        public async Task<ResponseDataTable<GetNewsListModel>> GetList(GetNewsListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }



        [HttpGet("GetNewsDashboard")]
        [DisableRBAC]
        public async Task<JsonResult> GetNewsDashboard()
        {
            NewsDashboardQuery Query = new();
            return await Mediator.Send(Query);
        }

    }
}
