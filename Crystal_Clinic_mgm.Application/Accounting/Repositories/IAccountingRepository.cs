using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.Accounting.Repositories
{
    public interface IAccountingRepository
    {
        #region Chart of Accounts
        Task<ChartOfAccounts?> GetAccountByCodeAsync(string accountCode, CancellationToken cancellationToken);
        Task<IEnumerable<ChartOfAccounts>> GetAccountsByTypeAsync(AccountType accountType, CancellationToken cancellationToken);
        Task<IEnumerable<ChartOfAccounts>> GetAccountsByCategoryAsync(AccountCategory accountCategory, CancellationToken cancellationToken);
        Task<ChartOfAccounts?> GetAccountWithChildrenAsync(int accountId, CancellationToken cancellationToken);
        #endregion

        #region Journal Entries
        Task<JournalEntry?> GetJournalEntryWithLinesAsync(int journalEntryId, CancellationToken cancellationToken);
        Task<decimal> GetTotalDebitsAsync(int journalEntryId, CancellationToken cancellationToken);
        Task<decimal> GetTotalCreditsAsync(int journalEntryId, CancellationToken cancellationToken);
        Task<bool> JournalEntryExistsAsync(string entryNumber, CancellationToken cancellationToken);
        #endregion

        #region General Ledger
        Task<IEnumerable<GeneralLedger>> GetAccountLedgerAsync(int chartOfAccountId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
        Task<decimal> GetAccountBalanceAsync(int chartOfAccountId, DateTime asOfDate, CancellationToken cancellationToken);
        Task<IEnumerable<GeneralLedger>> GetTrialBalanceAsync(int? branchId, DateTime asOfDate, CancellationToken cancellationToken);
        #endregion

        #region Accounts Receivable
        Task<IEnumerable<AccountsReceivable>> GetOverdueReceivablesAsync(int? branchId, CancellationToken cancellationToken);
        Task<decimal> GetTotalReceivablesByCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<IEnumerable<AccountsReceivable>> GetReceivablesByStatusAsync(ARStatus status, CancellationToken cancellationToken);
        #endregion

        #region Accounts Payable
        Task<IEnumerable<AccountsPayable>> GetOverduePayablesAsync(int? branchId, CancellationToken cancellationToken);
        Task<decimal> GetTotalPayablesByVendorAsync(int vendorId, CancellationToken cancellationToken);
        Task<IEnumerable<AccountsPayable>> GetPayablesByStatusAsync(APStatus status, CancellationToken cancellationToken);
        #endregion

        #region Budget
        Task<Budget?> GetBudgetWithLinesAsync(int budgetId, CancellationToken cancellationToken);
        Task<IEnumerable<BudgetLine>> GetBudgetVarianceAsync(int budgetId, CancellationToken cancellationToken);
        Task<Budget?> GetActiveBudgetAsync(int fiscalYear, int? branchId, CancellationToken cancellationToken);
        #endregion

        #region Expenses
        Task<decimal> GetTotalExpensesByDateRangeAsync(DateTime fromDate, DateTime toDate, int? branchId, CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetExpensesByCategoryAsync(int categoryId, CancellationToken cancellationToken);
        #endregion
    }
}
