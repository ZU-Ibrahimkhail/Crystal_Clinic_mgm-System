using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get All Chart of Accounts
    public class GetAllChartOfAccountsQuery : IRequest<Result>
    {
        public bool? IsActive { get; set; }
        public int? AccountType { get; set; }
        public string? SearchText { get; set; }
    }

    public class GetAllChartOfAccountsQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllChartOfAccountsQuery, Result>
    {
        public async Task<Result> Handle(GetAllChartOfAccountsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.ChartOfAccounts.Where(c => !c.IsDeleted);

                if (request.IsActive.HasValue)
                    query = query.Where(c => c.IsActive == request.IsActive);

                if (request.AccountType.HasValue)
                    query = query.Where(c => (int)c.AccountType == request.AccountType);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                    query = query.Where(c => c.AccountCode.Contains(request.SearchText) || 
                                             c.AccountName.Contains(request.SearchText));

                var accounts = await query
                    .OrderBy(c => c.AccountCode)
                    .Select(c => new ChartOfAccountsDto
                    {
                        Id = c.Id,
                        AccountCode = c.AccountCode,
                        AccountName = c.AccountName,
                        AccountType = c.AccountType,
                        AccountCategory = c.AccountCategory,
                        NormalBalance = c.NormalBalance,
                        IsSystemAccount = false,
                        IsActive = c.IsActive,
                        Description = c.Description,
                        ParentAccountId = c.ParentAccountId

                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(accounts);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Chart of Accounts: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Chart of Accounts By Id
    public class GetChartOfAccountsByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetChartOfAccountsByIdQueryHandler(ERP_DbContext context) : IRequestHandler<GetChartOfAccountsByIdQuery, Result>
    {
        public async Task<Result> Handle(GetChartOfAccountsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await context.ChartOfAccounts
                    .Include(c => c.ChildAccounts)
                    .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

                if (account == null)
                    return Result.Fail("Chart of Accounts not found.");

                var dto = new ChartOfAccountsDto
                {
                    Id = account.Id,
                    AccountCode = account.AccountCode,
                    AccountName = account.AccountName,
                    AccountType = account.AccountType,
                    AccountCategory = account.AccountCategory,
                    NormalBalance = account.NormalBalance,
                    IsSystemAccount = account.IsSystemAccount,
                    IsActive = account.IsActive,
                    Description = account.Description,
                    ParentAccountId = account.ParentAccountId
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Chart of Accounts: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Accounts By Type
    public class GetAccountsByTypeQuery : IRequest<Result>
    {
        public int AccountType { get; set; }
    }

    public class GetAccountsByTypeQueryHandler(ERP_DbContext context) : IRequestHandler<GetAccountsByTypeQuery, Result>
    {
        public async Task<Result> Handle(GetAccountsByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await context.ChartOfAccounts
                    .Where(c => (int)c.AccountType == request.AccountType && c.IsActive && !c.IsDeleted)
                    .OrderBy(c => c.AccountCode)
                    .Select(c => new ChartOfAccountsDto
                    {
                        Id = c.Id,
                        AccountCode = c.AccountCode,
                        AccountName = c.AccountName,
                        AccountType = c.AccountType,
                        AccountCategory = c.AccountCategory,
                        NormalBalance = c.NormalBalance,
                        IsSystemAccount = c.IsSystemAccount,
                        IsActive = c.IsActive,
                        Description = c.Description,
                        ParentAccountId = c.ParentAccountId
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(accounts);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Accounts: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Account Hierarchy
    public class GetAccountHierarchyQuery : IRequest<Result>
    {
        public int? ParentAccountId { get; set; }
    }

    public class GetAccountHierarchyQueryHandler(ERP_DbContext context) : IRequestHandler<GetAccountHierarchyQuery, Result>
    {
        public async Task<Result> Handle(GetAccountHierarchyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await context.ChartOfAccounts
                    .Where(c => !c.IsDeleted && c.ParentAccountId == request.ParentAccountId)
                    .Include(c => c.ChildAccounts)
                    .OrderBy(c => c.AccountCode)
                    .Select(c => new AccountHierarchyDto
                    {
                        Id = c.Id,
                        AccountCode = c.AccountCode,
                        AccountName = c.AccountName,
                        AccountType = c.AccountType,
                        IsActive = c.IsActive,
                        ParentAccountId = c.ParentAccountId,
                        ChildCount = c.ChildAccounts.Count(ch => !ch.IsDeleted)
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(accounts);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving account hierarchy: {ex.Message}");
            }
        }
    }
    #endregion
}

public class AccountHierarchyDto
{
    public int Id { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public bool IsActive { get; set; }
    public int? ParentAccountId { get; set; }
    public int ChildCount { get; set; }
}
