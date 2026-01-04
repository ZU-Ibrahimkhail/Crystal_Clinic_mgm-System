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
    #region Create Budget
    public class CreateBudgetCommand : IRequest<Result>
    {
        public CreateBudgetDto Dto { get; set; } = null!;
    }

    public class CreateBudgetCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateBudgetCommand, Result>
    {
        public async Task<Result> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingBudget = await context.Budgets
                    .FirstOrDefaultAsync(b => b.FiscalYear == request.Dto.FiscalYear && !b.IsDeleted && 
                        (request.Dto.BranchId == null || b.BranchId == request.Dto.BranchId), cancellationToken);

                if (existingBudget != null)
                    return Result.Fail("Budget for this fiscal year already exists.");

                var budget = new Budget
                {
                    BudgetName = request.Dto.BudgetName,
                    FiscalYear = request.Dto.FiscalYear,
                    Status = BudgetStatus.Draft,
                    BranchId = request.Dto.BranchId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                if (request.Dto.Lines.Any())
                {
                    foreach (var lineDto in request.Dto.Lines)
                    {
                        var account = await context.ChartOfAccounts
                            .FirstOrDefaultAsync(c => c.Id == lineDto.ChartOfAccountId && !c.IsDeleted, cancellationToken);

                        if (account == null)
                            return Result.Fail($"Account {lineDto.ChartOfAccountId} not found.");

                        var line = new BudgetLine
                        {
                            ChartOfAccountId = lineDto.ChartOfAccountId,
                            BranchId = request.Dto.BranchId,
                            PeriodId = lineDto.PeriodId,
                            BudgetedAmount = lineDto.BudgetedAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        budget.BudgetLines.Add(line);
                    }
                }

                context.Budgets.Add(budget);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(budget.Id, "Budget created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Budget: {ex.Message}");
            }
        }
    }
    #endregion

    #region Approve Budget
    public class ApproveBudgetCommand : IRequest<Result>
    {
        public int BudgetId { get; set; }
    }

    public class ApproveBudgetCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ApproveBudgetCommand, Result>
    {
        public async Task<Result> Handle(ApproveBudgetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var budget = await context.Budgets
                    .FirstOrDefaultAsync(b => b.Id == request.BudgetId && !b.IsDeleted, cancellationToken);

                if (budget == null)
                    return Result.Fail("Budget not found.");

                if (budget.Status != BudgetStatus.Draft)
                    return Result.Fail("Only draft budgets can be approved.");

                budget.Status = BudgetStatus.Approved;
                budget.ApprovedBy = loggedInUser.Id;
                budget.ApprovedDate = DateTime.UtcNow;
                budget.ModifiedBy = loggedInUser.Id;
                budget.ModifiedOn = DateTime.UtcNow;

                context.Budgets.Update(budget);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Budget approved successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error approving Budget: {ex.Message}");
            }
        }
    }
    #endregion

    #region Activate Budget
    public class ActivateBudgetCommand : IRequest<Result>
    {
        public int BudgetId { get; set; }
    }

    public class ActivateBudgetCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ActivateBudgetCommand, Result>
    {
        public async Task<Result> Handle(ActivateBudgetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var budget = await context.Budgets
                    .FirstOrDefaultAsync(b => b.Id == request.BudgetId && !b.IsDeleted, cancellationToken);

                if (budget == null)
                    return Result.Fail("Budget not found.");

                if (budget.Status != BudgetStatus.Approved)
                    return Result.Fail("Only approved budgets can be activated.");

                budget.Status = BudgetStatus.Active;
                budget.ModifiedBy = loggedInUser.Id;
                budget.ModifiedOn = DateTime.UtcNow;

                context.Budgets.Update(budget);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Budget activated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error activating Budget: {ex.Message}");
            }
        }
    }
    #endregion
}
