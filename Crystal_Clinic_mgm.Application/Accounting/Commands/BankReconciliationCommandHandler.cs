using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Upload Bank Statement
    public class UploadBankStatementCommand : IRequest<Result>
    {
        public CreateBankStatementImportDto Dto { get; set; } = null!;
    }

    public class UploadBankStatementCommandHandler(ERP_DbContext context) : IRequestHandler<UploadBankStatementCommand, Result>
    {
        public async Task<Result> Handle(UploadBankStatementCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var import = new BankStatementImport
                {
                    FileName = request.Dto.FileName,
                    BankAccountId = request.Dto.BankAccountId,
                    StatementPeriodStart = request.Dto.StatementPeriodStart,
                    StatementPeriodEnd = request.Dto.StatementPeriodEnd,
                    OpeningBalance = request.Dto.OpeningBalance,
                    ClosingBalance = request.Dto.ClosingBalance,
                    TotalTransactions = request.Dto.Lines.Count,
                    FileFormat = request.Dto.FileFormat,
                    ImportDate = DateTime.UtcNow,
                    Status = BankImportStatus.Processing
                };

                if (request.Dto.Lines.Any())
                {
                    foreach (var lineDto in request.Dto.Lines)
                    {
                        var line = new BankStatementLine
                        {
                            TransactionDate = lineDto.TransactionDate,
                            ReferenceNumber = lineDto.ReferenceNumber,
                            Description = lineDto.Description,
                            Amount = lineDto.Amount,
                            TransactionType = lineDto.TransactionType,
                            RunningBalance = lineDto.RunningBalance,
                            BankCode = lineDto.BankCode,
                            IsMatched = false
                        };
                        import.Lines.Add(line);
                    }
                }

                context.BankStatementImports.Add(import);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = import.Id, status = "Uploaded successfully" });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error uploading bank statement: {ex.Message}");
            }
        }
    }
    #endregion

    #region Auto-Match Bank Transactions
    public class AutoMatchBankTransactionsCommand : IRequest<Result>
    {
        public int BankStatementImportId { get; set; }
        public decimal AmountTolerance { get; set; } = 0.01m;
        public int DateTolerance { get; set; } = 5;
    }

    public class AutoMatchBankTransactionsCommandHandler(ERP_DbContext context) : IRequestHandler<AutoMatchBankTransactionsCommand, Result>
    {
        public async Task<Result> Handle(AutoMatchBankTransactionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var import = await context.BankStatementImports
                    .Include(b => b.Lines)
                    .FirstOrDefaultAsync(b => b.Id == request.BankStatementImportId, cancellationToken);

                if (import == null)
                    return Result.Fail("Bank statement import not found");

                var unmatchedLines = import.Lines.Where(l => !l.IsMatched).ToList();
                var glEntries = await context.GeneralLedgers
                    .Where(g => g.TransactionDate >= import.StatementPeriodStart.AddDays(-request.DateTolerance) &&
                                g.TransactionDate <= import.StatementPeriodEnd.AddDays(request.DateTolerance))
                    .ToListAsync(cancellationToken);

                int matchCount = 0;
                foreach (var line in unmatchedLines)
                {
                    var potentialMatches = glEntries.Where(g =>
                        Math.Abs((g.DebitAmount + g.CreditAmount) - Math.Abs(line.Amount)) <= request.AmountTolerance &&
                        Math.Abs((g.TransactionDate - line.TransactionDate).TotalDays) <= request.DateTolerance)
                        .ToList();

                    if (potentialMatches.Count == 1)
                    {
                        var match = new BankMatch
                        {
                            BankStatementImportId = request.BankStatementImportId,
                            BankStatementLineId = line.Id,
                            GeneralLedgerId = potentialMatches[0].Id,
                            MatchedAmount = Math.Abs(line.Amount),
                            MatchDate = DateTime.UtcNow,
                            Status = BankMatchStatus.Proposed
                        };

                        context.BankMatches.Add(match);
                        line.IsMatched = true;
                        matchCount++;
                    }
                }

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { matchCount, totalUnmatched = unmatchedLines.Count });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error auto-matching transactions: {ex.Message}");
            }
        }
    }
    #endregion

    #region Manual Match Bank Transaction
    public class ManualMatchBankTransactionCommand : IRequest<Result>
    {
        public CreateBankMatchDto Dto { get; set; } = null!;
    }

    public class ManualMatchBankTransactionCommandHandler(ERP_DbContext context) : IRequestHandler<ManualMatchBankTransactionCommand, Result>
    {
        public async Task<Result> Handle(ManualMatchBankTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var line = await context.BankStatementLines
                    .FirstOrDefaultAsync(l => l.Id == request.Dto.BankStatementLineId, cancellationToken);

                if (line == null)
                    return Result.Fail("Bank statement line not found");

                var match = new BankMatch
                {
                    BankStatementImportId = request.Dto.BankStatementImportId,
                    BankStatementLineId = request.Dto.BankStatementLineId,
                    GeneralLedgerId = request.Dto.GeneralLedgerId,
                    JournalEntryId = request.Dto.JournalEntryId,
                    MatchedAmount = request.Dto.MatchedAmount,
                    MatchDate = DateTime.UtcNow,
                    Status = BankMatchStatus.Approved,
                    Notes = request.Dto.Notes
                };

                context.BankMatches.Add(match);
                line.IsMatched = true;
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = match.Id, status = "Matched successfully" });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error matching transaction: {ex.Message}");
            }
        }
    }
    #endregion

    #region Close Bank Reconciliation
    public class CloseBankReconciliationCommand : IRequest<Result>
    {
        public int BankStatementImportId { get; set; }
    }

    public class CloseBankReconciliationCommandHandler(ERP_DbContext context) : IRequestHandler<CloseBankReconciliationCommand, Result>
    {
        public async Task<Result> Handle(CloseBankReconciliationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var import = await context.BankStatementImports
                    .Include(b => b.Lines)
                    .Include(b => b.Matches)
                    .FirstOrDefaultAsync(b => b.Id == request.BankStatementImportId, cancellationToken);

                if (import == null)
                    return Result.Fail("Bank statement import not found");

                var unmatchedLines = import.Lines.Where(l => !l.IsMatched).ToList();

                if (unmatchedLines.Any())
                {
                    return Result.Fail($"Cannot close reconciliation with {unmatchedLines.Count} unmatched transactions. Reconcile all transactions first.");
                }

                import.Status = BankImportStatus.Completed;
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { status = "Reconciliation closed successfully", importId = import.Id });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error closing reconciliation: {ex.Message}");
            }
        }
    }
    #endregion
}
