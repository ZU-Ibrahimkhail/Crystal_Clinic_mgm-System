using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get Income Statement
    public class GetIncomeStatementQuery : IRequest<Result>
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? BranchId { get; set; }
        public int? CurrencyId { get; set; }
    }

    public class GetIncomeStatementQueryHandler(ERP_DbContext context) : IRequestHandler<GetIncomeStatementQuery, Result>
    {
        public async Task<Result> Handle(GetIncomeStatementQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var statement = new IncomeStatementDto
                {
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    BranchId = request.BranchId
                };

                var ledgerQuery = context.GeneralLedgers
                    .Include(g => g.ChartOfAccount)
                    .Where(g => !g.IsDeleted && 
                               g.TransactionDate >= request.FromDate && 
                               g.TransactionDate <= request.ToDate);

                if (request.BranchId.HasValue)
                    ledgerQuery = ledgerQuery.Where(g => g.BranchId == request.BranchId);

                var ledgerEntries = await ledgerQuery.ToListAsync(cancellationToken);

                var groupedByAccount = ledgerEntries
                    .GroupBy(g => new { g.ChartOfAccountId, g.ChartOfAccount.AccountCode, g.ChartOfAccount.AccountName, g.ChartOfAccount.AccountType })
                    .Select(g => new
                    {
                        AccountId = g.Key.ChartOfAccountId,
                        AccountCode = g.Key.AccountCode,
                        AccountName = g.Key.AccountName,
                        AccountType = g.Key.AccountType,
                        Balance = g.Sum(x => x.DebitAmount - x.CreditAmount)
                    })
                    .ToList();

                foreach (var account in groupedByAccount)
                {
                    if (account.AccountType == AccountType.Revenue)
                    {
                        var revenueAmount = Math.Abs(account.Balance);
                        statement.RevenueLines.Add(new RevenueLineDto
                        {
                            AccountCode = account.AccountCode,
                            AccountName = account.AccountName,
                            Amount = revenueAmount
                        });
                        statement.TotalRevenue += revenueAmount;
                    }
                    else if (account.AccountType == AccountType.Expense)
                    {
                        var expenseAmount = Math.Abs(account.Balance);

                        if (account.AccountCode.StartsWith("5"))
                        {
                            statement.CostLines.Add(new CostLineDto
                            {
                                AccountCode = account.AccountCode,
                                AccountName = account.AccountName,
                                Amount = expenseAmount
                            });
                            statement.TotalCostOfGoodsSold += expenseAmount;
                        }
                        else if (account.AccountCode.StartsWith("6"))
                        {
                            statement.OperatingExpenseLines.Add(new ExpenseLineDto
                            {
                                AccountCode = account.AccountCode,
                                AccountName = account.AccountName,
                                Amount = expenseAmount
                            });
                            statement.TotalOperatingExpenses += expenseAmount;
                        }
                        else
                        {
                            statement.OtherExpenseLines.Add(new OtherExpenseLineDto
                            {
                                AccountCode = account.AccountCode,
                                AccountName = account.AccountName,
                                Amount = expenseAmount
                            });
                            statement.TotalOtherExpenses += expenseAmount;
                        }
                    }
                    else if (account.AccountType == AccountType.OtherIncome)
                    {
                        var otherIncomeAmount = Math.Abs(account.Balance);
                        statement.OtherIncomeLines.Add(new OtherIncomeLineDto
                        {
                            AccountCode = account.AccountCode,
                            AccountName = account.AccountName,
                            Amount = otherIncomeAmount
                        });
                        statement.TotalOtherIncome += otherIncomeAmount;
                    }
                }

                statement.GrossProfit = statement.TotalRevenue - statement.TotalCostOfGoodsSold;
                statement.OperatingIncome = statement.GrossProfit - statement.TotalOperatingExpenses;
                statement.NetIncome = statement.OperatingIncome + statement.TotalOtherIncome - statement.TotalOtherExpenses;

                return Result.Success(statement);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error generating income statement: {ex.Message}");
            }
        }
    }
    #endregion

    #region Compare Income Statements
    public class CompareIncomeStatementsQuery : IRequest<Result>
    {
        public DateTime FromDate1 { get; set; }
        public DateTime ToDate1 { get; set; }
        public DateTime FromDate2 { get; set; }
        public DateTime ToDate2 { get; set; }
        public int? BranchId { get; set; }
    }

    public class CompareIncomeStatementsQueryHandler(ERP_DbContext context) : IRequestHandler<CompareIncomeStatementsQuery, Result>
    {
        public async Task<Result> Handle(CompareIncomeStatementsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var handler = new GetIncomeStatementQueryHandler(context);

                var statement1 = await handler.Handle(new GetIncomeStatementQuery
                {
                    FromDate = request.FromDate1,
                    ToDate = request.ToDate1,
                    BranchId = request.BranchId
                }, cancellationToken);

                var statement2 = await handler.Handle(new GetIncomeStatementQuery
                {
                    FromDate = request.FromDate2,
                    ToDate = request.ToDate2,
                    BranchId = request.BranchId
                }, cancellationToken);

                if (!statement1.IsSuccess || !statement2.IsSuccess)
                    return Result.Fail("Error generating comparison statements");

                var stmt1 = statement1.Data as IncomeStatementDto;
                var stmt2 = statement2.Data as IncomeStatementDto;

                var comparison = new
                {
                    period1 = new { from = request.FromDate1, to = request.ToDate1, statement = stmt1 },
                    period2 = new { from = request.FromDate2, to = request.ToDate2, statement = stmt2 },
                    variance = new
                    {
                        revenue = stmt2.TotalRevenue - stmt1.TotalRevenue,

                        revenuePercentage = stmt1.TotalRevenue != 0 ? ((stmt2.TotalRevenue - stmt1.TotalRevenue) / stmt1.TotalRevenue * 100) : 0,
                        netIncome = stmt2.NetIncome - stmt1.NetIncome,
                        netIncomePercentage = stmt1.NetIncome != 0 ? ((stmt2.NetIncome - stmt1.NetIncome) / stmt1.NetIncome * 100) : 0
                    }
                };

                return Result.Success(comparison);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error comparing income statements: {ex.Message}");
            }
        }
    }
    #endregion
}
