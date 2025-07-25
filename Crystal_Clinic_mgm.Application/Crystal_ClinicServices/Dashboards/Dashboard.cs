using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Dashboards
{
    public class DashboardQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class DashboardHandler(ERP_DbContext context) : IRequestHandler<DashboardQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(DashboardQuery request, CancellationToken cancellationToken)
        {
            // Base queries
            var visitsQuery = context.Visit
                .Where(v => !v.IsDeleted)
                .AsQueryable();

            var sessionsQuery = context.ServiceSessions
                .Where(s => s.IsImplemented)
                .AsQueryable();

            var duesQuery = context.SupplierDue
                .Include(d => d.CurrencyType)
                .Where(d => !d.IsDeleted)
                .AsQueryable();

            // Apply date filters
            if (request.StartDate.HasValue)
            {
                visitsQuery = visitsQuery.Where(v => v.visitDate >= request.StartDate.Value);
                sessionsQuery = sessionsQuery.Where(s => s.ImplementationDate >= request.StartDate.Value);
                duesQuery = duesQuery.Where(d => d.CreatedOn >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                visitsQuery = visitsQuery.Where(v => v.visitDate <= request.EndDate.Value);
                sessionsQuery = sessionsQuery.Where(s => s.ImplementationDate <= request.EndDate.Value);
                duesQuery = duesQuery.Where(d => d.CreatedOn <= request.EndDate.Value);
            }

            // Calculate metrics
            var visitsByStatus = await visitsQuery
                .GroupBy(v => v.status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count, cancellationToken);

            var totalImplementedSessions = await sessionsQuery.CountAsync(cancellationToken);

            var duesByCurrency = await duesQuery
                .GroupBy(d => d.CurrencyType!.Code)
                .Select(g => new
                {
                    CurrencyCode = g.Key,
                    Summary = new DashboardDto.DueSummary
                    {
                        TotalDues = g.Count(),
                        TotalDueAmount = g.Sum(d => d.DueAmount)
                    }
                })
                .ToDictionaryAsync(k => k.CurrencyCode, v => v.Summary, cancellationToken);

            var totalPatients = await visitsQuery
                .Select(v => v.patientId)
                .Distinct()
                .CountAsync(cancellationToken);

            var totalEmployees = await context.EmployeeProfiles
                .CountAsync(e => !e.IsDeleted && e.IsActive, cancellationToken);

            var totalRevenue = await visitsQuery
                .SumAsync(v => v.paidAmount, cancellationToken);

            var totalOutstandingAmount = await visitsQuery
                .SumAsync(v => v.remainingAmount, cancellationToken);

            // Ensure all VisitStatus values are included, even if count is 0
            var allStatuses = Enum.GetValues(typeof(VisitStatus)).Cast<VisitStatus>();
            var visitsByStatusComplete = allStatuses.ToDictionary(s => s, s => visitsByStatus.ContainsKey(s) ? visitsByStatus[s] : 0);

            var dashboard = new DashboardDto
            {
                VisitsByStatus = visitsByStatusComplete,
                TotalImplementedSessions = totalImplementedSessions,
                SupplierDuesByCurrency = duesByCurrency,
                TotalPatients = totalPatients,
                TotalEmployees = totalEmployees,
                TotalRevenue = totalRevenue,
                TotalOutstandingAmount = totalOutstandingAmount
            };

            return new JsonResult(dashboard);
        }
    }
    public class DashboardDto
    {
        public Dictionary<VisitStatus, int> VisitsByStatus { get; set; } = new();
        public int TotalImplementedSessions { get; set; }
        public Dictionary<string, DueSummary> SupplierDuesByCurrency { get; set; } = new();
        public int TotalPatients { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalRevenue { get; set; } // Sum of Visit.paidAmount
        public decimal TotalOutstandingAmount { get; set; } // Sum of Visit.remainingAmount

        public class DueSummary
        {
            public int TotalDues { get; set; }
            public decimal TotalDueAmount { get; set; }
        }
    }
}