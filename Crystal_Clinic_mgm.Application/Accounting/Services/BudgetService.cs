using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using MediatR;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class BudgetService(IMediator mediator) : IBudgetService
    {
        public async Task<Result> CreateBudgetAsync(CreateBudgetDto dto, CancellationToken cancellationToken = default)
        {
            var command = new CreateBudgetCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> ApproveBudgetAsync(int budgetId, CancellationToken cancellationToken = default)
        {
            var command = new ApproveBudgetCommand { BudgetId = budgetId };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> ActivateBudgetAsync(int budgetId, CancellationToken cancellationToken = default)
        {
            var command = new ActivateBudgetCommand { BudgetId = budgetId };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> GetAllBudgetsAsync(int? fiscalYear = null, int? status = null, int? branchId = null, CancellationToken cancellationToken = default)
        {
            var query = new GetAllBudgetsQuery
            {
                FiscalYear = fiscalYear,
                Status = (BudgetStatus?)status,
                BranchId = branchId
            };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetBudgetVarianceAsync(int budgetId, CancellationToken cancellationToken = default)
        {
            var query = new GetBudgetVarianceQuery { BudgetId = budgetId };
            return await mediator.Send(query, cancellationToken);
        }
    }
}
