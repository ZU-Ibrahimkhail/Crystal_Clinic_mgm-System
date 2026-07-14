using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get Balance Sheet
    public class GetBalanceSheetQuery : IRequest<Result>
    {
        public DateTime AsOfDate { get; set; } = DateTime.Now;
        public int? BranchId { get; set; }
    }

    public class GetBalanceSheetQueryHandler(ERP_DbContext context) : IRequestHandler<GetBalanceSheetQuery, Result>
    {
        public async Task<Result> Handle(GetBalanceSheetQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.GeneralLedgers
                    .Where(g => g.TransactionDate <= request.AsOfDate && !g.ChartOfAccount.IsDeleted);

                if (request.BranchId.HasValue)
                    query = query.Where(g => g.BranchId == request.BranchId);

                var ledgerEntries = await query.ToListAsync(cancellationToken);
                var accounts = await context.ChartOfAccounts
                    .Where(a => !a.IsDeleted)
                    .ToListAsync(cancellationToken);

                var balanceSheet = new
                {
                    asOfDate = request.AsOfDate,
                    branchId = request.BranchId,
                    assets = new
                    {
                        currentAssets = CalculateAccountTypeTotal(ledgerEntries, accounts, AccountType.Asset, true, request.AsOfDate),
                        fixedAssets = CalculateAccountTypeTotal(ledgerEntries, accounts, AccountType.Asset, false, request.AsOfDate),
                        otherAssets = CalculateAssetsByCategory(ledgerEntries, accounts, AccountCategory.OtherAsset, request.AsOfDate)
                    },
                    liabilities = new
                    {
                        currentLiabilities = CalculateLiabilitiesByCategory(ledgerEntries, accounts, AccountCategory.CurrentLiability, request.AsOfDate),
                        longTermLiabilities = CalculateLiabilitiesByCategory(ledgerEntries, accounts, AccountCategory.LongTermLiability, request.AsOfDate)
                    },
                    equity = new
                    {
                        capital = CalculateEquityByCategory(ledgerEntries, accounts, AccountCategory.Capital, request.AsOfDate),
                        retainedEarnings = CalculateEquityByCategory(ledgerEntries, accounts, AccountCategory.RetainedEarnings, request.AsOfDate)
                    }
                };

                var totalAssets = (decimal)balanceSheet.assets.currentAssets + 
                                (decimal)balanceSheet.assets.fixedAssets + 
                                (decimal)balanceSheet.assets.otherAssets;

                var totalLiabilities = (decimal)balanceSheet.liabilities.currentLiabilities + 
                                     (decimal)balanceSheet.liabilities.longTermLiabilities;

                var totalEquity = (decimal)balanceSheet.equity.capital + 
                                (decimal)balanceSheet.equity.retainedEarnings;

                var result = new
                {
                    balanceSheet,
                    totals = new
                    {
                        totalAssets,
                        totalLiabilities,
                        totalEquity,
                        totalLiabilitiesAndEquity = totalLiabilities + totalEquity,
                        difference = totalAssets - (totalLiabilities + totalEquity)
                    }
                };

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error generating Balance Sheet: {ex.Message}");
            }
        }

        private decimal CalculateAccountTypeTotal(
            List<GeneralLedger> ledgerEntries,
            List<ChartOfAccounts> accounts,
            AccountType accountType,
            bool isCurrent,
            DateTime asOfDate)
        {
            var typeAccounts = accounts
                .Where(a => a.AccountType == accountType && 
                           ((isCurrent && a.AccountCategory == AccountCategory.CurrentAsset) ||
                            (!isCurrent && a.AccountCategory == AccountCategory.FixedAsset)))
                .Select(a => a.Id)
                .ToList();

            return ledgerEntries
                .Where(g => typeAccounts.Contains(g.ChartOfAccountId) && g.TransactionDate <= asOfDate)
                .GroupBy(g => g.ChartOfAccountId)
                .Sum(g => g.Last().Balance);
        }

        private decimal CalculateAssetsByCategory(
            List<GeneralLedger> ledgerEntries,
            List<ChartOfAccounts> accounts,
            AccountCategory category,
            DateTime asOfDate)
        {
            var categoryAccounts = accounts
                .Where(a => a.AccountCategory == category)
                .Select(a => a.Id)
                .ToList();

            return ledgerEntries
                .Where(g => categoryAccounts.Contains(g.ChartOfAccountId) && g.TransactionDate <= asOfDate)
                .GroupBy(g => g.ChartOfAccountId)
                .Sum(g => g.Last().Balance);
        }

        private decimal CalculateLiabilitiesByCategory(
            List<GeneralLedger> ledgerEntries,
            List<ChartOfAccounts> accounts,
            AccountCategory category,
            DateTime asOfDate)
        {
            var categoryAccounts = accounts
                .Where(a => a.AccountCategory == category)
                .Select(a => a.Id)
                .ToList();

            return ledgerEntries
                .Where(g => categoryAccounts.Contains(g.ChartOfAccountId) && g.TransactionDate <= asOfDate)
                .GroupBy(g => g.ChartOfAccountId)
                .Sum(g => g.Last().Balance);
        }

        private decimal CalculateEquityByCategory(
            List<GeneralLedger> ledgerEntries,
            List<ChartOfAccounts> accounts,
            AccountCategory category,
            DateTime asOfDate)
        {
            var categoryAccounts = accounts
                .Where(a => a.AccountCategory == category)
                .Select(a => a.Id)
                .ToList();

            return ledgerEntries
                .Where(g => categoryAccounts.Contains(g.ChartOfAccountId) && g.TransactionDate <= asOfDate)
                .GroupBy(g => g.ChartOfAccountId)
                .Sum(g => g.Last().Balance);
        }
    }
    #endregion
}
