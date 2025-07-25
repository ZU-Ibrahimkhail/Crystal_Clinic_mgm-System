using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices
{
    public class VisitReportDto
    {
        public int VisitId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public VisitStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public int? CurrencyTypeId { get; set; }
        public string CurrencyTypeCode { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }

        public VisitReportDto(Visit visit)
        {
            VisitId = visit.visitId;
            PatientId = visit.patientId;
            PatientName = visit.Patient?.name ?? string.Empty;
            DoctorId = visit.doctorId;
            DoctorName = visit.Doctor != null ? $"{visit.Doctor.firstName} {visit.Doctor.lastName}" : string.Empty;
            VisitDate = visit.visitDate;
            Status = visit.status;
            TotalAmount = visit.totalAmount;
            PaidAmount = visit.paidAmount;
            RemainingAmount = visit.remainingAmount;
            CurrencyTypeId = visit.Payments.FirstOrDefault()?.CurrencyTypeId;
            CurrencyTypeCode = visit.Payments.FirstOrDefault()?.CurrencyType?.Code ?? "AFN";
            CreatedOn = visit.CreatedOn;
            CreatedBy = visit.CreatedBy;
        }
    }

    public class VisitReportResult
    {
        public List<VisitReportDto> Visits { get; set; } = new();
        public List<DoctorSummary> Summaries { get; set; } = new();
    }
    public class DoctorSummary
    {
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int TotalVisitCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalRemainingAmount { get; set; }
    }
    public class VisitReportQuery : IRequest<JsonResult>
    {
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public VisitStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CurrencyTypeId { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
    }

    public class VisitReportHandler(ERP_DbContext context) : IRequestHandler<VisitReportQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(VisitReportQuery request, CancellationToken cancellationToken)
        {
            var query = context.Visit
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Include(v => v.Payments)
                .ThenInclude(p => p.CurrencyType)
                .Where(v => !v.IsDeleted)
                .OrderByDescending(v => v.visitId)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.PatientName))
            {
                query = query.Where(v => v.Patient != null && v.Patient.name.Contains(request.PatientName));
            }

            if (!string.IsNullOrEmpty(request.DoctorName))
            {
                query = query.Where(v => v.Doctor != null && (v.Doctor.firstName + " " + v.Doctor.lastName).Contains(request.DoctorName));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(v => v.status == request.Status.Value);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(v => v.visitDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(v => v.visitDate <= request.EndDate.Value);
            }

            if (request.CurrencyTypeId.HasValue)
            {
                query = query.Where(v => v.Payments.Any(p => p.CurrencyTypeId == request.CurrencyTypeId.Value && !p.IsDeleted));
            }

            if (request.LastId.HasValue)
            {
                query = query.Where(v => v.visitId < request.LastId.Value);
            }

            // Validate PageSize
            if (request.PageSize <= 0 || request.PageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100.");
            }

            var visits = await query
                .Take(request.PageSize)
                .Select(v => new VisitReportDto(v))
                .ToListAsync(cancellationToken);

            var summaries = await query
                .GroupBy(v => v.doctorId)
                .Select(g => new DoctorSummary
                {
                    DoctorId = g.Key,
                    DoctorName = g.Key.HasValue ? g.First().Doctor != null ? $"{g.First().Doctor.firstName} {g.First().Doctor.lastName}" : "Unknown Doctor" : "No Doctor",
                    TotalVisitCount = g.Count(),
                    TotalAmount = g.Sum(v => v.totalAmount),
                    TotalPaidAmount = g.Sum(v => v.paidAmount),
                    TotalRemainingAmount = g.Sum(v => v.remainingAmount)
                }).ToListAsync(cancellationToken);

            return new JsonResult(new { Summaries = summaries, Visits = visits });
        }
    }
}
