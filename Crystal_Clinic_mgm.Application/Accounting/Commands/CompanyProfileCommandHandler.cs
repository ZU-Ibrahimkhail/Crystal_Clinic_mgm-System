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
    #region Setup Company Profile
    public class SetupCompanyCommand : IRequest<Result>
    {
        public CompanyProfileDto Dto { get; set; } = null!;
    }

    public class SetupCompanyCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<SetupCompanyCommand, Result>
    {
        public async Task<Result> Handle(SetupCompanyCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var existingProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                if (existingProfile != null && existingProfile.IsInitialized)
                    return Result.Fail("Company profile is already initialized.");

                var profile = new CompanyProfile
                {
                    Name = request.Dto.Name,
                    PhoneNumber = request.Dto.PhoneNumber,
                    Email = request.Dto.Email,
                    BaseCurrencyId = request.Dto.BaseCurrencyId,
                    IsInitialized = true,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };
                context.CompanyProfile.Add(profile);
                await context.SaveChangesAsync(cancellationToken);

                var systemAccounts = await SeedChartOfAccounts(context, cancellationToken);

                profile.CashAccountId = systemAccounts.CashId;
                profile.AccountsReceivableAccountId = systemAccounts.ARId;
                profile.AccountsPayableAccountId = systemAccounts.APId;
                profile.SalesRevenueAccountId = systemAccounts.RevenueId;
                profile.InventoryAccountId = systemAccounts.InventoryId;
                profile.PurchaseExpenseAccountId = systemAccounts.ExpenseId;

                await context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return Result.Success(profile.Id, "Company profile initialized and Chart of Accounts seeded successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Fail($"Error setting up company profile: {ex.Message}");
            }
        }

        private class SystemAccountsResult
        {
            public int CashId { get; set; }
            public int ARId { get; set; }
            public int APId { get; set; }
            public int RevenueId { get; set; }
            public int InventoryId { get; set; }
            public int ExpenseId { get; set; }
        }

        private async Task<SystemAccountsResult> SeedChartOfAccounts(ERP_DbContext context, CancellationToken ct)
        {
            var accounts = new List<ChartOfAccounts>();

            var assets = AddAccount(accounts, "1000", "Assets", AccountType.Asset, AccountCategory.CurrentAsset, NormalBalanceType.Debit, null);
            var currentAssets = AddAccount(accounts, "1100", "Current Assets", AccountType.Asset, AccountCategory.CurrentAsset, NormalBalanceType.Debit, assets);

            var cashAccount = AddAccount(accounts, "1101", "Cash and Cash Equivalents", AccountType.Asset, AccountCategory.CurrentAsset, NormalBalanceType.Debit, currentAssets);
            var aRAccount = AddAccount(accounts, "1102", "Accounts Receivable", AccountType.Asset, AccountCategory.CurrentAsset, NormalBalanceType.Debit, currentAssets);
            var inventoryAccount = AddAccount(accounts, "1103", "Inventory", AccountType.Asset, AccountCategory.CurrentAsset, NormalBalanceType.Debit, currentAssets);
            AddAccount(accounts, "1104", "Prepaid Expenses", AccountType.Asset, AccountCategory.CurrentAsset, NormalBalanceType.Debit, currentAssets);

            var fixedAssets = AddAccount(accounts, "1200", "Fixed Assets", AccountType.Asset, AccountCategory.FixedAsset, NormalBalanceType.Debit, assets);
            AddAccount(accounts, "1201", "Property, Plant & Equipment", AccountType.Asset, AccountCategory.FixedAsset, NormalBalanceType.Debit, fixedAssets);
            AddAccount(accounts, "1202", "Accumulated Depreciation", AccountType.ContraAsset, AccountCategory.FixedAsset, NormalBalanceType.Credit, fixedAssets);

            var liabilities = AddAccount(accounts, "2000", "Liabilities", AccountType.Liability, AccountCategory.CurrentLiability, NormalBalanceType.Credit, null);
            var currentLiabilities = AddAccount(accounts, "2100", "Current Liabilities", AccountType.Liability, AccountCategory.CurrentLiability, NormalBalanceType.Credit, liabilities);
            var aPAccount = AddAccount(accounts, "2101", "Accounts Payable", AccountType.Liability, AccountCategory.CurrentLiability, NormalBalanceType.Credit, currentLiabilities);
            AddAccount(accounts, "2102", "Employee Advances", AccountType.Liability, AccountCategory.CurrentLiability, NormalBalanceType.Credit, currentLiabilities);
            AddAccount(accounts, "2103", "Taxes Payable", AccountType.Liability, AccountCategory.CurrentLiability, NormalBalanceType.Credit, currentLiabilities);

            var equity = AddAccount(accounts, "3000", "Equity", AccountType.Equity, AccountCategory.Capital, NormalBalanceType.Credit, null);
            AddAccount(accounts, "3100", "Capital", AccountType.Equity, AccountCategory.Capital, NormalBalanceType.Credit, equity);
            AddAccount(accounts, "3200", "Retained Earnings", AccountType.Equity, AccountCategory.RetainedEarnings, NormalBalanceType.Credit, equity);

            var revenueGroup = AddAccount(accounts, "4000", "Revenue", AccountType.Revenue, AccountCategory.ServiceRevenue, NormalBalanceType.Credit, null);
            var revenueAccount = AddAccount(accounts, "4100", "Service Revenue", AccountType.Revenue, AccountCategory.ServiceRevenue, NormalBalanceType.Credit, revenueGroup);
            AddAccount(accounts, "4200", "Other Revenue", AccountType.Revenue, AccountCategory.OtherRevenue, NormalBalanceType.Credit, revenueGroup);

            var expensesGroup = AddAccount(accounts, "5000", "Expenses", AccountType.Expense, AccountCategory.OperatingExpense, NormalBalanceType.Debit, null);
            var expenseAccount = AddAccount(accounts, "5100", "Operating Expenses", AccountType.Expense, AccountCategory.OperatingExpense, NormalBalanceType.Debit, expensesGroup);
            AddAccount(accounts, "5101", "Salaries & Wages", AccountType.Expense, AccountCategory.OperatingExpense, NormalBalanceType.Debit, expenseAccount);
            AddAccount(accounts, "5102", "Medical Supplies", AccountType.Expense, AccountCategory.OperatingExpense, NormalBalanceType.Debit, expenseAccount);
            AddAccount(accounts, "5103", "Utilities", AccountType.Expense, AccountCategory.OperatingExpense, NormalBalanceType.Debit, expenseAccount);
            AddAccount(accounts, "5200", "Administrative Expenses", AccountType.Expense, AccountCategory.AdministrativeExpense, NormalBalanceType.Debit, expensesGroup);
            AddAccount(accounts, "5300", "Financial Expenses", AccountType.Expense, AccountCategory.FinancialExpense, NormalBalanceType.Debit, expensesGroup);

            context.ChartOfAccounts.AddRange(accounts);
            await context.SaveChangesAsync(ct);

            return new SystemAccountsResult
            {
                CashId = cashAccount.Id,
                ARId = aRAccount.Id,
                InventoryId = inventoryAccount.Id,
                APId = aPAccount.Id,
                RevenueId = revenueAccount.Id,
                ExpenseId = expenseAccount.Id
            };
        }


        private ChartOfAccounts AddAccount(List<ChartOfAccounts> list, string code, string name, AccountType type, AccountCategory category, NormalBalanceType balance, ChartOfAccounts? parent)
        {
            var account = new ChartOfAccounts
            {
                AccountCode = code,
                AccountName = name,
                AccountType = type,
                AccountCategory = category,
                NormalBalance = balance,
                IsSystemAccount = true,
                IsActive = true,
                ParentAccountId = parent?.Id,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.UtcNow
            };
            list.Add(account);
            return account;
        }
    }
    #endregion

    #region Update Company Profile
    public class UpdateCompanyProfileCommand : IRequest<Result>
    {
        public CompanyProfileDto Dto { get; set; } = null!;
    }

    public class UpdateCompanyProfileCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateCompanyProfileCommand, Result>
    {
        public async Task<Result> Handle(UpdateCompanyProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var profile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                if (profile == null)
                    return Result.Fail("Company profile not found. Please initialize it first.");

                profile.Name = request.Dto.Name;
                profile.PhoneNumber = request.Dto.PhoneNumber;
                profile.Email = request.Dto.Email;
                profile.BaseCurrencyId = request.Dto.BaseCurrencyId;
                profile.ModifiedBy = loggedInUser.Id;
                profile.ModifiedOn = DateTime.UtcNow;

                context.CompanyProfile.Update(profile);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Company profile updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating company profile: {ex.Message}");
            }
        }
    }
    #endregion
}
