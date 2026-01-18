using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IExpenseService
    {
        Task<Result> CreateExpenseAsync(CreateExpenseDto dto, Guid userId);
        Task<Result> UpdateExpenseAsync(UpdateExpenseDto dto);
        Task<Result> SubmitExpenseAsync(int expenseId, Guid userId);
        Task<Result> ApproveExpenseAsync(int expenseId, Guid approverId);
        Task<Result> RejectExpenseAsync(int expenseId, Guid rejectedBy, string reason);
        Task<Result> GetExpenseAsync(int expenseId);
        Task<Result> GetAllExpensesAsync(ExpenseStatus? status = null, int? branchId = null, int? categoryId = null, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10);
        Task<Result> GetPendingApprovalsAsync(int? branchId = null, int page = 1, int pageSize = 10);
    }
}
