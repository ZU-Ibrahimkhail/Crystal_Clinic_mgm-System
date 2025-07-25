using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Dashboards
{
    public class VisitsByDoctorChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }

    public class VisitsByDoctorChartHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<VisitsByDoctorChartQuery, JsonResult>
    {
        private readonly ERP_DbContext _context = context;

        public async Task<JsonResult> Handle(VisitsByDoctorChartQuery request, CancellationToken cancellationToken)
        {
            var visitsQuery = _context.Visit
                .Include(v => v.Doctor)
                .Where(v => !v.IsDeleted);

            if (request.StartDateTime.HasValue)
                visitsQuery = visitsQuery.Where(v => v.visitDate >= request.StartDateTime.Value);
            if (request.EndDateTime.HasValue)
                visitsQuery = visitsQuery.Where(v => v.visitDate <= request.EndDateTime.Value);

            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            var dayData = await GetDailyDataAsync(visitsQuery, today, daysInMonth, cancellationToken);
            var weekData = await GetWeeklyDataAsync(visitsQuery, today, cancellationToken);
            var monthData = await GetMonthlyDataAsync(visitsQuery, today, cancellationToken);
            var yearData = await GetYearlyDataAsync(visitsQuery, cancellationToken);

            var chartLabels = new ChartModel
            {
                Day = Enumerable.Range(1, daysInMonth).Select(day => new DateTime(today.Year, today.Month, day).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(week => $"Week {week}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(month => new DateTime(today.Year, month, 1).ToString("MMM")).ToArray(),
                Year = yearData.Select(d => d.Year).Distinct().OrderBy(y => y).ToArray()
            };

            var doctors = await visitsQuery
                .Select(v => new { v.doctorId, DoctorName = v.Doctor != null ? $"{v.Doctor.firstName} {v.Doctor.lastName}" : "No Doctor" })
                .Distinct()
                .ToListAsync(cancellationToken);

            List<ChartData> chartData = [
                new()
                {
                    GroupName = "Day",
                    Data = doctors.Select(d => new Data
                    {
                        Name = d.DoctorName,
                        data = dayData.Select(dd => dd.Data.FirstOrDefault(x => x.DoctorId == d.doctorId).TotalVisits).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Week",
                    Data = doctors.Select(d => new Data
                    {
                        Name = d.DoctorName,
                        data = weekData.Select(wd => wd.Data.FirstOrDefault(x => x.DoctorId == d.doctorId).TotalVisits).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Month",
                    Data = doctors.Select(d => new Data
                    {
                        Name = d.DoctorName,
                        data = monthData.Select(md => md.Data.FirstOrDefault(x => x.DoctorId == d.doctorId).TotalVisits).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Year",
                    Data = doctors.Select(d => new Data
                    {
                        Name = d.DoctorName,
                        data = yearData.Select(yd => yd.Data.FirstOrDefault(x => x.DoctorId == d.doctorId).TotalVisits).ToArray()
                    }).ToList()
                }
            ];

            return new JsonResult(new { ChartLabels = chartLabels, ChartData = chartData });
        }

        private async Task<IEnumerable<(int Day, List<(int? DoctorId, decimal TotalVisits, decimal PaidAmount, decimal RemainingAmount)> Data)>> GetDailyDataAsync(IQueryable<Visit> visitsQuery, DateTime today, int daysInMonth, CancellationToken cancellationToken)
        {
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);

            var data = await visitsQuery
                .Where(v => v.visitDate >= startOfMonth && v.visitDate <= endOfMonth)
                .GroupBy(v => new { v.visitDate.Day, v.doctorId })
                .Select(g => new
                {
                    g.Key.Day,
                    DoctorId = g.Key.doctorId,
                    TotalVisits = g.Count(),
                    PaidAmount = g.Sum(v => v.paidAmount),
                    RemainingAmount = g.Sum(v => v.remainingAmount)
                })
                .ToListAsync(cancellationToken);

            return Enumerable.Range(1, daysInMonth)
                .Select(day => (Day: day, Data: data.Where(d => d.Day == day)
                    .Select(d => (d.DoctorId, TotalVisits: (decimal)d.TotalVisits, d.PaidAmount, d.RemainingAmount))
                    .ToList()));
        }

        private async Task<IEnumerable<(int Week, List<(int? DoctorId, decimal TotalVisits, decimal PaidAmount, decimal RemainingAmount)> Data)>> GetWeeklyDataAsync(IQueryable<Visit> visitsQuery, DateTime today, CancellationToken cancellationToken)
        {
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var data = await visitsQuery
                .Where(v => v.visitDate >= startOfYear && v.visitDate <= endOfYear)
                .GroupBy(v => new { Week = (v.visitDate.DayOfYear - 1) / 7 + 1, v.doctorId })
                .Select(g => new
                {
                    g.Key.Week,
                    DoctorId = g.Key.doctorId,
                    TotalVisits = g.Count(),
                    PaidAmount = g.Sum(v => v.paidAmount),
                    RemainingAmount = g.Sum(v => v.remainingAmount)
                })
                .ToListAsync(cancellationToken);

            return Enumerable.Range(1, 52)
                .Select(week => (Week: week, Data: data.Where(d => d.Week == week)
                    .Select(d => (d.DoctorId, TotalVisits: (decimal)d.TotalVisits, d.PaidAmount, d.RemainingAmount))
                    .ToList()));
        }

        private async Task<IEnumerable<(int Month, List<(int? DoctorId, decimal TotalVisits, decimal PaidAmount, decimal RemainingAmount)> Data)>> GetMonthlyDataAsync(IQueryable<Visit> visitsQuery, DateTime today, CancellationToken cancellationToken)
        {
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var data = await visitsQuery
                .Where(v => v.visitDate >= startOfYear && v.visitDate <= endOfYear)
                .GroupBy(v => new { v.visitDate.Month, v.doctorId })
                .Select(g => new
                {
                    g.Key.Month,
                    DoctorId = g.Key.doctorId,
                    TotalVisits = g.Count(),
                    PaidAmount = g.Sum(v => v.paidAmount),
                    RemainingAmount = g.Sum(v => v.remainingAmount)
                })
                .ToListAsync(cancellationToken);

            return Enumerable.Range(1, 12)
                .Select(month => (Month: month, Data: data.Where(d => d.Month == month)
                    .Select(d => (d.DoctorId, TotalVisits: (decimal)d.TotalVisits, d.PaidAmount, d.RemainingAmount))
                    .ToList()));
        }

        private async Task<IEnumerable<(string Year, List<(int? DoctorId, decimal TotalVisits, decimal PaidAmount, decimal RemainingAmount)> Data)>> GetYearlyDataAsync(IQueryable<Visit> visitsQuery, CancellationToken cancellationToken)
        {
            var data = await visitsQuery
                .GroupBy(v => new { v.visitDate.Year, v.doctorId })
                .Select(g => new
                {
                    g.Key.Year,
                    DoctorId = g.Key.doctorId,
                    TotalVisits = g.Count(),
                    PaidAmount = g.Sum(v => v.paidAmount),
                    RemainingAmount = g.Sum(v => v.remainingAmount)
                })
                .OrderBy(d => d.Year)
                .ToListAsync(cancellationToken);

            return data.GroupBy(d => d.Year)
                .Select(g => (Year: g.Key.ToString(), Data: g.Select(d => (d.DoctorId, TotalVisits: (decimal)d.TotalVisits, d.PaidAmount, d.RemainingAmount)).ToList()));
        }
    }
}