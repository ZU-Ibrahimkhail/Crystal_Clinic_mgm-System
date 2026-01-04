using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Repositories
{
    public class AccountingRepository : IAccountingRepository
    {
        private readonly ERP_DbContext _context;

        public AccountingRepository(ERP_DbContext context)
        {
            _context = context;
        }

        #region Chart of Accounts
        public async Task<ChartOfAccounts?> GetAccountByCodeAsync(string accountCode, CancellationToken cancellationToken)
        {
            return await _context.ChartOfAccounts
                .FirstOrDefaultAsync(c => c.AccountCode == accountCode && !c.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<ChartOfAccounts>> GetAccountsByTypeAsync(AccountType accountType, CancellationToken cancellationToken)
        {
            return await _context.ChartOfAccounts
                .Where(c => c.AccountType == accountType && c.IsActive && !c.IsDeleted)
                .OrderBy(c => c.AccountCode)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ChartOfAccounts>> GetAccountsByCategoryAsync(AccountCategory accountCategory, CancellationToken cancellationToken)
        {
            return await _context.ChartOfAccounts
                .Where(c => c.AccountCategory == accountCategory && c.IsActive && !c.IsDeleted)
                .OrderBy(c => c.AccountCode)
                .ToListAsync(cancellationToken);
        }

        public async Task<ChartOfAccounts?> GetAccountWithChildrenAsync(int accountId, CancellationToken cancellationToken)
        {
            return await _context.ChartOfAccounts
                .Include(c => c.ChildAccounts)
                .FirstOrDefaultAsync(c => c.Id == accountId && !c.IsDeleted, cancellationToken);
        }
        #endregion

        #region Journal Entries
        public async Task<JournalEntry?> GetJournalEntryWithLinesAsync(int journalEntryId, CancellationToken cancellationToken)
        {
            return await _context.JournalEntries
                .Include(j => j.JournalEntryLines)
                .FirstOrDefaultAsync(j => j.Id == journalEntryId && !j.IsDeleted, cancellationToken);
        }

        public async Task<decimal> GetTotalDebitsAsync(int journalEntryId, CancellationToken cancellationToken)
        {
            return await _context.JournalEntryLines
                .Where(l => l.JournalEntryId == journalEntryId && !l.IsDeleted)
                .SumAsync(l => l.DebitAmount, cancellationToken);
        }

        public async Task<decimal> GetTotalCreditsAsync(int journalEntryId, CancellationToken cancellationToken)
        {
            return await _context.JournalEntryLines
                .Where(l => l.JournalEntryId == journalEntryId && !l.IsDeleted)
                .SumAsync(l => l.CreditAmount, cancellationToken);
        }

        public async Task<bool> JournalEntryExistsAsync(string entryNumber, CancellationToken cancellationToken)
        {
            return await _context.JournalEntries
                .AnyAsync(j => j.EntryNumber == entryNumber && !j.IsDeleted, cancellationToken);
        }
        #endregion

        #region General Ledger
        public async Task<IEnumerable<GeneralLedger>> GetAccountLedgerAsync(int chartOfAccountId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            return await _context.GeneralLedgers
                .Where(g => g.ChartOfAccountId == chartOfAccountId && g.TransactionDate >= fromDate && g.TransactionDate <= toDate && !g.IsDeleted)
                .OrderBy(g => g.TransactionDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetAccountBalanceAsync(int chartOfAccountId, DateTime asOfDate, CancellationToken cancellationToken)
        {
            var debit = await _context.GeneralLedgers
                .Where(g => g.ChartOfAccountId == chartOfAccountId && g.TransactionDate <= asOfDate && !g.IsDeleted)
                .SumAsync(g => g.DebitAmount, cancellationToken);

            var credit = await _context.GeneralLedgers
                .Where(g => g.ChartOfAccountId == chartOfAccountId && g.TransactionDate <= asOfDate && !g.IsDeleted)
                .SumAsync(g => g.CreditAmount, cancellationToken);

            var account = await _context.ChartOfAccounts.FirstOrDefaultAsync(c => c.Id == chartOfAccountId, cancellationToken);
            if (account?.NormalBalance == NormalBalanceType.Debit)
                return debit - credit;
            else
                return credit - debit;
        }

        public async Task<IEnumerable<GeneralLedger>> GetTrialBalanceAsync(int? branchId, DateTime asOfDate, CancellationToken cancellationToken)
        {
            var query = _context.GeneralLedgers
                .Where(g => g.TransactionDate <= asOfDate && !g.IsDeleted);

            if (branchId.HasValue)
                query = query.Where(g => g.BranchId == branchId);

            return await query
                .OrderBy(g => g.ChartOfAccountId)
                .ThenBy(g => g.TransactionDate)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region Accounts Receivable
        public async Task<IEnumerable<AccountsReceivable>> GetOverdueReceivablesAsync(int? branchId, CancellationToken cancellationToken)
        {
            var query = _context.AccountsReceivables
                .Where(a => a.DueDate < DateTime.Now && a.BalanceAmount > 0 && !a.IsDeleted);

            if (branchId.HasValue)
                query = query.Where(a => a.BranchId == branchId);

            return await query.OrderBy(a => a.DueDate).ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalReceivablesByCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.AccountsReceivables
                .Where(a => a.CustomerId == customerId && a.BalanceAmount > 0 && !a.IsDeleted)
                .SumAsync(a => a.BalanceAmount, cancellationToken);
        }

        public async Task<IEnumerable<AccountsReceivable>> GetReceivablesByStatusAsync(ARStatus status, CancellationToken cancellationToken)
        {
            return await _context.AccountsReceivables
                .Where(a => a.Status == status && !a.IsDeleted)
                .OrderByDescending(a => a.InvoiceDate)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region Accounts Payable
        public async Task<IEnumerable<AccountsPayable>> GetOverduePayablesAsync(int? branchId, CancellationToken cancellationToken)
        {
            var query = _context.AccountsPayables
                .Where(a => a.DueDate < DateTime.Now && a.BalanceAmount > 0 && !a.IsDeleted);

            if (branchId.HasValue)
                query = query.Where(a => a.BranchId == branchId);

            return await query.OrderBy(a => a.DueDate).ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalPayablesByVendorAsync(int vendorId, CancellationToken cancellationToken)
        {
            return await _context.AccountsPayables
                .Where(a => a.VendorId == vendorId && a.BalanceAmount > 0 && !a.IsDeleted)
                .SumAsync(a => a.BalanceAmount, cancellationToken);
        }

        public async Task<IEnumerable<AccountsPayable>> GetPayablesByStatusAsync(APStatus status, CancellationToken cancellationToken)
        {
            return await _context.AccountsPayables
                .Where(a => a.Status == status && !a.IsDeleted)
                .OrderByDescending(a => a.InvoiceDate)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region Budget
        public async Task<Budget?> GetBudgetWithLinesAsync(int budgetId, CancellationToken cancellationToken)
        {
            return await _context.Budgets
                .Include(b => b.BudgetLines)
                .FirstOrDefaultAsync(b => b.Id == budgetId && !b.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<BudgetLine>> GetBudgetVarianceAsync(int budgetId, CancellationToken cancellationToken)
        {
            return await _context.BudgetLines
                .Where(b => b.BudgetId == budgetId && !b.IsDeleted)
                .OrderBy(b => b.ChartOfAccountId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Budget?> GetActiveBudgetAsync(int fiscalYear, int? branchId, CancellationToken cancellationToken)
        {
            var query = _context.Budgets
                .Where(b => b.FiscalYear == fiscalYear && b.Status == BudgetStatus.Active && !b.IsDeleted);

            if (branchId.HasValue)
                query = query.Where(b => b.BranchId == branchId);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region Expenses
        public async Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime fromDate, DateTime toDate, int? branchId, CancellationToken cancellationToken)
        {
            var query = _context.Expenses
                .Where(e => e.ExpenseDate >= fromDate && e.ExpenseDate <= toDate && !e.IsDeleted);

            if (branchId.HasValue)
                query = query.Where(e => e.BranchId == branchId);

            return await query.SumAsync(e => e.Amount, cancellationToken);
        }

        public async Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId, CancellationToken cancellationToken)
        {
            return await _context.Expenses
                .Where(e => e.CategoryId == categoryId && !e.IsDeleted)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
