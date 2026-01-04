using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Repositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Budgets
    public class GetAllBudgetsQuery : IRequest<Result>
    {
        public int? FiscalYear { get; set; }
        public BudgetStatus? Status { get; set; }
        public int? BranchId { get; set; }
    }

    public class GetAllBudgetsQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllBudgetsQuery, Result>
    {
        public async Task<Result> Handle(GetAllBudgetsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.Budgets.Where(b => !b.IsDeleted);

                if (request.FiscalYear.HasValue)
                    query = query.Where(b => b.FiscalYear == request.FiscalYear);

                if (request.Status.HasValue)
                    query = query.Where(b => b.Status == request.Status);

                if (request.BranchId.HasValue)
                    query = query.Where(b => b.BranchId == request.BranchId);

                var budgets = await query
                    .Include(b => b.BudgetLines)
                    .ThenInclude(l => l.ChartOfAccount)
                    .OrderByDescending(b => b.FiscalYear)
                    .Select(b => new BudgetDto
                    {
                        Id = b.Id,
                        BudgetName = b.BudgetName,
                        FiscalYear = b.FiscalYear,
                        Status = b.Status,
                        ApprovedBy = b.ApprovedBy,
                        ApprovedDate = b.ApprovedDate,
                        BranchId = b.BranchId,
                        Lines = b.BudgetLines
                            .Where(l => !l.IsDeleted)
                            .Select(l => new BudgetLineDto
                        {
                            Id = l.Id,
                            ChartOfAccountId = l.ChartOfAccountId,
                            AccountCode = l.ChartOfAccount.AccountCode,
                            AccountName = l.ChartOfAccount.AccountName,
                            PeriodId = l.PeriodId,
                            BudgetedAmount = l.BudgetedAmount,
                            ActualAmount = l.ActualAmount,
                            Variance = l.Variance
                        }).ToList()
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(budgets);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Budgets: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Budget Variance
    public class GetBudgetVarianceQuery : IRequest<Result>
    {
        public int BudgetId { get; set; }
        public decimal? VarianceThresholdAmount { get; set; }
        public decimal? VarianceThresholdPercentage { get; set; }
        public AccountType? AccountType { get; set; }
        public string? SortBy { get; set; } = "AccountCode";
        public bool SortDescending { get; set; } = false;
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }

    public class GetBudgetVarianceQueryHandler(IAccountingRepository accountingRepository, ERP_DbContext context) : IRequestHandler<GetBudgetVarianceQuery, Result>
    {
        public async Task<Result> Handle(GetBudgetVarianceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var budgetLines = await accountingRepository.GetBudgetVarianceAsync(request.BudgetId, cancellationToken);

                var variances = new List<BudgetVarianceDto>();
                foreach (var line in budgetLines)
                {
                    var account = await context.ChartOfAccounts
                        .FirstOrDefaultAsync(c => c.Id == line.ChartOfAccountId, cancellationToken);

                    if (account != null)
                    {
                        var variance = line.BudgetedAmount - line.ActualAmount;
                        var variancePercentage = line.BudgetedAmount == 0 ? 0 : (variance / line.BudgetedAmount) * 100;

                        variances.Add(new BudgetVarianceDto
                        {
                            BudgetLineId = line.Id,
                            ChartOfAccountId = account.Id,
                            AccountCode = account.AccountCode,
                            AccountName = account.AccountName,
                            AccountType = account.AccountType,
                            BudgetedAmount = line.BudgetedAmount,
                            ActualAmount = line.ActualAmount,
                            Variance = variance,
                            VariancePercentage = variancePercentage
                        });
                    }
                }

                var filteredVariances = variances.AsEnumerable();

                if (request.VarianceThresholdAmount.HasValue)
                    filteredVariances = filteredVariances
                        .Where(v => Math.Abs(v.Variance) >= request.VarianceThresholdAmount.Value);

                if (request.VarianceThresholdPercentage.HasValue)
                    filteredVariances = filteredVariances
                        .Where(v => Math.Abs(v.VariancePercentage) >= request.VarianceThresholdPercentage.Value);

                if (request.AccountType.HasValue)
                {
                    var accountIds = variances
                        .Where(v => v.AccountType == request.AccountType.Value)
                        .Select(v => v.ChartOfAccountId)
                        .ToList();
                    filteredVariances = filteredVariances
                        .Where(v => accountIds.Contains(v.ChartOfAccountId));
                }

                filteredVariances = ApplySorting(filteredVariances, request.SortBy, request.SortDescending);

                if (request.PageNumber.HasValue && request.PageSize.HasValue)
                {
                    var skip = (request.PageNumber.Value - 1) * request.PageSize.Value;
                    var result = new
                    {
                        data = filteredVariances.Skip(skip).Take(request.PageSize.Value).ToList(),
                        totalCount = filteredVariances.Count(),
                        pageNumber = request.PageNumber,
                        pageSize = request.PageSize
                    };
                    return Result.Success(result);
                }

                return Result.Success(filteredVariances.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Budget Variance: {ex.Message}");
            }
        }

        private IEnumerable<BudgetVarianceDto> ApplySorting(
            IEnumerable<BudgetVarianceDto> variances,
            string? sortBy,
            bool sortDescending)
        {
            return sortBy?.ToLower() switch
            {
                "variance" => sortDescending 
                    ? variances.OrderByDescending(v => v.Variance)
                    : variances.OrderBy(v => v.Variance),
                "variancepercentage" => sortDescending
                    ? variances.OrderByDescending(v => v.VariancePercentage)
                    : variances.OrderBy(v => v.VariancePercentage),
                "budgetedamount" => sortDescending
                    ? variances.OrderByDescending(v => v.BudgetedAmount)
                    : variances.OrderBy(v => v.BudgetedAmount),
                "actualamount" => sortDescending
                    ? variances.OrderByDescending(v => v.ActualAmount)
                    : variances.OrderBy(v => v.ActualAmount),
                "accountname" => sortDescending
                    ? variances.OrderByDescending(v => v.AccountName)
                    : variances.OrderBy(v => v.AccountName),
                _ => sortDescending
                    ? variances.OrderByDescending(v => v.AccountCode)
                    : variances.OrderBy(v => v.AccountCode)
            };
        }
    }
    #endregion
}
