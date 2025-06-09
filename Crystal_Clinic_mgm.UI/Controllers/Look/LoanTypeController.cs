using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.LoanTypes.Commands.Create;
using Crystal_Clinic_Mgm.Application.Look.LoanTypes.Commands.Delete;
using Crystal_Clinic_Mgm.Application.Look.LoanTypes.Commands.Update;
using Crystal_Clinic_Mgm.Application.Look.LoanTypes.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.Look.LoanTypes.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Look.LoanTypes.Queries.GetList;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.Look
{
    [Authorize]
    [RBAC]
    public class LoanTypeController : BaseController
    {

        /// <summary>
        /// Create LoanType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Json Record</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateLoanTypeCommand command)
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
        /// Update LoanType By ID
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id">ID is require</param>
        /// <returns></returns>
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdateLoanTypeCommand command, int Id)
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
         /// Delete LoanType by ID
         /// </summary>
         /// <param name="command"></param>
         /// <param name="Id"> ID is require</param>
         /// <returns></returns>
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteLoanTypeCommand command, int Id)
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
        ///  Get LoanType by filtration List/Record
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetLoanTypeDetailModel>> GetList(GetLoanTypeListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            return await Mediator.Send(query);
        }
        /// <summary>
        /// Get LoanType Detail by ID
        /// </summary>
        /// <param name="Id">ID is require </param>
        /// <returns></returns>      
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetLoanTypeDetailQuery
            {
                Id = Id,
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Get LoanType Dropdown List
        /// </summary>
        /// <returns></returns> 
        [DisableRBAC]
        [HttpGet("GetLoanTypeDDL")]
        public async Task<IActionResult> GetLoanTypeDDL()
        {
            var branch = new GetLoanTypeDDLQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty
            };
            var result = await Mediator.Send(branch);
            return Ok(result);
        }
    }
}
