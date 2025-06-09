using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Create;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Delete;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Update;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetChildDepartmentDDL;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDDL;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentList;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.Look
{
    [Authorize]
    [RBAC]
    //[Tags("DMTS")]
    public class BranchController : BaseController
    {
        private readonly ILoggedInUser _loggedInUser;
        private readonly IMailRepositoy _emailRepo;
        public BranchController(ILoggedInUser loggedInUser, IMailRepositoy emailRepo)
        {
            _loggedInUser = loggedInUser;
            _emailRepo = emailRepo;
        }

        /// <summary>
        /// Create Branch
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Json Record</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateBranchCommand command)
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
        /// Update Branch By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdateBranchCommand command, int Id)
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
         /// Delete Branch by ID
         /// </summary>
         /// <param name="command"></param>
         /// <param name="Id"> ID is require</param>
         /// <returns></returns>
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteBranchCommand command, int Id)
        {
            if (Id <= 0)
                return BadRequest("Not valid Id");
            if (ModelState.IsValid)
            {
                command.Id = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        ///  Get Branch by filtration List/Record
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetBranchDetailModel>> GetList(GetBranchListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get Branch Detail by ID
        /// </summary>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetBranchDetailQuery
            {
                Id = Id,
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Get Branch Dropdown List
        /// </summary>
        /// <returns></returns> 
        [DisableRBAC]
        [HttpGet("GetBranchDDL")]
        public async Task<IActionResult> GetBranchDDL()
        {
            var branch = new GetBranchDDLQuery
            {
                AllowedBranch =  _loggedInUser.AllowedBranch,
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            var result = await Mediator.Send(branch);
            return Ok(result);
        }
        /// <summary>
        /// Get Sub/Child Branch List
        /// </summary>
        /// <returns></returns>
        [DisableRBAC]
        [HttpGet("GetChildBranchDDL")]
        public async Task<IActionResult> GetChildBranchDDL()
        {
            var branch = new GetChildBranchDDLQuery
            {
                AllowedBranch = _loggedInUser.AllowedBranch,
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            ;
            var result = await Mediator.Send(branch);
            return Ok(result);
        }

    }
}
