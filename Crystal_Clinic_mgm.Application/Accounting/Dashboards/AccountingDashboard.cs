using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Dashboards
{
    #region Shared DTOs

    public class ChartSeriesDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal[] Data { get; set; } = [];
    }

    public class PieSliceDto
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    public class SpiderAxisDto
    {
        public string[] Categories { get; set; } = [];
        public List<ChartSeriesDto> Series { get; set; } = [];
    }

    public class TimeSeriesLabelsDto
    {
        public string[] Day { get; set; } = [];
        public string[] Week { get; set; } = [];
        public string[] Month { get; set; } = [];
        public string[] Year { get; set; } = [];
    }

    public class MultiSeriesChartDto
    {
        public TimeSeriesLabelsDto Labels { get; set; } = new();
        public List<GroupedSeriesDto> Groups { get; set; } = [];
    }

    public class GroupedSeriesDto
    {
        public string GroupName { get; set; } = string.Empty;
        public List<ChartSeriesDto> Series { get; set; } = [];
    }

    #endregion

    #region Bar Chart: Revenue vs Expenses

    public class AccountingRevenueExpensesBarChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class AccountingRevenueExpensesBarChartHandler(ERP_DbContext context)
        : IRequestHandler<AccountingRevenueExpensesBarChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(AccountingRevenueExpensesBarChartQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            var invoicesQuery = context.SalesInvoices
                .Where(i => i.Status != SalesStatus.Draft && i.Status != SalesStatus.Void);

            var expensesQuery = context.Expenses
                .Where(e => e.Status == ExpenseStatus.Approved || e.Status == ExpenseStatus.Paid);

            if (request.BranchId.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.BranchId == request.BranchId);
                expensesQuery = expensesQuery.Where(e => e.BranchId == request.BranchId);
            }

            if (request.StartDate.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.InvoiceDate >= request.StartDate.Value);
                expensesQuery = expensesQuery.Where(e => e.ExpenseDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.InvoiceDate <= request.EndDate.Value);
                expensesQuery = expensesQuery.Where(e => e.ExpenseDate <= request.EndDate.Value);
            }

            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var dailyRevenue = await invoicesQuery
                .Where(i => i.InvoiceDate >= startOfMonth && i.InvoiceDate <= endOfMonth)
                .GroupBy(i => i.InvoiceDate.Day)
                .Select(g => new { Day = g.Key, Total = g.Sum(i => i.NetAmount) })
                .ToListAsync(cancellationToken);

            var dailyExpense = await expensesQuery
                .Where(e => e.ExpenseDate >= startOfMonth && e.ExpenseDate <= endOfMonth)
                .GroupBy(e => e.ExpenseDate.Day)
                .Select(g => new { Day = g.Key, Total = g.Sum(e => e.Amount) })
                .ToListAsync(cancellationToken);

            var monthlyRevenue = await invoicesQuery
                .Where(i => i.InvoiceDate >= startOfYear && i.InvoiceDate <= endOfYear)
                .GroupBy(i => i.InvoiceDate.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(i => i.NetAmount) })
                .ToListAsync(cancellationToken);

            var monthlyExpense = await expensesQuery
                .Where(e => e.ExpenseDate >= startOfYear && e.ExpenseDate <= endOfYear)
                .GroupBy(e => e.ExpenseDate.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(e => e.Amount) })
                .ToListAsync(cancellationToken);

            var yearlyRevenue = await invoicesQuery
                .GroupBy(i => i.InvoiceDate.Year)
                .Select(g => new { Year = g.Key, Total = g.Sum(i => i.NetAmount) })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var yearlyExpense = await expensesQuery
                .GroupBy(e => e.ExpenseDate.Year)
                .Select(g => new { Year = g.Key, Total = g.Sum(e => e.Amount) })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var allYears = yearlyRevenue.Select(y => y.Year)
                .Union(yearlyExpense.Select(y => y.Year))
                .OrderBy(y => y).ToArray();

            var labels = new TimeSeriesLabelsDto
            {
                Day = Enumerable.Range(1, daysInMonth).Select(d => new DateTime(today.Year, today.Month, d).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(w => $"Week {w}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(m => new DateTime(today.Year, m, 1).ToString("MMM")).ToArray(),
                Year = allYears.Select(y => y.ToString()).ToArray()
            };

            var weeklyRevenue = await invoicesQuery
                .Where(i => i.InvoiceDate >= startOfYear && i.InvoiceDate <= endOfYear)
                .GroupBy(i => (i.InvoiceDate.DayOfYear - 1) / 7 + 1)
                .Select(g => new { Week = g.Key, Total = g.Sum(i => i.NetAmount) })
                .ToListAsync(cancellationToken);

            var weeklyExpense = await expensesQuery
                .Where(e => e.ExpenseDate >= startOfYear && e.ExpenseDate <= endOfYear)
                .GroupBy(e => (e.ExpenseDate.DayOfYear - 1) / 7 + 1)
                .Select(g => new { Week = g.Key, Total = g.Sum(e => e.Amount) })
                .ToListAsync(cancellationToken);

            var groups = new List<GroupedSeriesDto>
            {
                new()
                {
                    GroupName = "Day",
                    Series =
                    [
                        new ChartSeriesDto
                        {
                            Name = "Revenue",
                            Data = Enumerable.Range(1, daysInMonth).Select(d => dailyRevenue.FirstOrDefault(r => r.Day == d)?.Total ?? 0).ToArray()
                        },
                        new ChartSeriesDto
                        {
                            Name = "Expenses",
                            Data = Enumerable.Range(1, daysInMonth).Select(d => dailyExpense.FirstOrDefault(r => r.Day == d)?.Total ?? 0).ToArray()
                        }
                    ]
                },
                new()
                {
                    GroupName = "Week",
                    Series =
                    [
                        new ChartSeriesDto
                        {
                            Name = "Revenue",
                            Data = Enumerable.Range(1, 52).Select(w => weeklyRevenue.FirstOrDefault(r => r.Week == w)?.Total ?? 0).ToArray()
                        },
                        new ChartSeriesDto
                        {
                            Name = "Expenses",
                            Data = Enumerable.Range(1, 52).Select(w => weeklyExpense.FirstOrDefault(r => r.Week == w)?.Total ?? 0).ToArray()
                        }
                    ]
                },
                new()
                {
                    GroupName = "Month",
                    Series =
                    [
                        new ChartSeriesDto
                        {
                            Name = "Revenue",
                            Data = Enumerable.Range(1, 12).Select(m => monthlyRevenue.FirstOrDefault(r => r.Month == m)?.Total ?? 0).ToArray()
                        },
                        new ChartSeriesDto
                        {
                            Name = "Expenses",
                            Data = Enumerable.Range(1, 12).Select(m => monthlyExpense.FirstOrDefault(r => r.Month == m)?.Total ?? 0).ToArray()
                        }
                    ]
                },
                new()
                {
                    GroupName = "Year",
                    Series =
                    [
                        new ChartSeriesDto
                        {
                            Name = "Revenue",
                            Data = allYears.Select(y => yearlyRevenue.FirstOrDefault(r => r.Year == y)?.Total ?? 0).ToArray()
                        },
                        new ChartSeriesDto
                        {
                            Name = "Expenses",
                            Data = allYears.Select(y => yearlyExpense.FirstOrDefault(r => r.Year == y)?.Total ?? 0).ToArray()
                        }
                    ]
                }
            };

            return new JsonResult(new MultiSeriesChartDto { Labels = labels, Groups = groups });
        }
    }

    #endregion

    #region Line Chart: AR vs AP Trend

    public class AccountingARvsAPLineChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class AccountingARvsAPLineChartHandler(ERP_DbContext context)
        : IRequestHandler<AccountingARvsAPLineChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(AccountingARvsAPLineChartQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var arQuery = context.AccountsReceivables.AsQueryable();
            var apQuery = context.AccountsPayables.AsQueryable();

            if (request.BranchId.HasValue)
            {
                arQuery = arQuery.Where(a => a.BranchId == request.BranchId);
                apQuery = apQuery.Where(a => a.BranchId == request.BranchId);
            }

            if (request.StartDate.HasValue)
            {
                arQuery = arQuery.Where(a => a.InvoiceDate >= request.StartDate.Value);
                apQuery = apQuery.Where(a => a.InvoiceDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                arQuery = arQuery.Where(a => a.InvoiceDate <= request.EndDate.Value);
                apQuery = apQuery.Where(a => a.InvoiceDate <= request.EndDate.Value);
            }

            var dailyAR = await arQuery
                .Where(a => a.InvoiceDate >= startOfMonth && a.InvoiceDate <= endOfMonth)
                .GroupBy(a => a.InvoiceDate.Day)
                .Select(g => new { Day = g.Key, Balance = g.Sum(a => a.BalanceAmount) })
                .ToListAsync(cancellationToken);

            var dailyAP = await apQuery
                .Where(a => a.InvoiceDate >= startOfMonth && a.InvoiceDate <= endOfMonth)
                .GroupBy(a => a.InvoiceDate.Day)
                .Select(g => new { Day = g.Key, Balance = g.Sum(a => a.BalanceAmount) })
                .ToListAsync(cancellationToken);

            var monthlyAR = await arQuery
                .Where(a => a.InvoiceDate >= startOfYear && a.InvoiceDate <= endOfYear)
                .GroupBy(a => a.InvoiceDate.Month)
                .Select(g => new { Month = g.Key, Balance = g.Sum(a => a.BalanceAmount) })
                .ToListAsync(cancellationToken);

            var monthlyAP = await apQuery
                .Where(a => a.InvoiceDate >= startOfYear && a.InvoiceDate <= endOfYear)
                .GroupBy(a => a.InvoiceDate.Month)
                .Select(g => new { Month = g.Key, Balance = g.Sum(a => a.BalanceAmount) })
                .ToListAsync(cancellationToken);

            var weeklyAR = await arQuery
                .Where(a => a.InvoiceDate >= startOfYear && a.InvoiceDate <= endOfYear)
                .GroupBy(a => (a.InvoiceDate.DayOfYear - 1) / 7 + 1)
                .Select(g => new { Week = g.Key, Balance = g.Sum(a => a.BalanceAmount) })
                .ToListAsync(cancellationToken);

            var weeklyAP = await apQuery
                .Where(a => a.InvoiceDate >= startOfYear && a.InvoiceDate <= endOfYear)
                .GroupBy(a => (a.InvoiceDate.DayOfYear - 1) / 7 + 1)
                .Select(g => new { Week = g.Key, Balance = g.Sum(a => a.BalanceAmount) })
                .ToListAsync(cancellationToken);

            var yearlyAR = await arQuery
                .GroupBy(a => a.InvoiceDate.Year)
                .Select(g => new { Year = g.Key, Balance = g.Sum(a => a.BalanceAmount) })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var yearlyAP = await apQuery
                .GroupBy(a => a.InvoiceDate.Year)
                .Select(g => new { Year = g.Key, Balance = g.Sum(a => a.BalanceAmount) })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var allYears = yearlyAR.Select(y => y.Year)
                .Union(yearlyAP.Select(y => y.Year))
                .OrderBy(y => y).ToArray();

            var labels = new TimeSeriesLabelsDto
            {
                Day = Enumerable.Range(1, daysInMonth).Select(d => new DateTime(today.Year, today.Month, d).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(w => $"Week {w}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(m => new DateTime(today.Year, m, 1).ToString("MMM")).ToArray(),
                Year = allYears.Select(y => y.ToString()).ToArray()
            };

            var groups = new List<GroupedSeriesDto>
            {
                new()
                {
                    GroupName = "Day",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Accounts Receivable", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyAR.FirstOrDefault(x => x.Day == d)?.Balance ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Accounts Payable", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyAP.FirstOrDefault(x => x.Day == d)?.Balance ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Week",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Accounts Receivable", Data = Enumerable.Range(1, 52).Select(w => weeklyAR.FirstOrDefault(x => x.Week == w)?.Balance ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Accounts Payable", Data = Enumerable.Range(1, 52).Select(w => weeklyAP.FirstOrDefault(x => x.Week == w)?.Balance ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Month",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Accounts Receivable", Data = Enumerable.Range(1, 12).Select(m => monthlyAR.FirstOrDefault(x => x.Month == m)?.Balance ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Accounts Payable", Data = Enumerable.Range(1, 12).Select(m => monthlyAP.FirstOrDefault(x => x.Month == m)?.Balance ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Year",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Accounts Receivable", Data = allYears.Select(y => yearlyAR.FirstOrDefault(x => x.Year == y)?.Balance ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Accounts Payable", Data = allYears.Select(y => yearlyAP.FirstOrDefault(x => x.Year == y)?.Balance ?? 0).ToArray() }
                    ]
                }
            };

            return new JsonResult(new MultiSeriesChartDto { Labels = labels, Groups = groups });
        }
    }

    #endregion

    #region Pie Chart: Expenses by Category

    public class AccountingExpensesByCategoryPieChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class AccountingExpensesByCategoryPieChartHandler(ERP_DbContext context)
        : IRequestHandler<AccountingExpensesByCategoryPieChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(AccountingExpensesByCategoryPieChartQuery request, CancellationToken cancellationToken)
        {
            var query = context.Expenses
                .Include(e => e.ChartOfAccount)
                .Where(e => e.Status == ExpenseStatus.Approved || e.Status == ExpenseStatus.Paid);

            if (request.BranchId.HasValue)
                query = query.Where(e => e.BranchId == request.BranchId);

            if (request.StartDate.HasValue)
                query = query.Where(e => e.ExpenseDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(e => e.ExpenseDate <= request.EndDate.Value);

            var slices = await query
                .GroupBy(e => e.ChartOfAccount != null ? e.ChartOfAccount.AccountName : "Uncategorized")
                .Select(g => new PieSliceDto
                {
                    Label = g.Key,
                    Value = g.Sum(e => e.Amount)
                })
                .OrderByDescending(s => s.Value)
                .ToListAsync(cancellationToken);

            var apSlices = await context.AccountsPayables
                .Where(a => (request.BranchId == null || a.BranchId == request.BranchId)
                    && (request.StartDate == null || a.InvoiceDate >= request.StartDate.Value)
                    && (request.EndDate == null || a.InvoiceDate <= request.EndDate.Value))
                .GroupBy(a => a.Type.ToString())
                .Select(g => new PieSliceDto
                {
                    Label = "AP - " + g.Key,
                    Value = g.Sum(a => a.InvoiceAmount)
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(new
            {
                ExpensesByCategory = slices,
                APByType = apSlices,
                TotalExpenses = slices.Sum(s => s.Value),
                TotalAP = apSlices.Sum(s => s.Value)
            });
        }
    }

    #endregion

    #region Spider Chart: Financial KPIs

    public class AccountingFinancialKPIsSpiderChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class AccountingFinancialKPIsSpiderChartHandler(ERP_DbContext context)
        : IRequestHandler<AccountingFinancialKPIsSpiderChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(AccountingFinancialKPIsSpiderChartQuery request, CancellationToken cancellationToken)
        {
            var invoicesQuery = context.SalesInvoices
                .Where(i => i.Status != SalesStatus.Draft && i.Status != SalesStatus.Void);

            var expensesQuery = context.Expenses
                .Where(e => e.Status == ExpenseStatus.Approved || e.Status == ExpenseStatus.Paid);

            var arQuery = context.AccountsReceivables.AsQueryable();
            var apQuery = context.AccountsPayables.AsQueryable();

            if (request.BranchId.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.BranchId == request.BranchId);
                expensesQuery = expensesQuery.Where(e => e.BranchId == request.BranchId);
                arQuery = arQuery.Where(a => a.BranchId == request.BranchId);
                apQuery = apQuery.Where(a => a.BranchId == request.BranchId);
            }

            if (request.StartDate.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.InvoiceDate >= request.StartDate.Value);
                expensesQuery = expensesQuery.Where(e => e.ExpenseDate >= request.StartDate.Value);
                arQuery = arQuery.Where(a => a.InvoiceDate >= request.StartDate.Value);
                apQuery = apQuery.Where(a => a.InvoiceDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.InvoiceDate <= request.EndDate.Value);
                expensesQuery = expensesQuery.Where(e => e.ExpenseDate <= request.EndDate.Value);
                arQuery = arQuery.Where(a => a.InvoiceDate <= request.EndDate.Value);
                apQuery = apQuery.Where(a => a.InvoiceDate <= request.EndDate.Value);
            }

            var totalRevenue = await invoicesQuery.SumAsync(i => i.NetAmount, cancellationToken);
            var totalExpenses = await expensesQuery.SumAsync(e => e.Amount, cancellationToken);
            var totalAR = await arQuery.SumAsync(a => a.InvoiceAmount, cancellationToken);
            var totalARPaid = await arQuery.SumAsync(a => a.PaidAmount, cancellationToken);
            var totalARBalance = await arQuery.SumAsync(a => a.BalanceAmount, cancellationToken);
            var totalAP = await apQuery.SumAsync(a => a.InvoiceAmount, cancellationToken);
            var totalAPPaid = await apQuery.SumAsync(a => a.PaidAmount, cancellationToken);
            var totalAPBalance = await apQuery.SumAsync(a => a.BalanceAmount, cancellationToken);

            var branchData = await context.SalesInvoices
                .Where(i => i.Status != SalesStatus.Draft && i.Status != SalesStatus.Void
                    && (request.StartDate == null || i.InvoiceDate >= request.StartDate.Value)
                    && (request.EndDate == null || i.InvoiceDate <= request.EndDate.Value))
                .Include(i => i.Branch)
                .GroupBy(i => new { i.BranchId, BranchName = i.Branch != null ? i.Branch.EnglishName : "Unknown" })
                .Select(g => new
                {
                    g.Key.BranchName,
                    Revenue = g.Sum(i => i.NetAmount)
                })
                .ToListAsync(cancellationToken);

            var categories = new[] { "Total Revenue", "Total Expenses", "AR Invoiced", "AR Collected", "AR Balance", "AP Invoiced", "AP Paid", "AP Balance" };

            var spiderData = new SpiderAxisDto
            {
                Categories = categories,
                Series =
                [
                    new ChartSeriesDto
                    {
                        Name = "Financial Overview",
                        Data = [totalRevenue, totalExpenses, totalAR, totalARPaid, totalARBalance, totalAP, totalAPPaid, totalAPBalance]
                    }
                ]
            };

            return new JsonResult(new
            {
                Spider = spiderData,
                BranchRevenue = branchData,
                Summary = new
                {
                    TotalRevenue = totalRevenue,
                    TotalExpenses = totalExpenses,
                    NetIncome = totalRevenue - totalExpenses,
                    TotalAR = totalAR,
                    TotalARCollected = totalARPaid,
                    TotalARBalance = totalARBalance,
                    TotalAP = totalAP,
                    TotalAPPaid = totalAPPaid,
                    TotalAPBalance = totalAPBalance
                }
            });
        }
    }

    #endregion
}
