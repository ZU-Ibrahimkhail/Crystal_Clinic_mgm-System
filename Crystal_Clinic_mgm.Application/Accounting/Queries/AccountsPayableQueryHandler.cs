using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Repositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Accounts Payable
    public class GetAllAccountsPayableQuery : IRequest<Result>
    {
        public APStatus? Status { get; set; }
        public int? VendorId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllAccountsPayableQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllAccountsPayableQuery, Result>
    {
        public async Task<Result> Handle(GetAllAccountsPayableQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.AccountsPayables.Include(x=>x.Vendor).Where(a => !a.IsDeleted);

                if (request.Status.HasValue)
                    query = query.Where(a => a.Status == request.Status);

                if (request.VendorId.HasValue)
                    query = query.Where(a => a.VendorId == request.VendorId);

                var totalCount = await query.CountAsync(cancellationToken);

                var payables = await query
                    .OrderByDescending(a => a.InvoiceDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(a => new AccountsPayableDto
                    {
                        Id = a.Id,
                        InvoiceNumber = a.InvoiceNumber,
                        VendorId = a.VendorId,
                        VendorName = a.Vendor!.Name,
                        InvoiceDate = a.InvoiceDate,
                        DueDate = a.DueDate,
                        InvoiceAmount = a.InvoiceAmount,
                        PaidAmount = a.PaidAmount,
                        BalanceAmount = a.BalanceAmount,
                        Status = a.Status,
                        BranchId = a.BranchId
                    })
                    .ToListAsync(cancellationToken);

                var result = new
                {
                    payables,
                    totalCount,
                    pageNumber = request.PageNumber,
                    pageSize = request.PageSize
                };

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Accounts Payable: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Accounts Payable By Id
    public class GetAccountsPayableByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetAccountsPayableByIdQueryHandler(ERP_DbContext context) : IRequestHandler<GetAccountsPayableByIdQuery, Result>
    {
        public async Task<Result> Handle(GetAccountsPayableByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var payable = await context.AccountsPayables
                    .Include(a => a.Vendor)
                    .Include(a => a.ChartOfAccount)
                    .FirstOrDefaultAsync(a => a.Id == request.Id && !a.IsDeleted, cancellationToken);

                if (payable == null)
                    return Result.Fail("Accounts Payable not found.");

                var payments = await context.Payments
                    .Where(p => p.AccountsPayableId == request.Id && !p.IsDeleted)
                    .OrderByDescending(r => r.PaymentDate)
                    .Select(p => new PaymentDto
                    {
                        Id = p.Id,
                        AccountsPayableId = p.Id,
                        PaymentNumber = p.PaymentNumber,
                        PaymentDate = p.PaymentDate,
                        AmountPaid = p.AmountPaid,
                        PaymentMethodId = p.PaymentMethodId,
                        Reference = p.Reference,
                        CurrencyId = p.CurrencyId,
                        ExchangeRate = p.ExchangeRate,
                        AmountInBaseCurrency = p.AmountInBaseCurrency
                        
                    }).ToListAsync(cancellationToken);

                var dto = new AccountsPayableDto
                {
                    Id = payable.Id,
                    InvoiceNumber = payable.InvoiceNumber,
                    ChartOfAccountId = payable.ChartOfAccountId,
                    ChartOfAccountName = payable.ChartOfAccount.AccountName,
                    //PurchaseOrderId = payable.PurchaseOrderId,
                    CurrencyId = payable.CurrencyId,
                    VendorId = payable.VendorId,
                    InvoiceDate = payable.InvoiceDate,
                    DueDate = payable.DueDate,
                    InvoiceAmount = payable.InvoiceAmount,
                    PaidAmount = payable.PaidAmount,
                    BalanceAmount = payable.BalanceAmount,
                    Status = payable.Status,
                    BranchId = payable.BranchId,
                    VendorName = payable.Vendor!.Name,
                    CurrencyRate = payable.CurrencyRate,
                    Attachment = payable.Attachment,
                    Description = payable.Description,
                    Reference = payable.Reference,
                    Payments = payments

                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Accounts Payable: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Overdue Payables
    public class GetOverduePayablesQuery : IRequest<Result>
    {
        public int? BranchId { get; set; }
    }

    public class GetOverduePayablesQueryHandler(IAccountingRepository accountingRepository) : IRequestHandler<GetOverduePayablesQuery, Result>
    {
        public async Task<Result> Handle(GetOverduePayablesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var overduePayables = await accountingRepository.GetOverduePayablesAsync(request.BranchId, cancellationToken);

                var dtos = overduePayables
                    .Select(a => new AccountsPayableDto
                    {
                        Id = a.Id,
                        InvoiceNumber = a.InvoiceNumber,
                        VendorId = a.VendorId,
                        InvoiceDate = a.InvoiceDate,
                        DueDate = a.DueDate,
                        InvoiceAmount = a.InvoiceAmount,
                        PaidAmount = a.PaidAmount,
                        BalanceAmount = a.BalanceAmount,
                        Status = a.Status,
                        BranchId = a.BranchId
                    })
                    .ToList();

                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Overdue Payables: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Accounts Payable Aging
    public class GetAccountsPayableAgingQuery : IRequest<Result>
    {
        public DateTime AsOfDate { get; set; } = DateTime.UtcNow;
        public int? BranchId { get; set; }
    }

    public class GetAccountsPayableAgingQueryHandler(ERP_DbContext context) : IRequestHandler<GetAccountsPayableAgingQuery, Result>
    {
        public async Task<Result> Handle(GetAccountsPayableAgingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.AccountsPayables.Where(a => !a.IsDeleted && a.BalanceAmount > 0);

                if (request.BranchId.HasValue)
                    query = query.Where(a => a.BranchId == request.BranchId);

                var payables = await query.ToListAsync(cancellationToken);

                var agingBuckets = new
                {
                    current = payables.Where(a => a.DueDate > request.AsOfDate).Sum(a => a.BalanceAmount),
                    days0to30 = payables.Where(a => a.DueDate <= request.AsOfDate && a.DueDate > request.AsOfDate.AddDays(-30)).Sum(a => a.BalanceAmount),
                    days30to60 = payables.Where(a => a.DueDate <= request.AsOfDate.AddDays(-30) && a.DueDate > request.AsOfDate.AddDays(-60)).Sum(a => a.BalanceAmount),
                    days60to90 = payables.Where(a => a.DueDate <= request.AsOfDate.AddDays(-60) && a.DueDate > request.AsOfDate.AddDays(-90)).Sum(a => a.BalanceAmount),
                    days90plus = payables.Where(a => a.DueDate <= request.AsOfDate.AddDays(-90)).Sum(a => a.BalanceAmount)
                };

                var total = agingBuckets.current + agingBuckets.days0to30 + agingBuckets.days30to60 + agingBuckets.days60to90 + agingBuckets.days90plus;

                var result = new
                {
                    asOfDate = request.AsOfDate,
                    agingBuckets,
                    totalOutstanding = total,
                    details = payables.Select(a =>
                    {
                        var daysOverdue = (request.AsOfDate - a.DueDate).Days;
                        return new
                        {
                            a.Id,
                            a.InvoiceNumber,
                            a.VendorId,
                            a.DueDate,
                            a.BalanceAmount,
                            daysOverdue,
                            a.Status
                        };
                    }).ToList()
                };

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Aging Report: {ex.Message}");
            }
        }
    }
    #endregion
}
