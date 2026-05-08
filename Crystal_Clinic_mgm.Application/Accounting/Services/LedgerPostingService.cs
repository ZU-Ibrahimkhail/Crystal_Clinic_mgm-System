using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public static class LedgerPostingService
    {
        public static async Task PostToGeneralLedgerAsync(
            ERP_DbContext context,
            JournalEntry journalEntry,
            CancellationToken cancellationToken = default)
        {
            foreach (var line in journalEntry.JournalEntryLines)
            {
                var account = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(a => a.Id == line.ChartOfAccountId && !a.IsDeleted, cancellationToken);

                if (account == null)
                    throw new InvalidOperationException($"Chart of Account with ID {line.ChartOfAccountId} not found.");

                var lastBalance = await context.GeneralLedgers
                    .Where(g => g.ChartOfAccountId == line.ChartOfAccountId && !g.IsDeleted)
                    .OrderByDescending(g => g.Id)
                    .Select(g => g.Balance)
                    .FirstOrDefaultAsync(cancellationToken);

                decimal runningBalance;
                if (account.NormalBalance == NormalBalanceType.Debit)
                    runningBalance = lastBalance + line.DebitAmount - line.CreditAmount;
                else
                    runningBalance = lastBalance + line.CreditAmount - line.DebitAmount;

                var glEntry = new GeneralLedger
                {
                    ChartOfAccountId = line.ChartOfAccountId,
                    JournalEntryId = journalEntry.Id,
                    BranchId = journalEntry.BranchId,
                    TransactionDate = journalEntry.EntryDate,
                    Description = line.Description,
                    DebitAmount = line.DebitAmount,
                    CreditAmount = line.CreditAmount,
                    Balance = runningBalance,
                    CreatedBy = journalEntry.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                context.GeneralLedgers.Add(glEntry);
            }
        }

        public static string GenerateJournalEntryNumber()
        {
            return $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}
