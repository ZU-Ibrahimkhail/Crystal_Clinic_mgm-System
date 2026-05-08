using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Repositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get Account Ledger
    public class GetAccountLedgerQuery : IRequest<Result>
    {
        public int? ChartOfAccountId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class GetAccountLedgerQueryHandler(IAccountingRepository accountingRepository) : IRequestHandler<GetAccountLedgerQuery, Result>
    {
        public async Task<Result> Handle(GetAccountLedgerQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ledgerEntries = await accountingRepository.GetAccountLedgerAsync(
                    request.ChartOfAccountId, 
                    request.FromDate, 
                    request.ToDate, 
                    cancellationToken);

                var dtos = ledgerEntries
                    .Select(g => new GeneralLedgerDto
                    {
                        Id = g.Id,
                        ChartOfAccountId = g.ChartOfAccountId,
                        AccountCode = g.ChartOfAccount.AccountCode,
                        AccountName = g.ChartOfAccount.AccountName,
                        JournalEntryId = g.JournalEntryId,
                        BranchId = g.BranchId,
                        TransactionDate = g.TransactionDate,
                        Description = g.Description,
                        DebitAmount = g.DebitAmount,
                        CreditAmount = g.CreditAmount,
                        Balance = g.Balance
                    })
                    .ToList();

                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Account Ledger: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Trial Balance
    public class GetTrialBalanceQuery : IRequest<Result>
    {
        public int? BranchId { get; set; }
        public DateTime AsOfDate { get; set; }
    }

    public class GetTrialBalanceQueryHandler(ERP_DbContext context) : IRequestHandler<GetTrialBalanceQuery, Result>
    {
        public async Task<Result> Handle(GetTrialBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var glQuery = context.GeneralLedgers
                    .Where(g => g.TransactionDate <= request.AsOfDate && !g.IsDeleted);

                if (request.BranchId.HasValue)
                    glQuery = glQuery.Where(g => g.BranchId == request.BranchId);

                var aggregated = await glQuery
                    .GroupBy(g => g.ChartOfAccountId)
                    .Select(g => new
                    {
                        ChartOfAccountId = g.Key,
                        TotalDebit = g.Sum(x => x.DebitAmount),
                        TotalCredit = g.Sum(x => x.CreditAmount)
                    })
                    .ToListAsync(cancellationToken);

                var accountIds = aggregated.Select(a => a.ChartOfAccountId).ToList();

                var accounts = await context.ChartOfAccounts
                    .Where(c => accountIds.Contains(c.Id) && !c.IsDeleted)
                    .ToDictionaryAsync(c => c.Id, cancellationToken);

                var trialBalance = new List<TrialBalanceDto>();

                foreach (var row in aggregated)
                {
                    if (!accounts.TryGetValue(row.ChartOfAccountId, out var account))
                        continue;

                    decimal balance = account.NormalBalance == NormalBalanceType.Debit
                        ? row.TotalDebit - row.TotalCredit
                        : row.TotalCredit - row.TotalDebit;

                    if (balance == 0)
                        continue;

                    var tb = new TrialBalanceDto
                    {
                        AccountId = account.Id,
                        AccountCode = account.AccountCode,
                        AccountName = account.AccountName,
                        AccountType = account.AccountType,
                        DebitBalance = balance > 0 && account.NormalBalance == NormalBalanceType.Debit ? balance
                                     : balance < 0 && account.NormalBalance == NormalBalanceType.Credit ? Math.Abs(balance)
                                     : 0,
                        CreditBalance = balance > 0 && account.NormalBalance == NormalBalanceType.Credit ? balance
                                      : balance < 0 && account.NormalBalance == NormalBalanceType.Debit ? Math.Abs(balance)
                                      : 0
                    };

                    trialBalance.Add(tb);
                }

                var result = new
                {
                    trialBalance,
                    totalDebits = trialBalance.Sum(t => t.DebitBalance),
                    totalCredits = trialBalance.Sum(t => t.CreditBalance),
                    asOfDate = request.AsOfDate
                };

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Trial Balance: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Account Balance
    public class GetAccountBalanceQuery : IRequest<Result>
    {
        public int? ChartOfAccountId { get; set; } 
        public DateTime AsOfDate { get; set; }
    }

    public class GetAccountBalanceQueryHandler(IAccountingRepository accountingRepository, ERP_DbContext context) : IRequestHandler<GetAccountBalanceQuery, Result>
    {
        public async Task<Result> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ChartOfAccountId.HasValue)
                {
                    var account = await context.ChartOfAccounts
                        .FirstOrDefaultAsync(c => c.Id == request.ChartOfAccountId.Value && !c.IsDeleted, cancellationToken);

                    if (account == null)
                        return Result.Fail("Chart of Accounts not found.");

                    var balance = await accountingRepository.GetAccountBalanceAsync(
                        request.ChartOfAccountId,
                        request.AsOfDate,
                        cancellationToken);

                    var result = new
                    {
                        accountId = account.Id,
                        accountCode = account.AccountCode,
                        accountName = account.AccountName,
                        balance,
                        asOfDate = request.AsOfDate
                    };

                    return Result.Success(result);
                }
                else
                {
                    var balance = await accountingRepository.GetAccountBalanceAsync(
                        null,
                        request.AsOfDate,
                        cancellationToken);

                    var result = new
                    {
                        accountId = (int?)null,
                        accountCode = "ALL",
                        accountName = "All Accounts",
                        balance,
                        asOfDate = request.AsOfDate
                    };

                    return Result.Success(result);
                }
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Account Balance: {ex.Message}");
            }
        }
    }
    #endregion
}
