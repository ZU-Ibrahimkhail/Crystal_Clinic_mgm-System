using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Dashboards
{
    public class SessionsByEmployeeChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class SessionsByEmployeeChartHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<SessionsByEmployeeChartQuery, JsonResult>
    {
        private readonly ERP_DbContext _context = context;

        public async Task<JsonResult> Handle(SessionsByEmployeeChartQuery request, CancellationToken cancellationToken)
        {
            var sessions = _context.ServiceSessions
                .Include(s => s.ImplementorEmployee)
                .Where(s => s.IsImplemented); // Only include implemented sessions

            if (request.StartDate.HasValue)
                sessions = sessions.Where(s => s.ImplementationDate >= request.StartDate.Value);
            if (request.EndDate.HasValue)
                sessions = sessions.Where(s => s.ImplementationDate <= request.EndDate.Value);

            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            var dayData = await GetDailyData(sessions, today, daysInMonth, cancellationToken);
            var weekData = await GetWeeklyData(sessions, today, cancellationToken);
            var monthData = await GetMonthlyData(sessions, today, cancellationToken);
            var yearData = await GetYearlyData(sessions, cancellationToken);

            var chartLabels = new ChartModel
            {
                Day = Enumerable.Range(1, daysInMonth).Select(day => new DateTime(today.Year, today.Month, day).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(week => $"Week {week}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(month => new DateTime(today.Year, month, 1).ToString("MMM")).ToArray(),
                Year = yearData.Select(d => d.Year).Distinct().OrderBy(y => y).ToArray()
            };

            var employees = await sessions
                .Select(s => new { s.ImplementorEmployeeId, EmployeeName = s.ImplementorEmployee != null ? $"{s.ImplementorEmployee.EnglishFirstName} {s.ImplementorEmployee.EnglishSurName}" : "No Employee" })
                .Distinct()
                .ToListAsync(cancellationToken);

            List<ChartData> chartData = [
                new()
                {
                    GroupName = "Day",
                    Data = employees.Select(e => new Data
                    {
                        Name = e.EmployeeName,
                        data = dayData.Select(dd => dd.Data.FirstOrDefault(x => x.EmployeeId == e.ImplementorEmployeeId).TotalSessions).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Week",
                    Data = employees.Select(e => new Data
                    {
                        Name = e.EmployeeName,
                        data = weekData.Select(wd => wd.Data.FirstOrDefault(x => x.EmployeeId == e.ImplementorEmployeeId).TotalSessions).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Month",
                    Data = employees.Select(e => new Data
                    {
                        Name = e.EmployeeName,
                        data = monthData.Select(md => md.Data.FirstOrDefault(x => x.EmployeeId == e.ImplementorEmployeeId).TotalSessions).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Year",
                    Data = employees.Select(e => new Data
                    {
                        Name = e.EmployeeName,
                        data = yearData.Select(yd => yd.Data.FirstOrDefault(x => x.EmployeeId == e.ImplementorEmployeeId).TotalSessions).ToArray()
                    }).ToList()
                }
            ];

            return new JsonResult(new { ChartLabels = chartLabels, ChartData = chartData });
        }

        private async Task<IEnumerable<(int Day, List<(int? EmployeeId, decimal TotalSessions, decimal TotalPriceInAFN)> Data)>> GetDailyData(IQueryable<ServiceSessions> sessions, DateTime today, int daysInMonth, CancellationToken cancellationToken)
        {
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);

            var data = await sessions
                .Where(s => s.ImplementationDate >= startOfMonth && s.ImplementationDate <= endOfMonth)
                .GroupBy(s => new { s.ImplementationDate!.Value.Day, s.ImplementorEmployeeId })
                .Select(g => new
                {
                    g.Key.Day,
                    EmployeeId = g.Key.ImplementorEmployeeId,
                    TotalSessions = g.Count(),
                    TotalPriceInAFN = g.Sum(s => s.PriceInAFN)
                })
                .ToListAsync(cancellationToken);

            return Enumerable.Range(1, daysInMonth)
                .Select(day => (day, Data: data.Where(d => d.Day == day)
                    .Select(d => (d.EmployeeId, (decimal)d.TotalSessions, d.TotalPriceInAFN))
                    .ToList()));
        }

        private async Task<IEnumerable<(int Week, List<(int? EmployeeId, decimal TotalSessions, decimal TotalPriceInAFN)> Data)>> GetWeeklyData(IQueryable<ServiceSessions> sessions, DateTime today, CancellationToken cancellationToken)
        {
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var data = await sessions
                .Where(s => s.ImplementationDate >= startOfYear && s.ImplementationDate <= endOfYear)
                .GroupBy(s => new { Week = (s.ImplementationDate!.Value.DayOfYear - 1) / 7 + 1, s.ImplementorEmployeeId })
                .Select(g => new
                {
                    g.Key.Week,
                    EmployeeId = g.Key.ImplementorEmployeeId,
                    TotalSessions = g.Count(),
                    TotalPriceInAFN = g.Sum(s => s.PriceInAFN)
                })
                .ToListAsync(cancellationToken);

            return Enumerable.Range(1, 52)
                .Select(week => (week, Data: data.Where(d => d.Week == week)
                    .Select(d => (d.EmployeeId, (decimal)d.TotalSessions, d.TotalPriceInAFN))
                    .ToList()));
        }

        private async Task<IEnumerable<(int Month, List<(int? EmployeeId, decimal TotalSessions, decimal TotalPriceInAFN)> Data)>> GetMonthlyData(IQueryable<ServiceSessions> sessions, DateTime today, CancellationToken cancellationToken)
        {
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var data = await sessions
                .Where(s => s.ImplementationDate >= startOfYear && s.ImplementationDate <= endOfYear)
                .GroupBy(s => new { s.ImplementationDate!.Value.Month, s.ImplementorEmployeeId })
                .Select(g => new
                {
                    g.Key.Month,
                    EmployeeId = g.Key.ImplementorEmployeeId,
                    TotalSessions = g.Count(),
                    TotalPriceInAFN = g.Sum(s => s.PriceInAFN)
                })
                .ToListAsync(cancellationToken);

            return Enumerable.Range(1, 12)
                .Select(month => (month, Data: data.Where(d => d.Month == month)
                    .Select(d => (d.EmployeeId, (decimal)d.TotalSessions, d.TotalPriceInAFN))
                    .ToList()));
        }

        private async Task<IEnumerable<(string Year, List<(int? EmployeeId, decimal TotalSessions, decimal TotalPriceInAFN)> Data)>> GetYearlyData(IQueryable<ServiceSessions> sessions, CancellationToken cancellationToken)
        {
            var data = await sessions
                .GroupBy(s => new { s.ImplementationDate!.Value.Year, s.ImplementorEmployeeId })
                .Select(g => new
                {
                    g.Key.Year,
                    EmployeeId = g.Key.ImplementorEmployeeId,
                    TotalSessions = g.Count(),
                    TotalPriceInAFN = g.Sum(s => s.PriceInAFN)
                })
                .OrderBy(d => d.Year)
                .ToListAsync(cancellationToken);

            return data.GroupBy(d => d.Year)
                .Select(g => (Year: g.Key.ToString(), Data: g.Select(d => (d.EmployeeId, (decimal)d.TotalSessions, d.TotalPriceInAFN)).ToList()));
        }
    }
}