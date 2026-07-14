using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Journal Entry
    public class CreateJournalEntryCommand : IRequest<Result>
    {
        public CreateJournalEntryDto Dto { get; set; } = null!;
    }

    public class CreateJournalEntryCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateJournalEntryCommand, Result>
    {
        public async Task<Result> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.Dto.Lines.Any())
                    return Result.Fail("Journal entry must have at least one line.");

                var totalDebits = request.Dto.Lines.Sum(l => l.DebitAmount);
                var totalCredits = request.Dto.Lines.Sum(l => l.CreditAmount);

                if (Math.Abs(totalDebits - totalCredits) > 0.01m)
                    return Result.Fail("Journal entry is not balanced. Total debits must equal total credits.");

                var entryNumber = GenerateEntryNumber();
                var journalEntry = new JournalEntry
                {
                    EntryNumber = entryNumber,
                    EntryDate = request.Dto.EntryDate,
                    Description = request.Dto.Description,
                    Status = JournalEntryStatus.Draft,
                    ReferenceNumber = request.Dto.ReferenceNumber,
                    ReferenceType = request.Dto.ReferenceType,
                    BranchId = request.Dto.BranchId,
                    Attachment = request.Dto.Attachment,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                foreach (var lineDto in request.Dto.Lines)
                {
                    var account = await context.ChartOfAccounts
                        .FirstOrDefaultAsync(c => c.Id == lineDto.ChartOfAccountId && c.IsActive && !c.IsDeleted, cancellationToken);

                    if (account == null)
                        return Result.Fail($"Account {lineDto.ChartOfAccountId} not found or inactive.");

                    var line = new JournalEntryLine
                    {
                        ChartOfAccountId = lineDto.ChartOfAccountId,
                        Description = lineDto.Description,
                        DebitAmount = lineDto.DebitAmount,
                        CreditAmount = lineDto.CreditAmount,
                        CurrencyId = lineDto.CurrencyId,
                        Status = JournalEntryStatus.Draft,
                        ExchangeRate = lineDto.ExchangeRate,
                        AmountInBaseCurrency = (lineDto.DebitAmount > 0 ? lineDto.DebitAmount : lineDto.CreditAmount) * lineDto.ExchangeRate,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    };

                    journalEntry.JournalEntryLines.Add(line);
                }

                context.JournalEntries.Add(journalEntry);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(journalEntry.Id, $"Journal entry {entryNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Journal Entry: {ex.Message}");
            }
        }

        private string GenerateEntryNumber()
        {
            return $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Post Journal Entry
    public class PostJournalEntryCommand : IRequest<Result>
    {
        public int JournalEntryId { get; set; }
    }

    public class PostJournalEntryCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<PostJournalEntryCommand, Result>
    {
        public async Task<Result> Handle(PostJournalEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journalEntry = await context.JournalEntries
                    .Include(j => j.JournalEntryLines)
                    .FirstOrDefaultAsync(j => j.Id == request.JournalEntryId && !j.IsDeleted, cancellationToken);

                if (journalEntry == null)
                    return Result.Fail("Journal entry not found.");

                if (journalEntry.Status == JournalEntryStatus.Posted)
                    return Result.Fail("Journal entry is already posted.");

                var executionStrategy = context.Database.CreateExecutionStrategy();

                await executionStrategy.ExecuteAsync(async () =>
                {
                    journalEntry.Status = JournalEntryStatus.Posted;
                    journalEntry.ApprovedBy = loggedInUser.Id;
                    journalEntry.ApprovedDate = DateTime.UtcNow;
                    journalEntry.ModifiedBy = loggedInUser.Id;
                    journalEntry.ModifiedOn = DateTime.UtcNow;

                    foreach (var line in journalEntry.JournalEntryLines)
                    {
                        var ledgerEntry = new GeneralLedger
                        {
                            ChartOfAccountId = line.ChartOfAccountId,
                            JournalEntryId = journalEntry.Id,
                            BranchId = journalEntry.BranchId,
                            TransactionDate = journalEntry.EntryDate,
                            Description = line.Description,
                            DebitAmount = line.DebitAmount,
                            CreditAmount = line.CreditAmount,
                            Balance = line.DebitAmount - line.CreditAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.GeneralLedgers.Add(ledgerEntry);
                        line.Status = JournalEntryStatus.Posted;
                        context.Update(line);
                    }

                    context.JournalEntries.Update(journalEntry);
                    await context.SaveChangesAsync(cancellationToken);
                });

                return Result.Success("Journal entry posted successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error posting Journal Entry: {ex.Message}");
            }
        }
    }
    #endregion

    #region Void Journal Entry
    public class VoidJournalEntryCommand : IRequest<Result>
    {
        public int JournalEntryId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class VoidJournalEntryCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<VoidJournalEntryCommand, Result>
    {
        public async Task<Result> Handle(VoidJournalEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journalEntry = await context.JournalEntries
                    .FirstOrDefaultAsync(j => j.Id == request.JournalEntryId && !j.IsDeleted, cancellationToken);

                if (journalEntry == null)
                    return Result.Fail("Journal entry not found.");

                if (journalEntry.Status == JournalEntryStatus.Voided)
                    return Result.Fail("Journal entry is already voided.");

                var executionStrategy = context.Database.CreateExecutionStrategy();

                await executionStrategy.ExecuteAsync(async () =>
                {
                    journalEntry.Status = JournalEntryStatus.Voided;
                    journalEntry.ModifiedBy = loggedInUser.Id;
                    journalEntry.ModifiedOn = DateTime.UtcNow;
                    journalEntry.Remarks = request.Reason;

                    var ledgerEntries = await context.GeneralLedgers
                        .Where(g => g.JournalEntryId == request.JournalEntryId && !g.IsDeleted)
                        .ToListAsync(cancellationToken);

                    foreach (var entry in ledgerEntries)
                    {
                        entry.IsDeleted = true;
                        entry.ModifiedBy = loggedInUser.Id;
                        entry.ModifiedOn = DateTime.UtcNow;
                    }
                    var lines = context.JournalEntryLines.Where(x => x.JournalEntryId == request.JournalEntryId && !x.IsDeleted);
                    foreach (var line in lines)
                    {
                        line.Status = JournalEntryStatus.Voided;
                    }
                    context.JournalEntryLines.UpdateRange(lines);
                    context.JournalEntries.Update(journalEntry);
                    context.GeneralLedgers.UpdateRange(ledgerEntries);
                    await context.SaveChangesAsync(cancellationToken);
                });

                return Result.Success("Journal entry voided successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error voiding Journal Entry: {ex.Message}");
            }
        }
    }
    #endregion
}
