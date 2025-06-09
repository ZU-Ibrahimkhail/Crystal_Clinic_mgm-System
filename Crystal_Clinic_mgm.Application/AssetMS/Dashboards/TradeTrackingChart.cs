using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.Dashboards
{
    #region Request
    public class TradeTrackingChartQuery : IRequest<JsonResult> { }

    #endregion

    #region Handler
    public class TradeTrackingService(ERP_DbContext context, IGeneralHelperRepositoryAsync _helper, ILoggedInUser _loggedInUser) : IRequestHandler<TradeTrackingChartQuery, JsonResult>
    {
        private readonly ERP_DbContext _context = context;

        public async Task<JsonResult> Handle(TradeTrackingChartQuery request, CancellationToken cancellationToken)
        {
            var ChildBranchIdList = await _helper.GetChildBranchs(_loggedInUser.BranchId);
            var trades = _context.TradeTracking.Where(x => !x.IsDeleted && (x.BranchId == _loggedInUser.BranchId || ChildBranchIdList.Contains(x.BranchId)));

            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var dayData = await GetDailyData(trades, today, daysInMonth);
            var weekData = await GetWeeklyData(trades, today);
            var monthData = await GetMonthlyData(trades, today);
            var yearData = await GetYearlyData(trades);

            var ChartLabels = new TradeTrackingChartModel
            {
                Day = Enumerable.Range(1, daysInMonth).Select(day => new DateTime(today.Year, today.Month, day).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(week => $"Week {week}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(month => new DateTime(today.Year, month, 1).ToString("MMM")).ToArray(),
                Year = yearData.Select(d => d.Year.ToString()).ToArray(),

            };

            List<ChartData> ChartData =
                [
                    new()
                    {
                        GroupName = "Day",
                        Data =
                        [
                            new() { Name = "Profit", data = dayData.Select(d => d.Profit).ToArray() },
                            new() { Name = "Loss", data = dayData.Select(d => d.Loss).ToArray() }
                        ]
                    },
                    new()
                    {
                        GroupName = "Week",
                        Data =
                        [
                            new() { Name = "Profit", data = weekData.Select(d => d.Profit).ToArray() },
                            new() { Name = "Loss", data = weekData.Select(d => d.Loss).ToArray() }
                        ]
                    },
                    new()
                    {
                        GroupName = "Month",
                        Data =
                    [
                        new() { Name = "Profit", data = monthData.Select(d => d.Profit).ToArray() },
                        new() { Name = "Loss", data = monthData.Select(d => d.Loss).ToArray() }
                    ]
                    },
                    new()
                    {
                        GroupName = "Year",
                        Data =
                    [
                        new() { Name = "Profit", data = yearData.Select(d => d.Profit).ToArray() },
                        new() { Name = "Loss", data = yearData.Select(d => d.Loss).ToArray() }
                    ]
                    }
                ];
            return new JsonResult(new { ChartLabels, ChartData });


        }


        #region Helper Methodes
        private async Task<IEnumerable<(float Profit, float Loss)>> GetDailyData(IQueryable<TradeTracking> trades, DateTime today, int daysInMonth)
        {
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);

            var data = await trades
                .Where(t => t.Date >= startOfMonth && t.Date <= endOfMonth)
                .GroupBy(t => t.Date.Day)
                .Select(g => new { Day = g.Key, Profit = g.Sum(t => t.ProfitAmount), Loss = g.Sum(t => t.LossAmount) })
                .ToListAsync();

            var result = Enumerable.Range(1, daysInMonth)
                .Select(day => data.FirstOrDefault(d => d.Day == day) ?? new { Day = day, Profit = 0f, Loss = 0f })
                .OrderBy(d => d.Day)
                .Select(d => (d.Profit, d.Loss));

            return result;
        }

        private async Task<IEnumerable<(float Profit, float Loss)>> GetWeeklyData(IQueryable<TradeTracking> trades, DateTime today)
        {
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var data = await trades
                .Where(t => t.Date >= startOfYear && t.Date <= endOfYear)
                .GroupBy(t => (t.Date.DayOfYear - 1) / 7 + 1)
                .Select(g => new { Week = g.Key, Profit = g.Sum(t => t.ProfitAmount), Loss = g.Sum(t => t.LossAmount) })
                .ToListAsync();

            var result = Enumerable.Range(1, 52)
                .Select(week => data.FirstOrDefault(d => d.Week == week) ?? new { Week = week, Profit = 0f, Loss = 0f })
                .OrderBy(d => d.Week)
                .Select(d => (d.Profit, d.Loss));

            return result;
        }

        private async Task<IEnumerable<(float Profit, float Loss)>> GetMonthlyData(IQueryable<TradeTracking> trades, DateTime today)
        {
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var data = await trades
                .Where(t => t.Date >= startOfYear && t.Date <= endOfYear)
                .GroupBy(t => t.Date.Month)
                .Select(g => new { Month = g.Key, Profit = g.Sum(t => t.ProfitAmount), Loss = g.Sum(t => t.LossAmount) })
                .ToListAsync();

            var result = Enumerable.Range(1, 12)
                .Select(month => data.FirstOrDefault(d => d.Month == month) ?? new { Month = month, Profit = 0f, Loss = 0f })
                .OrderBy(d => d.Month)
                .Select(d => (d.Profit, d.Loss));

            return result;
        }

        private async Task<IEnumerable<(string Year, float Profit, float Loss)>> GetYearlyData(IQueryable<TradeTracking> trades)
        {
            var data = await trades
                .GroupBy(t => t.Date.Year)
                .Select(g => new { Year = g.Key, Profit = g.Sum(t => t.ProfitAmount), Loss = g.Sum(t => t.LossAmount) })
                .OrderBy(d => d.Year)
                .ToListAsync();

            var result = data.Select(d => (Year: d.Year.ToString(), d.Profit, d.Loss));

            return result;
        }

        private async Task<string[]> GetYearLabels(IQueryable<TradeTracking> trades)
        {
            var data = await trades
                .Select(t => t.Date.Year)
                .Distinct()
                .OrderBy(year => year)
                .ToListAsync();

            return data.Select(year => year.ToString()).ToArray();
        }
        #endregion

    }

    #endregion

    #region Models
    public class TradeTrackingChartModel
    {
        public string[] Day { get; set; } = [];
        public string[] Week { get; set; } = [];
        public string[] Month { get; set; } = [];
        public string[] Year { get; set; } = [];
    }

    public class ChartData
    {
        public string GroupName { get; set; } = string.Empty;
        public List<Data> Data { get; set; } = [];
    }

    public class Data
    {
        public string Name { get; set; } = string.Empty;
        public float[] data { get; set; } = [];
    }
    #endregion

}
