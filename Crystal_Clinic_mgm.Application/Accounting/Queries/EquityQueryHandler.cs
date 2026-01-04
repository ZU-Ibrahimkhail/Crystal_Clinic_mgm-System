using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Shareholders
    public class GetAllShareholdersQuery : IRequest<Result>
    {
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetAllShareholdersQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllShareholdersQuery, Result>
    {
        public async Task<Result> Handle(GetAllShareholdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.Shareholders.Where(s => !s.IsDeleted);

                if (request.IsActive.HasValue)
                    query = query.Where(s => s.IsActive == request.IsActive);

                var totalCount = await query.CountAsync(cancellationToken);

                var shareholders = await query
                    .OrderByDescending(s => s.OwnershipPercentage)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(s => new ShareholderDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        OwnershipPercentage = s.OwnershipPercentage,
                        TotalInvestment = s.TotalInvestment,
                        TotalDrawings = s.TotalDrawings,
                        NetEquity = s.TotalInvestment - s.TotalDrawings,
                        ContactInfo = s.ContactInfo,
                        Email = s.Email,
                        IsActive = s.IsActive
                    })
                    .ToListAsync(cancellationToken);

                var result = new { shareholders, totalCount, pageNumber = request.PageNumber, pageSize = request.PageSize };
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Shareholders: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Shareholder By Id
    public class GetShareholderByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetShareholderByIdQueryHandler(ERP_DbContext context) : IRequestHandler<GetShareholderByIdQuery, Result>
    {
        public async Task<Result> Handle(GetShareholderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var shareholder = await context.Shareholders
                    .Include(s => s.Transactions)
                    .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken);

                if (shareholder == null)
                    return Result.Fail("Shareholder not found.");

                var dto = new ShareholderEquitySummaryDto
                {
                    ShareholderId = shareholder.Id,
                    ShareholderName = shareholder.Name,
                    OwnershipPercentage = shareholder.OwnershipPercentage,
                    TotalInvestment = shareholder.TotalInvestment,
                    TotalDrawings = shareholder.TotalDrawings,
                    NetEquity = shareholder.TotalInvestment - shareholder.TotalDrawings,
                    Transactions = shareholder.Transactions
                        .Where(t => !t.IsDeleted)
                        .OrderByDescending(t => t.TransactionDate)
                        .Select(t => new EquityTransactionDto
                        {
                            Id = t.Id,
                            ShareholderId = t.ShareholderId,
                            ShareholderName = shareholder.Name,
                            Type = t.Type,
                            Amount = t.Amount,
                            TransactionDate = t.TransactionDate,
                            Description = t.Description,
                            Reference = t.Reference
                        })
                        .ToList()
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Shareholder: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Equity Transactions
    public class GetEquityTransactionsQuery : IRequest<Result>
    {
        public int? ShareholderId { get; set; }
        public EquityTransactionType? Type { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetEquityTransactionsQueryHandler(ERP_DbContext context) : IRequestHandler<GetEquityTransactionsQuery, Result>
    {
        public async Task<Result> Handle(GetEquityTransactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.EquityTransactions.Where(t => !t.IsDeleted);

                if (request.ShareholderId.HasValue)
                    query = query.Where(t => t.ShareholderId == request.ShareholderId);

                if (request.Type.HasValue)
                    query = query.Where(t => t.Type == request.Type);

                var totalCount = await query.CountAsync(cancellationToken);

                var transactions = await query
                    .Include(t => t.Shareholder)
                    .OrderByDescending(t => t.TransactionDate)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(t => new EquityTransactionDto
                    {
                        Id = t.Id,
                        ShareholderId = t.ShareholderId,
                        ShareholderName = t.Shareholder.Name,
                        Type = t.Type,
                        Amount = t.Amount,
                        TransactionDate = t.TransactionDate,
                        Description = t.Description,
                        Reference = t.Reference
                    })
                    .ToListAsync(cancellationToken);

                var result = new { transactions, totalCount, pageNumber = request.PageNumber, pageSize = request.PageSize };
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Equity Transactions: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Equity Report
    public class GetEquityReportQuery : IRequest<Result>
    {
        public DateTime? AsOfDate { get; set; }
    }

    public class GetEquityReportQueryHandler(ERP_DbContext context) : IRequestHandler<GetEquityReportQuery, Result>
    {
        public async Task<Result> Handle(GetEquityReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var asOfDate = request.AsOfDate ?? DateTime.UtcNow;

                var shareholders = await context.Shareholders
                    .Where(s => !s.IsDeleted && s.IsActive)
                    .Include(s => s.Transactions)
                    .OrderByDescending(s => s.OwnershipPercentage)
                    .ToListAsync(cancellationToken);

                var shareholderSummaries = shareholders
                    .Select(s => new ShareholderEquitySummaryDto
                    {
                        ShareholderId = s.Id,
                        ShareholderName = s.Name,
                        OwnershipPercentage = s.OwnershipPercentage,
                        TotalInvestment = s.TotalInvestment,
                        TotalDrawings = s.TotalDrawings,
                        NetEquity = s.TotalInvestment - s.TotalDrawings,
                        Transactions = s.Transactions
                            .Where(t => !t.IsDeleted && t.TransactionDate <= asOfDate)
                            .OrderByDescending(t => t.TransactionDate)
                            .Select(t => new EquityTransactionDto
                            {
                                Id = t.Id,
                                ShareholderId = t.ShareholderId,
                                ShareholderName = s.Name,
                                Type = t.Type,
                                Amount = t.Amount,
                                TransactionDate = t.TransactionDate,
                                Description = t.Description,
                                Reference = t.Reference
                            })
                            .ToList()
                    })
                    .ToList();

                var totalEquity = shareholderSummaries.Sum(s => s.NetEquity);

                var report = new EquityReportDto
                {
                    AsOfDate = asOfDate,
                    TotalEquity = totalEquity,
                    Shareholders = shareholderSummaries
                };

                return Result.Success(report);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Equity Report: {ex.Message}");
            }
        }
    }
    #endregion
}
