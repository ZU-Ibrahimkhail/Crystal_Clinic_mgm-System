using Crystal_Clinic_Mgm.Application.Accounting.Dashboards;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Dashboards
{
    #region Bar Chart: Service Revenue by Service Type

    public class ServiceRevenueBarChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class ServiceRevenueBarChartHandler(ERP_DbContext context)
        : IRequestHandler<ServiceRevenueBarChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(ServiceRevenueBarChartQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var sessionsQuery = context.ServiceSessions
                .Where(s => s.IsImplemented && s.ImplementationDate.HasValue);

            if (request.BranchId.HasValue)
                sessionsQuery = sessionsQuery.Where(s => s.BranchId == request.BranchId);

            if (request.StartDate.HasValue)
                sessionsQuery = sessionsQuery.Where(s => s.ImplementationDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                sessionsQuery = sessionsQuery.Where(s => s.ImplementationDate <= request.EndDate.Value);

            var services = await sessionsQuery
                .Select(s => new { s.serviceId, s.serviceName })
                .Distinct()
                .ToListAsync(cancellationToken);

            var dailyData = await sessionsQuery
                .Where(s => s.ImplementationDate >= startOfMonth && s.ImplementationDate <= endOfMonth)
                .GroupBy(s => new { s.ImplementationDate!.Value.Day, s.serviceId, s.serviceName })
                .Select(g => new { g.Key.Day, g.Key.serviceId, g.Key.serviceName, Revenue = g.Sum(s => s.PriceInAFN), Count = g.Count() })
                .ToListAsync(cancellationToken);

            var monthlyData = await sessionsQuery
                .Where(s => s.ImplementationDate >= startOfYear && s.ImplementationDate <= endOfYear)
                .GroupBy(s => new { s.ImplementationDate!.Value.Month, s.serviceId, s.serviceName })
                .Select(g => new { g.Key.Month, g.Key.serviceId, g.Key.serviceName, Revenue = g.Sum(s => s.PriceInAFN), Count = g.Count() })
                .ToListAsync(cancellationToken);

            var weeklyData = await sessionsQuery
                .Where(s => s.ImplementationDate >= startOfYear && s.ImplementationDate <= endOfYear)
                .GroupBy(s => new { Week = (s.ImplementationDate!.Value.DayOfYear - 1) / 7 + 1, s.serviceId, s.serviceName })
                .Select(g => new { g.Key.Week, g.Key.serviceId, g.Key.serviceName, Revenue = g.Sum(s => s.PriceInAFN), Count = g.Count() })
                .ToListAsync(cancellationToken);

            var yearlyData = await sessionsQuery
                .GroupBy(s => new { s.ImplementationDate!.Value.Year, s.serviceId, s.serviceName })
                .Select(g => new { g.Key.Year, g.Key.serviceId, g.Key.serviceName, Revenue = g.Sum(s => s.PriceInAFN), Count = g.Count() })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var allYears = yearlyData.Select(y => y.Year).Distinct().OrderBy(y => y).ToArray();

            var labels = new TimeSeriesLabelsDto
            {
                Day = Enumerable.Range(1, daysInMonth).Select(d => new DateTime(today.Year, today.Month, d).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(w => $"Week {w}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(m => new DateTime(today.Year, m, 1).ToString("MMM")).ToArray(),
                Year = allYears.Select(y => y.ToString()).ToArray()
            };

            var groups = new List<GroupedSeriesDto>
            {
                new()
                {
                    GroupName = "Day",
                    Series = services.Select(svc => new ChartSeriesDto
                    {
                        Name = svc.serviceName,
                        Data = Enumerable.Range(1, daysInMonth).Select(d => dailyData.Where(x => x.Day == d && x.serviceId == svc.serviceId).Sum(x => x.Revenue)).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Week",
                    Series = services.Select(svc => new ChartSeriesDto
                    {
                        Name = svc.serviceName,
                        Data = Enumerable.Range(1, 52).Select(w => weeklyData.Where(x => x.Week == w && x.serviceId == svc.serviceId).Sum(x => x.Revenue)).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Month",
                    Series = services.Select(svc => new ChartSeriesDto
                    {
                        Name = svc.serviceName,
                        Data = Enumerable.Range(1, 12).Select(m => monthlyData.Where(x => x.Month == m && x.serviceId == svc.serviceId).Sum(x => x.Revenue)).ToArray()
                    }).ToList()
                },
                new()
                {
                    GroupName = "Year",
                    Series = services.Select(svc => new ChartSeriesDto
                    {
                        Name = svc.serviceName,
                        Data = allYears.Select(y => yearlyData.Where(x => x.Year == y && x.serviceId == svc.serviceId).Sum(x => x.Revenue)).ToArray()
                    }).ToList()
                }
            };

            return new JsonResult(new MultiSeriesChartDto { Labels = labels, Groups = groups });
        }
    }

    #endregion

    #region Line Chart: Patient Visit Trend

    public class PatientVisitTrendLineChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class PatientVisitTrendLineChartHandler(ERP_DbContext context)
        : IRequestHandler<PatientVisitTrendLineChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(PatientVisitTrendLineChartQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var visitsQuery = context.Visit
                .Where(v => !v.IsDeleted);

            if (request.BranchId.HasValue)
                visitsQuery = visitsQuery.Where(v => v.BranchId == request.BranchId);

            if (request.StartDate.HasValue)
                visitsQuery = visitsQuery.Where(v => v.visitDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                visitsQuery = visitsQuery.Where(v => v.visitDate <= request.EndDate.Value);

            var dailyVisits = await visitsQuery
                .Where(v => v.visitDate >= startOfMonth && v.visitDate <= endOfMonth)
                .GroupBy(v => v.visitDate.Day)
                .Select(g => new
                {
                    Day = g.Key,
                    TotalVisits = g.Count(),
                    TotalRevenue = g.Sum(v => v.paidAmount),
                    UniquePatients = g.Select(v => v.patientId).Distinct().Count()
                })
                .ToListAsync(cancellationToken);

            var weeklyVisits = await visitsQuery
                .Where(v => v.visitDate >= startOfYear && v.visitDate <= endOfYear)
                .GroupBy(v => (v.visitDate.DayOfYear - 1) / 7 + 1)
                .Select(g => new
                {
                    Week = g.Key,
                    TotalVisits = g.Count(),
                    TotalRevenue = g.Sum(v => v.paidAmount),
                    UniquePatients = g.Select(v => v.patientId).Distinct().Count()
                })
                .ToListAsync(cancellationToken);

            var monthlyVisits = await visitsQuery
                .Where(v => v.visitDate >= startOfYear && v.visitDate <= endOfYear)
                .GroupBy(v => v.visitDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalVisits = g.Count(),
                    TotalRevenue = g.Sum(v => v.paidAmount),
                    UniquePatients = g.Select(v => v.patientId).Distinct().Count()
                })
                .ToListAsync(cancellationToken);

            var yearlyVisits = await visitsQuery
                .GroupBy(v => v.visitDate.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    TotalVisits = g.Count(),
                    TotalRevenue = g.Sum(v => v.paidAmount),
                    UniquePatients = g.Select(v => v.patientId).Distinct().Count()
                })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var allYears = yearlyVisits.Select(y => y.Year).OrderBy(y => y).ToArray();

            var labels = new TimeSeriesLabelsDto
            {
                Day = Enumerable.Range(1, daysInMonth).Select(d => new DateTime(today.Year, today.Month, d).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(w => $"Week {w}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(m => new DateTime(today.Year, m, 1).ToString("MMM")).ToArray(),
                Year = allYears.Select(y => y.ToString()).ToArray()
            };

            var groups = new List<GroupedSeriesDto>
            {
                new()
                {
                    GroupName = "Day",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Total Visits", Data = Enumerable.Range(1, daysInMonth).Select(d => (decimal)(dailyVisits.FirstOrDefault(x => x.Day == d)?.TotalVisits ?? 0)).ToArray() },
                        new ChartSeriesDto { Name = "Revenue", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyVisits.FirstOrDefault(x => x.Day == d)?.TotalRevenue ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Unique Patients", Data = Enumerable.Range(1, daysInMonth).Select(d => (decimal)(dailyVisits.FirstOrDefault(x => x.Day == d)?.UniquePatients ?? 0)).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Week",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Total Visits", Data = Enumerable.Range(1, 52).Select(w => (decimal)(weeklyVisits.FirstOrDefault(x => x.Week == w)?.TotalVisits ?? 0)).ToArray() },
                        new ChartSeriesDto { Name = "Revenue", Data = Enumerable.Range(1, 52).Select(w => weeklyVisits.FirstOrDefault(x => x.Week == w)?.TotalRevenue ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Unique Patients", Data = Enumerable.Range(1, 52).Select(w => (decimal)(weeklyVisits.FirstOrDefault(x => x.Week == w)?.UniquePatients ?? 0)).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Month",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Total Visits", Data = Enumerable.Range(1, 12).Select(m => (decimal)(monthlyVisits.FirstOrDefault(x => x.Month == m)?.TotalVisits ?? 0)).ToArray() },
                        new ChartSeriesDto { Name = "Revenue", Data = Enumerable.Range(1, 12).Select(m => monthlyVisits.FirstOrDefault(x => x.Month == m)?.TotalRevenue ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Unique Patients", Data = Enumerable.Range(1, 12).Select(m => (decimal)(monthlyVisits.FirstOrDefault(x => x.Month == m)?.UniquePatients ?? 0)).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Year",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Total Visits", Data = allYears.Select(y => (decimal)(yearlyVisits.FirstOrDefault(x => x.Year == y)?.TotalVisits ?? 0)).ToArray() },
                        new ChartSeriesDto { Name = "Revenue", Data = allYears.Select(y => yearlyVisits.FirstOrDefault(x => x.Year == y)?.TotalRevenue ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Unique Patients", Data = allYears.Select(y => (decimal)(yearlyVisits.FirstOrDefault(x => x.Year == y)?.UniquePatients ?? 0)).ToArray() }
                    ]
                }
            };

            return new JsonResult(new MultiSeriesChartDto { Labels = labels, Groups = groups });
        }
    }

    #endregion

    #region Pie Chart: Visit Status & Payment Distribution

    public class VisitStatusPieChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class VisitStatusPieChartHandler(ERP_DbContext context)
        : IRequestHandler<VisitStatusPieChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(VisitStatusPieChartQuery request, CancellationToken cancellationToken)
        {
            var visitsQuery = context.Visit
                .Where(v => !v.IsDeleted);

            if (request.BranchId.HasValue)
                visitsQuery = visitsQuery.Where(v => v.BranchId == request.BranchId);

            if (request.StartDate.HasValue)
                visitsQuery = visitsQuery.Where(v => v.visitDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                visitsQuery = visitsQuery.Where(v => v.visitDate <= request.EndDate.Value);

            var statusDistribution = await visitsQuery
                .GroupBy(v => v.status)
                .Select(g => new PieSliceDto
                {
                    Label = g.Key.ToString(),
                    Value = g.Count()
                })
                .ToListAsync(cancellationToken);

            var allStatuses = Enum.GetValues(typeof(VisitStatus)).Cast<VisitStatus>();
            var statusDistributionComplete = allStatuses
                .Select(s => statusDistribution.FirstOrDefault(x => x.Label == s.ToString()) ?? new PieSliceDto { Label = s.ToString(), Value = 0 })
                .ToList();

            var revenueByStatus = await visitsQuery
                .GroupBy(v => v.status)
                .Select(g => new PieSliceDto
                {
                    Label = g.Key.ToString(),
                    Value = g.Sum(v => v.paidAmount)
                })
                .ToListAsync(cancellationToken);

            var paymentCompletionSlices = new List<PieSliceDto>
            {
                new() { Label = "Fully Paid", Value = await visitsQuery.CountAsync(v => v.remainingAmount == 0 && v.paidAmount > 0, cancellationToken) },
                new() { Label = "Partially Paid", Value = await visitsQuery.CountAsync(v => v.remainingAmount > 0 && v.paidAmount > 0, cancellationToken) },
                new() { Label = "Unpaid", Value = await visitsQuery.CountAsync(v => v.paidAmount == 0, cancellationToken) }
            };

            var sessionCompletionSlices = await context.ServiceSessions
                .Where(s => (request.BranchId == null || s.BranchId == request.BranchId))
                .GroupBy(s => s.IsImplemented ? "Completed" : "Pending")
                .Select(g => new PieSliceDto
                {
                    Label = g.Key,
                    Value = g.Count()
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(new
            {
                VisitsByStatus = statusDistributionComplete,
                RevenueByStatus = revenueByStatus,
                PaymentCompletion = paymentCompletionSlices,
                SessionCompletion = sessionCompletionSlices
            });
        }
    }

    #endregion

    #region Spider Chart: Doctor Performance

    public class DoctorPerformanceSpiderChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class DoctorPerformanceSpiderChartHandler(ERP_DbContext context)
        : IRequestHandler<DoctorPerformanceSpiderChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(DoctorPerformanceSpiderChartQuery request, CancellationToken cancellationToken)
        {
            var visitsQuery = context.Visit
                .Include(v => v.Doctor)
                .Where(v => !v.IsDeleted && v.doctorId.HasValue);

            if (request.BranchId.HasValue)
                visitsQuery = visitsQuery.Where(v => v.BranchId == request.BranchId);

            if (request.StartDate.HasValue)
                visitsQuery = visitsQuery.Where(v => v.visitDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                visitsQuery = visitsQuery.Where(v => v.visitDate <= request.EndDate.Value);

            var doctorStats = await visitsQuery
                .GroupBy(v => new
                {
                    v.doctorId,
                    DoctorName = v.Doctor != null ? $"{v.Doctor.firstName} {v.Doctor.lastName}" : "Unknown"
                })
                .Select(g => new
                {
                    g.Key.doctorId,
                    g.Key.DoctorName,
                    TotalVisits = g.Count(),
                    CompletedVisits = g.Count(v => v.status == VisitStatus.COMPLETED),
                    PendingVisits = g.Count(v => v.status == VisitStatus.PENDING || v.status == VisitStatus.SCHEDULED),
                    InProcessVisits = g.Count(v => v.status == VisitStatus.IN_PROCESS),
                    TotalRevenue = g.Sum(v => v.paidAmount),
                    TotalOutstanding = g.Sum(v => v.remainingAmount),
                    UniquePatients = g.Select(v => v.patientId).Distinct().Count()
                })
                .OrderByDescending(d => d.TotalVisits)
                .ToListAsync(cancellationToken);

            var doctorNames = doctorStats.Select(d => d.DoctorName).ToArray();
            var axes = new[] { "Total Visits", "Completed", "Pending", "In Process", "Revenue", "Outstanding", "Unique Patients" };

            var series = new List<ChartSeriesDto>
            {
                new() { Name = "Total Visits", Data = doctorStats.Select(d => (decimal)d.TotalVisits).ToArray() },
                new() { Name = "Completed", Data = doctorStats.Select(d => (decimal)d.CompletedVisits).ToArray() },
                new() { Name = "Pending", Data = doctorStats.Select(d => (decimal)d.PendingVisits).ToArray() },
                new() { Name = "In Process", Data = doctorStats.Select(d => (decimal)d.InProcessVisits).ToArray() },
                new() { Name = "Revenue", Data = doctorStats.Select(d => d.TotalRevenue).ToArray() },
                new() { Name = "Outstanding", Data = doctorStats.Select(d => d.TotalOutstanding).ToArray() },
                new() { Name = "Unique Patients", Data = doctorStats.Select(d => (decimal)d.UniquePatients).ToArray() }
            };

            return new JsonResult(new
            {
                Categories = doctorNames,
                Axes = axes,
                Series = series,
                DoctorDetails = doctorStats
            });
        }
    }

    #endregion
}
