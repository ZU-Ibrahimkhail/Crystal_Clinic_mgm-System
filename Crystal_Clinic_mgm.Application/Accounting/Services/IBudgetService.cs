using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IBudgetService
    {
        Task<Result> CreateBudgetAsync(CreateBudgetDto dto, CancellationToken cancellationToken = default);
        Task<Result> ApproveBudgetAsync(int budgetId, CancellationToken cancellationToken = default);
        Task<Result> ActivateBudgetAsync(int budgetId, CancellationToken cancellationToken = default);
        Task<Result> GetAllBudgetsAsync(int? fiscalYear = null, int? status = null, int? branchId = null, CancellationToken cancellationToken = default);
        Task<Result> GetBudgetVarianceAsync(int budgetId, CancellationToken cancellationToken = default);
    }
}
