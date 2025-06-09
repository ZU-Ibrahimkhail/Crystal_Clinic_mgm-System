using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Commands.GeneratePayrollList;
using Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Commands.PaySalary;
using Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetDetail;
using Crystal_Clinic_Mgm.Common.Constants;
namespace Crystal_Clinic_Mgm.UI.Controllers.HR
{
    [Authorize]
    [RBAC]
    //[Tags("HR")]
    public class PayrollTrackingController : BaseController
    {
        [HttpGet("GeneratePayrollList")]
        public async Task<IActionResult> GeneratePayrollList()
        {
            return await Mediator.Send(new GeneratePayrollListCommand());
        }
        [HttpPut("PaySalary")]
        public async Task<IActionResult> PaySalary(PaySalaryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return await Mediator.Send(command);
            }
        }

        /// <summary>
        /// Search is done by employee and posation title
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetPayrollTrackingListModel>> GetList(GetPayrollTrackingListQuery query)
        {
            return await Mediator.Send(query);

        }
        [HttpGet("GetDetail/{Id:int}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var model = new GetPartnersDetailQuery
            {
                Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty,
                Id = Id
            };
            return await Mediator.Send(model);
        }
     



    }
}
