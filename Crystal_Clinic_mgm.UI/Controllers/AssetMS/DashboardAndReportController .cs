using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.AssetMS.Dashboards;
using Crystal_Clinic_Mgm.Application.AssetMS.Dashboards.BarChartofAssetByAssetTypesAndUser;
using Crystal_Clinic_Mgm.Application.AssetMS.Reports;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
namespace Crystal_Clinic_Mgm.UI.Controllers.AssetMS
{
    [Authorize]
    [RBAC]
    public class DashboardAndReportController : BaseController
    {

        /// <summary>
        /// Get MainAccount Barchart
        /// </summary>
        /// <returns></returns>      
        [HttpGet("BarChartofAssetByAssetTypesAndUser")]
        public async Task<IActionResult> BarChartofAssetByAssetTypesAndUser()
        {
            var model = new BarChartofAssetByAssetTypesAndUserQuery();
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Get Real Time Statistic Dashboard
        /// </summary>
        /// <returns></returns>      
        [HttpGet("RealTimeDashboard")]
        public async Task<IActionResult> RealTimeDashboard()
        {
            var model = new RealTimeDashboardQuery();
            return await Mediator.Send(model);
        }
        /// <summary>
        /// Get Trade Tracking line Chart
        /// </summary>
        /// <returns></returns>      
        [HttpGet("TradeTrackingChart")]
        public async Task<IActionResult> TradeTrackingChart()
        {
            var model = new TradeTrackingChartQuery();
            return await Mediator.Send(model);
        }

        /// <summary>
        /// Get Expense Chart
        /// </summary>
        /// <returns></returns>      
        [HttpGet("ExpenseChart")]
        public async Task<IActionResult> ExpenseChart()
        {
            var model = new ExpenseChartQuery();
            return await Mediator.Send(model);
        }
        
        /// <summary>
        /// Get MainAccount of current and child branchs
        /// </summary>
        /// <returns></returns>      
        [HttpGet("DashboardOfBranchsMainAccount")]
        public async Task<IActionResult> DashboardOfBranchsMainAccount()
        {
            var model = new BranchsMainAccountDashboardQuery();
            return await Mediator.Send(model);
        }

        

        /// <summary>
        /// Get Expense Report
        /// </summary>
        /// <returns></returns>      
        [HttpPost("GetExpenseReport")]
        public async Task<IActionResult> GetExpenseReport(ExpenseReportQuery query)
        {
            return await Mediator.Send(query);
        }


        /// <summary>
        /// Get Transaction Report
        /// </summary>
        /// <returns></returns>      
        [HttpPost("GetTransactionReport")]
        public async Task<IActionResult> GetTransactionReport(TransactionReportQuery query)
        {
            return await Mediator.Send(query);
        }

    }
}
