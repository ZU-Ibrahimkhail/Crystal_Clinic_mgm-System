using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using MediatR;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class ExpenseService(IMediator mediator) : IExpenseService
    {
        public async Task<Result> CreateExpenseAsync(CreateExpenseDto dto, int userId)
        {
            var command = new CreateExpenseCommand { Dto = dto, UserId = userId };
            return await mediator.Send(command);
        }

        public async Task<Result> UpdateExpenseAsync(UpdateExpenseDto dto)
        {
            var command = new UpdateExpenseCommand { Dto = dto };
            return await mediator.Send(command);
        }

        public async Task<Result> SubmitExpenseAsync(int expenseId, int userId)
        {
            var command = new SubmitExpenseCommand { ExpenseId = expenseId, SubmittedBy = userId };
            return await mediator.Send(command);
        }

        public async Task<Result> ApproveExpenseAsync(int expenseId, int approverId)
        {
            var command = new ApproveExpenseCommand { ExpenseId = expenseId, ApprovedBy = approverId };
            return await mediator.Send(command);
        }

        public async Task<Result> RejectExpenseAsync(int expenseId, int rejectedBy, string reason)
        {
            var command = new RejectExpenseCommand { ExpenseId = expenseId, RejectedBy = rejectedBy, RejectionReason = reason };
            return await mediator.Send(command);
        }

        public async Task<Result> GetExpenseAsync(int expenseId)
        {
            var query = new GetExpenseQuery { ExpenseId = expenseId };
            return await mediator.Send(query);
        }

        public async Task<Result> GetAllExpensesAsync(ExpenseStatus? status = null, int? branchId = null, int? categoryId = null, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10)
        {
            var query = new GetAllExpensesQuery
            {
                Status = status,
                BranchId = branchId,
                CategoryId = categoryId,
                FromDate = fromDate,
                ToDate = toDate,
                Page = page,
                PageSize = pageSize
            };
            return await mediator.Send(query);
        }

        public async Task<Result> GetPendingApprovalsAsync(int? branchId = null, int page = 1, int pageSize = 10)
        {
            var query = new GetPendingExpenseApprovalsQuery { BranchId = branchId, Page = page, PageSize = pageSize };
            return await mediator.Send(query);
        }
    }
}
