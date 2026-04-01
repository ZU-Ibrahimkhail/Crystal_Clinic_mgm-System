using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Repositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Accounts Receivable
    public class GetAllAccountsReceivableQuery : IRequest<Result>
    {
        public ARStatus? Status { get; set; }
        public int? CustomerId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllAccountsReceivableQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllAccountsReceivableQuery, Result>
    {
        public async Task<Result> Handle(GetAllAccountsReceivableQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.AccountsReceivables.Include(x => x.Customer).Where(a => !a.IsDeleted);

                if (request.Status.HasValue)
                    query = query.Where(a => a.Status == request.Status);

                if (request.CustomerId.HasValue)
                    query = query.Where(a => a.CustomerId == request.CustomerId);

                var totalCount = await query.CountAsync(cancellationToken);

                var receivables = await query
                    .OrderByDescending(a => a.InvoiceDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(a => new AccountsReceivableDto
                    {
                        Id = a.Id,
                        InvoiceNumber = a.InvoiceNumber,
                        CustomerId = a.CustomerId,
                        CustomerName = a.Customer!.name,
                        CurrencyRate = a.CurrencyRate,
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
                    receivables,
                    totalCount,
                    pageNumber = request.PageNumber,
                    pageSize = request.PageSize
                };

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Accounts Receivable: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Accounts Receivable By Id
    public class GetAccountsReceivableByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetAccountsReceivableByIdQueryHandler(ERP_DbContext context) : IRequestHandler<GetAccountsReceivableByIdQuery, Result>
    {
        public async Task<Result> Handle(GetAccountsReceivableByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var receivable = await context.AccountsReceivables
                    .Include(x=>x.Currency)
                    .Include(x=>x.Customer)
                    .FirstOrDefaultAsync(a => a.Id == request.Id && !a.IsDeleted, cancellationToken);

                if (receivable == null)
                    return Result.Fail("Accounts Receivable not found.");

                // Get associated receipts
                var receipts = await context.Receipts
                    .Where(r => r.AccountsReceivableId == request.Id && !r.IsDeleted)
                    .Include(r => r.OriginalReceipt)
                    .OrderByDescending(r => r.ReceiptDate)
                    .Select(r => new ReceiptDto
                    {
                        Id = r.Id,
                        AccountsReceivableId = r.AccountsReceivableId,
                        ReceiptNumber = r.ReceiptNumber,
                        ReceiptDate = r.ReceiptDate,
                        TransactionType = r.TransactionType,
                        Amount = r.Amount,
                        PaymentMethodId = r.PaymentMethodId,
                        Reference = r.Reference,
                        OriginalReceiptId = r.OriginalReceiptId
                    })
                    .ToListAsync(cancellationToken);

                var dto = new AccountsReceivableDto
                {
                    Id = receivable.Id,
                    InvoiceNumber = receivable.InvoiceNumber,
                    CustomerId = receivable.CustomerId,
                    CustomerName = receivable.Customer.name,
                    InvoiceDate = receivable.InvoiceDate,
                    DueDate = receivable.DueDate,
                    InvoiceAmount = receivable.InvoiceAmount,
                    PaidAmount = receivable.PaidAmount,
                    BalanceAmount = receivable.BalanceAmount,
                    Status = receivable.Status,
                    BranchId = receivable.BranchId,
                    CurrencyRate = receivable.CurrencyRate,
                    Attachment = receivable.Attachment,
                    Description = receivable.Description,
                    Reference = receivable.Reference,
                    VisitId = receivable.VisitId,
                    ChartOfAccountId = receivable.ChartOfAccountId,
                    CurrencyId = receivable.CurrencyId,
                    Receipts = receipts,
                    
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Accounts Receivable: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Receipts by AR
    public class GetReceiptsByARQuery : IRequest<Result>
    {
        public int AccountsReceivableId { get; set; }
    }

    public class GetReceiptsByARQueryHandler(ERP_DbContext context) : IRequestHandler<GetReceiptsByARQuery, Result>
    {
        public async Task<Result> Handle(GetReceiptsByARQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var receipts = await context.Receipts
                    .Where(r => r.AccountsReceivableId == request.AccountsReceivableId && !r.IsDeleted)
                    .Include(r => r.OriginalReceipt)
                    .OrderByDescending(r => r.ReceiptDate)
                    .Select(r => new ReceiptDto
                    {
                        Id = r.Id,
                        AccountsReceivableId = r.AccountsReceivableId,
                        ReceiptNumber = r.ReceiptNumber,
                        ReceiptDate = r.ReceiptDate,
                        TransactionType = r.TransactionType,
                        Amount = r.Amount,
                        PaymentMethodId = r.PaymentMethodId,
                        Reference = r.Reference,
                        OriginalReceiptId = r.OriginalReceiptId
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(receipts);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving receipts: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Overdue Receivables
    public class GetOverdueReceivablesQuery : IRequest<Result>
    {
        public int? BranchId { get; set; }
    }

    public class GetOverdueReceivablesQueryHandler(IAccountingRepository accountingRepository) : IRequestHandler<GetOverdueReceivablesQuery, Result>
    {
        public async Task<Result> Handle(GetOverdueReceivablesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var overdueReceivables = await accountingRepository.GetOverdueReceivablesAsync(request.BranchId, cancellationToken);

                var dtos = overdueReceivables
                    .Select(a => new AccountsReceivableDto
                    {
                        Id = a.Id,
                        InvoiceNumber = a.InvoiceNumber,
                        CustomerId = a.CustomerId,
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
                return Result.Fail($"Error retrieving Overdue Receivables: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Accounts Receivable Aging
    public class GetAccountsReceivableAgingQuery : IRequest<Result>
    {
        public DateTime AsOfDate { get; set; } = DateTime.UtcNow;
        public int? BranchId { get; set; }
    }

    public class GetAccountsReceivableAgingQueryHandler(ERP_DbContext context) : IRequestHandler<GetAccountsReceivableAgingQuery, Result>
    {
        public async Task<Result> Handle(GetAccountsReceivableAgingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.AccountsReceivables.Where(a => !a.IsDeleted && a.BalanceAmount > 0);

                if (request.BranchId.HasValue)
                    query = query.Where(a => a.BranchId == request.BranchId);

                var receivables = await query.ToListAsync(cancellationToken);

                var agingBuckets = new
                {
                    current = receivables.Where(a => a.DueDate > request.AsOfDate).Sum(a => a.BalanceAmount),
                    days0to30 = receivables.Where(a => a.DueDate <= request.AsOfDate && a.DueDate > request.AsOfDate.AddDays(-30)).Sum(a => a.BalanceAmount),
                    days30to60 = receivables.Where(a => a.DueDate <= request.AsOfDate.AddDays(-30) && a.DueDate > request.AsOfDate.AddDays(-60)).Sum(a => a.BalanceAmount),
                    days60to90 = receivables.Where(a => a.DueDate <= request.AsOfDate.AddDays(-60) && a.DueDate > request.AsOfDate.AddDays(-90)).Sum(a => a.BalanceAmount),
                    days90plus = receivables.Where(a => a.DueDate <= request.AsOfDate.AddDays(-90)).Sum(a => a.BalanceAmount)
                };

                var total = agingBuckets.current + agingBuckets.days0to30 + agingBuckets.days30to60 + agingBuckets.days60to90 + agingBuckets.days90plus;

                var result = new
                {
                    asOfDate = request.AsOfDate,
                    agingBuckets,
                    totalOutstanding = total,
                    details = receivables.Select(a =>
                    {
                        var daysOverdue = (request.AsOfDate - a.DueDate).Days;
                        return new
                        {
                            a.Id,
                            a.InvoiceNumber,
                            a.CustomerId,
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
