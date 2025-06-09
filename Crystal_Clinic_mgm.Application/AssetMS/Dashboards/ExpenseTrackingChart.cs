using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.Dashboards
{
    #region Request
    public class ExpenseChartQuery : IRequest<JsonResult> { }

    #endregion

    #region Handler
    public class ExpenseService(ERP_DbContext context, IGeneralHelperRepositoryAsync helper, ILoggedInUser loggedInUser, IHttpContextAccessor contextAccessor) : IRequestHandler<ExpenseChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(ExpenseChartQuery request, CancellationToken cancellationToken)
        {
            Localization localize = new(contextAccessor);
            var today = DateTime.Today;

            var ChildBranchIdList = await helper.GetChildBranchs(loggedInUser.BranchId);
            var expenses = await context.ExpenseTracking
                .Where(x => !x.IsDeleted && (x.BranchId == loggedInUser.BranchId || ChildBranchIdList.Contains(x.BranchId)))
                .Include(x => x.ExpenseType)
                .ToListAsync();

            var dailyData = GetGroupedData(expenses, localize, today, GroupingType.Day);
            var weeklyData = GetGroupedData(expenses, localize, today, GroupingType.Week);
            var monthlyData = GetGroupedData(expenses, localize, today, GroupingType.Month);
            var yearlyData = GetGroupedData(expenses, localize, today, GroupingType.Year);

            return new JsonResult(new
            {
                Day = dailyData,
                Week = weeklyData,
                Month = monthlyData,
                Year = yearlyData
            });
        }

        private enum GroupingType
        {
            Day,
            Week,
            Month,
            Year
        }

        private IEnumerable<ExpenseChartData> GetGroupedData(List<ExpenseTracking> expenses, Localization localize, DateTime referenceDate, GroupingType groupingType)
        {
            return groupingType switch
            {
                GroupingType.Day => expenses
                    .Where(e => e.Date.Month == referenceDate.Month && e.Date.Year == referenceDate.Year)
                    .GroupBy(e => e.Date.Day)
                    .Select(g => new ExpenseChartData
                    {
                        Label = $"{g.First().Date:dd MMM}",
                        Value = g.Sum(v => v.Amount)
                    }).ToList(),

                GroupingType.Week => expenses
                    .Where(e => e.Date.Year == referenceDate.Year)
                    .GroupBy(e => (e.Date.DayOfYear - 1) / 7 + 1)
                    .Select(g => new ExpenseChartData
                    {
                        Label = $"Week {g.Key}",
                        Value = g.Sum(v => v.Amount)
                    }).ToList(),

                GroupingType.Month => expenses
                    .Where(e => e.Date.Year == referenceDate.Year)
                    .GroupBy(e => e.Date.Month)
                    .Select(g => new ExpenseChartData
                    {
                        Label = $"{g.First().Date:MMM}",
                        Value = g.Sum(v => v.Amount)
                    }).ToList(),

                GroupingType.Year => expenses
                    .GroupBy(e => e.Date.Year)
                    .Select(g => new ExpenseChartData
                    {
                        Label = $"{g.First().Date:yyyy}",
                        Value = g.Sum(v => v.Amount)
                    }).ToList(),

                _ => throw new ArgumentOutOfRangeException(nameof(groupingType), groupingType, null)
            };
        }
    }
    #endregion


    #region Models
    public class ExpenseChartData
    {
        public string Label { get; set; } = string.Empty;
        public float Value { get; set; } 
    }

    #endregion

}
