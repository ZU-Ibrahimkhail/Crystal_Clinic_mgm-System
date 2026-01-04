using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get Bank Statement Import
    public class GetBankStatementImportQuery : IRequest<Result>
    {
        public int ImportId { get; set; }
    }

    public class GetBankStatementImportQueryHandler(ERP_DbContext context) : IRequestHandler<GetBankStatementImportQuery, Result>
    {
        public async Task<Result> Handle(GetBankStatementImportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var import = await context.BankStatementImports
                    .Include(b => b.Lines)
                    .FirstOrDefaultAsync(b => b.Id == request.ImportId && !b.IsDeleted, cancellationToken);

                if (import == null)
                    return Result.Fail("Bank statement import not found");

                var dto = new BankStatementImportDto
                {
                    Id = import.Id,
                    FileName = import.FileName,
                    BankAccountId = import.BankAccountId,
                    StatementPeriodStart = import.StatementPeriodStart,
                    StatementPeriodEnd = import.StatementPeriodEnd,
                    OpeningBalance = import.OpeningBalance,
                    ClosingBalance = import.ClosingBalance,
                    TotalTransactions = import.TotalTransactions,
                    FileFormat = import.FileFormat,
                    ImportDate = import.ImportDate,
                    Status = import.Status,
                    Lines = import.Lines.Select(l => new BankStatementLineDto
                    {
                        Id = l.Id,
                        TransactionDate = l.TransactionDate,
                        ReferenceNumber = l.ReferenceNumber,
                        Description = l.Description,
                        Amount = l.Amount,
                        TransactionType = l.TransactionType,
                        RunningBalance = l.RunningBalance,
                        BankCode = l.BankCode,
                        IsMatched = l.IsMatched
                    }).ToList()
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving bank statement import: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get All Bank Statement Imports
    public class GetAllBankStatementImportsQuery : IRequest<Result>
    {
        public int? BankAccountId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllBankStatementImportsQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllBankStatementImportsQuery, Result>
    {
        public async Task<Result> Handle(GetAllBankStatementImportsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.BankStatementImports
                    .Include(b => b.Lines)
                    .Where(b => !b.IsDeleted);

                if (request.BankAccountId.HasValue)
                    query = query.Where(b => b.BankAccountId == request.BankAccountId);

                if (request.FromDate.HasValue)
                    query = query.Where(b => b.ImportDate >= request.FromDate);

                if (request.ToDate.HasValue)
                    query = query.Where(b => b.ImportDate <= request.ToDate);

                var imports = await query
                    .OrderByDescending(b => b.ImportDate)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = imports.Select(b => new BankStatementImportDto
                {
                    Id = b.Id,
                    FileName = b.FileName,
                    BankAccountId = b.BankAccountId,
                    StatementPeriodStart = b.StatementPeriodStart,
                    StatementPeriodEnd = b.StatementPeriodEnd,
                    OpeningBalance = b.OpeningBalance,
                    ClosingBalance = b.ClosingBalance,
                    TotalTransactions = b.TotalTransactions,
                    FileFormat = b.FileFormat,
                    ImportDate = b.ImportDate,
                    Status = b.Status,
                    Lines = b.Lines.Select(l => new BankStatementLineDto
                    {
                        Id = l.Id,
                        TransactionDate = l.TransactionDate,
                        ReferenceNumber = l.ReferenceNumber,
                        Description = l.Description,
                        Amount = l.Amount,
                        TransactionType = l.TransactionType,
                        RunningBalance = l.RunningBalance,
                        BankCode = l.BankCode,
                        IsMatched = l.IsMatched
                    }).ToList()
                }).ToList();

                return Result.Success(new { data = dtos, page = request.Page, pageSize = request.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving bank statement imports: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Bank Reconciliation Summary
    public class GetBankReconciliationSummaryQuery : IRequest<Result>
    {
        public int BankStatementImportId { get; set; }
    }

    public class GetBankReconciliationSummaryQueryHandler(ERP_DbContext context) : IRequestHandler<GetBankReconciliationSummaryQuery, Result>
    {
        public async Task<Result> Handle(GetBankReconciliationSummaryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var import = await context.BankStatementImports
                    .Include(b => b.Lines)
                    .Include(b => b.Matches)
                    .FirstOrDefaultAsync(b => b.Id == request.BankStatementImportId && !b.IsDeleted, cancellationToken);

                if (import == null)
                    return Result.Fail("Bank statement import not found");

                var matchedCount = import.Lines.Count(l => l.IsMatched);
                var unmatchedCount = import.Lines.Count(l => !l.IsMatched);
                var unmatchedAmount = import.Lines.Where(l => !l.IsMatched).Sum(l => Math.Abs(l.Amount));

                var glBalance = await context.GeneralLedgers
                    .Where(g => !g.IsDeleted && g.TransactionDate <= import.StatementPeriodEnd)
                    .SumAsync(g => g.DebitAmount - g.CreditAmount, cancellationToken);

                var summary = new BankReconciliationSummaryDto
                {
                    ImportId = import.Id,
                    StatementClosingBalance = import.ClosingBalance,
                    BookBalance = glBalance,
                    Difference = import.ClosingBalance - glBalance,
                    MatchedCount = matchedCount,
                    UnmatchedCount = unmatchedCount,
                    AsOfDate = import.StatementPeriodEnd
                };

                return Result.Success(summary);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving reconciliation summary: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Unmatched Bank Transactions
    public class GetUnmatchedBankTransactionsQuery : IRequest<Result>
    {
        public int BankStatementImportId { get; set; }
    }

    public class GetUnmatchedBankTransactionsQueryHandler(ERP_DbContext context) : IRequestHandler<GetUnmatchedBankTransactionsQuery, Result>
    {
        public async Task<Result> Handle(GetUnmatchedBankTransactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lines = await context.BankStatementLines
                    .Where(l => l.BankStatementImportId == request.BankStatementImportId && !l.IsMatched && !l.IsDeleted)
                    .Select(l => new BankStatementLineDto
                    {
                        Id = l.Id,
                        TransactionDate = l.TransactionDate,
                        ReferenceNumber = l.ReferenceNumber,
                        Description = l.Description,
                        Amount = l.Amount,
                        TransactionType = l.TransactionType,
                        RunningBalance = l.RunningBalance,
                        BankCode = l.BankCode,
                        IsMatched = false
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(lines);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving unmatched transactions: {ex.Message}");
            }
        }
    }
    #endregion
}
