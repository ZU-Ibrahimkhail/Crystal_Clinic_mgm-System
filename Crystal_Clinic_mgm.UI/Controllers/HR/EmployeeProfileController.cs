using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.Create;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.Delete;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.Update;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.UpdateProfileImage;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetActiveEmplyeeDDLByDepartment;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetAuthoritiesDDL;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDDL;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDDLByDepartmentId;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDetails;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileForEdit;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileList;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.HR
{
    [Authorize]
    [RBAC]
    //[Tags("HR")]
    public class EmployeeProfileController(ILoggedInUser loggedInUser) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateEmployeeProfileCommand command)
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
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update([FromForm] UpdateEmployeeProfileCommand command, int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                command.ID = Id;
                return await Mediator.Send(command);
            }
        }
        /// <summary>
        /// UpdateProfileImage for Employees
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <remarks>ID and Prpfile image is required</remarks>
        [HttpPut("UpdateProfileImage{Id:int}")]
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateEmployeeProfileImageCommand command, int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                command.ID = Id;
                return await Mediator.Send(command);
            }
        }
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteEmployeeProfileCommand command, int Id)
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
        /// Search is done by 
        ///  Name
        ///  SurName
        ///  PhoneNumber
        ///  BranchName
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        [DisableRBAC]
        public async Task<ResponseDataTable<GetEmployeeProfileListModel>> GetAll(GetEmployeeProfileListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);

        }
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetEmployeeProfileDetailsQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                Id = Id
            };
            return await Mediator.Send(model);
        }
        [HttpGet("GetEmployeeProfileForEdit/{Id:int}")]
        public async Task<IActionResult> GetEmployeeProfileForEdit(int Id)
        {
            var model = new GetEmployeeProfileForEditQuery
            {
                Id = Id
            };
            return await Mediator.Send(model);
        }
        [HttpGet("GetEmployeeProfileDDL/{GetAll:bool}")]
        [DisableRBAC]
        public async Task<IActionResult> GetEmployeeProfileDDL(bool GetAll)
        {
            var command = new GetEmployeeProfileDDLQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                GetAll = GetAll
            };
            return await Mediator.Send(command);
        }

        [HttpGet("GetEmployeeProfileDDLByBranchId/{BranchId:int}")]
        [DisableRBAC]
        public async Task<IActionResult> GetEmployeeProfileDDLByBranchId(int BranchId)
        {
            var model = new GetEmployeeProfileDDLByBranchIdQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                BranchId = BranchId
            };
            return await Mediator.Send(model);
        }
        [HttpGet("GetActiveEmplyeeDDLByBranch/{BranchId:int}")]
        [DisableRBAC]
        public async Task<IActionResult> GetActiveEmplyeeDDLByBranch(int BranchId)
        {
            var model = new GetActiveEmplyeeDDLByBranchQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                BranchId = BranchId
            };
            return await Mediator.Send(model);
        }

        [HttpGet("GetAuthoritiesDDL")]
        [DisableRBAC]
        public async Task<IActionResult> GetAuthoritiesDDL()
        {
            var model = new GetAuthoritiesDDLQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };

            var result = await Mediator.Send(model);
            return Ok(result);
        }
        [HttpGet("GetEmplyeeDDLOfCurrentBranch")]
        [DisableRBAC]
        public async Task<IActionResult> GetEmplyeeDDLOfCurrentBranch()
        {
            var model = new GetEmployeeProfileDDLByBranchIdQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                BranchId = loggedInUser.BranchId
            };
            return await Mediator.Send(model);
        }



    }
}
