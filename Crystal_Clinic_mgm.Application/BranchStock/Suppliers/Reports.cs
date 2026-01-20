using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.Suppliers
{
    public class SupplierDueReportDTO
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierContactPerson { get; set; } = string.Empty;
        public string SupplierContactInfo { get; set; } = string.Empty;
        public decimal DueAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public int CurrencyTypeId { get; set; }
        public string CurrencyTypeCode { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }

        public SupplierDueReportDTO(SupplierDue due)
        {
            Id = due.Id;
            SupplierId = due.SupplierId;
            SupplierName = due.Supplier?.Name ?? string.Empty;
            SupplierContactPerson = due.Supplier?.ContactPerson ?? string.Empty;
            SupplierContactInfo = due.Supplier?.ContactInfo ?? string.Empty;
            DueAmount = due.DueAmount;
            PaidAmount = due.PaidAmount;
            RemainAmount = due.RemainAmount;
            CurrencyTypeId = due.CurrencyTypeId;
            CurrencyTypeCode = due.CurrencyType?.Code ?? string.Empty;
            DueDate = due.DueDate;
            CreatedOn = due.CreatedOn;
            CreatedBy = due.CreatedBy;
        }
    }
    public class SupplierDuesReportResult
    {
        public int currencyTypeId { get; set; }
        public string? currencyTypeName { get; set; }
        public decimal TotalDueAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalRemainAmount { get; set; }
    }
    
    public class SupplierDuesReportQuery : IRequest<JsonResult>
    {
        public string? SupplierName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CurrencyTypeId { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
    }
    public class SupplierDuesReportHandler(ERP_DbContext context)
    : IRequestHandler<SupplierDuesReportQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(SupplierDuesReportQuery request, CancellationToken cancellationToken)
        {
            var query = context.SupplierDue
                .Include(sd => sd.Supplier)
                .Include(sd => sd.CurrencyType)
                .Where(sd => !sd.IsDeleted)
                .OrderByDescending(sd => sd.Id)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SupplierName))
            {
                query = query.Where(sd => sd.Supplier != null && sd.Supplier.Name.Contains(request.SupplierName));
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(sd => sd.CreatedOn >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(sd => sd.CreatedOn <= request.EndDate.Value);
            }

            if (request.CurrencyTypeId.HasValue)
            {
                query = query.Where(sd => sd.CurrencyTypeId == request.CurrencyTypeId.Value);
            }

            if (request.LastId.HasValue)
            {
                query = query.Where(sd => sd.Id < request.LastId.Value);
            }
            var list = await query.Take(request.PageSize).Select(sd => new SupplierDueReportDTO(sd)).ToListAsync(cancellationToken);

            // Validate PageSize
            if (request.PageSize <= 0 || request.PageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100.");
            }
            var summary = await query
                        .GroupBy(x => x.CurrencyTypeId)
                        .Select(g => new SupplierDuesReportResult
                        {
                            currencyTypeId = g.Key,
                            currencyTypeName = g.First().CurrencyType!.Code,
                            TotalDueAmount = g.Sum(sd => sd.DueAmount),
                            TotalPaidAmount = g.Sum(sd => sd.PaidAmount),
                            TotalRemainAmount = g.Sum(sd => sd.RemainAmount)
                        })
                        .FirstOrDefaultAsync(cancellationToken);

            return new JsonResult(new { summary, list });
            
         }
    }

    public class DuePaymentReportDTO(DuePayment payment)
    {
        public int DuePaymentId { get; set; } = payment.DuePaymentId;
        public int SupplierDueId { get; set; } = payment.SupplierDueId;
        public int SupplierId { get; set; } = payment.SupplierDue?.SupplierId ?? 0;
        public string SupplierName { get; set; } = payment.SupplierDue?.Supplier?.Name ?? string.Empty;
        public int? CurrencyTypeId { get; set; } = payment.CurrencyTypeId;
        public string CurrencyTypeCode { get; set; } = payment.CurrencyType?.Code ?? string.Empty;
        public decimal ExchangeRateToDueCurrency { get; set; } = payment.ExchangeRateToDueCurrency;
        public decimal AmountPaid { get; set; } = payment.AmountPaid; // Note: Typo in entity, should be AmountPaid
        public decimal AmountInDueCurrency { get; set; } = payment.AmountInDueCurrency;
        public DateTime PaymentDate { get; set; } = payment.paymentDate;
        public string? Remarks { get; set; } = payment.Remarks;
        public List<string>? AttachmentPath { get; set; } = payment.AttachmentPath;
        public DateTime CreatedOn { get; set; } = payment.CreatedOn;
        public Guid CreatedBy { get; set; } = payment.CreatedBy;
    }
    public class DuePaymentsReportQuery : IRequest<List<DuePaymentReportDTO>>
    {
        public string? SupplierName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CurrencyTypeId { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
    }

    public class DuePaymentsReportHandler(ERP_DbContext context)
    : IRequestHandler<DuePaymentsReportQuery, List<DuePaymentReportDTO>>
    {
        public async Task<List<DuePaymentReportDTO>> Handle(DuePaymentsReportQuery request, CancellationToken cancellationToken)
        {
            var query = context.DuePayment
                .Include(dp => dp.SupplierDue)
                .ThenInclude(sd => sd!.Supplier)
                .Include(dp => dp.CurrencyType)
                .Where(dp => !dp.IsDeleted)
                .OrderByDescending(dp => dp.DuePaymentId)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SupplierName))
            {
                query = query.Where(dp => dp.SupplierDue != null && dp.SupplierDue.Supplier != null
                    && dp.SupplierDue.Supplier.Name.Contains(request.SupplierName));
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(dp => dp.paymentDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(dp => dp.paymentDate <= request.EndDate.Value);
            }

            if (request.CurrencyTypeId.HasValue)
            {
                query = query.Where(dp => dp.CurrencyTypeId == request.CurrencyTypeId.Value);
            }

            if (request.LastId.HasValue)
            {
                query = query.Where(dp => dp.DuePaymentId < request.LastId.Value);
            }

            // Validate PageSize
            if (request.PageSize <= 0 || request.PageSize > 100)
            {
                throw new ArgumentException("Page size must be between 1 and 100.");
            }

            return await query
                .Take(request.PageSize)
                .Select(dp => new DuePaymentReportDTO(dp))
                .ToListAsync(cancellationToken);
        }
    }
}
