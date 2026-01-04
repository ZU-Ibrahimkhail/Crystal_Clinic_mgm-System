using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get Expense
    public class GetExpenseQuery : IRequest<Result>
    {
        public int ExpenseId { get; set; }
    }

    public class GetExpenseQueryHandler(ERP_DbContext context) : IRequestHandler<GetExpenseQuery, Result>
    {
        public async Task<Result> Handle(GetExpenseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var expense = await context.Expenses
                    .Include(e => e.ChartOfAccount)
                    .Include(e => e.Customer)
                    .FirstOrDefaultAsync(e => e.Id == request.ExpenseId && !e.IsDeleted, cancellationToken);

                if (expense == null)
                    return Result.Fail("Expense not found");

                var dto = new ExpenseDto
                {
                    Id = expense.Id,
                    CategoryId = expense.CategoryId,
                    ClassId = expense.ClassId,
                    Amount = expense.Amount,
                    ExpenseDate = expense.ExpenseDate,
                    Description = expense.Description,
                    CustomerId = expense.CustomerId,
                    CustomerName = expense.Customer?.name,
                    IsReimbursable = expense.IsReimbursable,
                    ChartOfAccountId = expense.ChartOfAccountId,
                    ChartOfAccountName = expense.ChartOfAccount?.AccountName,
                    BranchId = expense.BranchId,
                    AttachmentPath = expense.AttachmentPath,
                    Status = expense.Status,
                    SubmittedBy = expense.SubmittedBy,
                    SubmittedDate = expense.SubmittedDate,
                    ApprovedBy = expense.ApprovedBy,
                    ApprovedDate = expense.ApprovedDate,
                    RejectedBy = expense.RejectedBy,
                    RejectedDate = expense.RejectedDate,
                    RejectionReason = expense.RejectionReason
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving expense: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get All Expenses
    public class GetAllExpensesQuery : IRequest<Result>
    {
        public ExpenseStatus? Status { get; set; }
        public int? BranchId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllExpensesQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllExpensesQuery, Result>
    {
        public async Task<Result> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.Expenses
                    .Include(e => e.ChartOfAccount)
                    .Include(e => e.Customer)
                    .Where(e => !e.IsDeleted);

                if (request.Status.HasValue)
                    query = query.Where(e => e.Status == request.Status);

                if (request.BranchId.HasValue)
                    query = query.Where(e => e.BranchId == request.BranchId);

                if (request.CategoryId.HasValue)
                    query = query.Where(e => e.CategoryId == request.CategoryId);

                if (request.FromDate.HasValue)
                    query = query.Where(e => e.ExpenseDate >= request.FromDate);

                if (request.ToDate.HasValue)
                    query = query.Where(e => e.ExpenseDate <= request.ToDate);

                var expenses = await query
                    .OrderByDescending(e => e.ExpenseDate)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = expenses.Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    CategoryId = e.CategoryId,
                    ClassId = e.ClassId,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    Description = e.Description,
                    CustomerId = e.CustomerId,
                    CustomerName = e.Customer?.name,
                    IsReimbursable = e.IsReimbursable,
                    ChartOfAccountId = e.ChartOfAccountId,
                    ChartOfAccountName = e.ChartOfAccount?.AccountName,
                    BranchId = e.BranchId,
                    AttachmentPath = e.AttachmentPath,
                    Status = e.Status,
                    SubmittedBy = e.SubmittedBy,
                    SubmittedDate = e.SubmittedDate,
                    ApprovedBy = e.ApprovedBy,
                    ApprovedDate = e.ApprovedDate,
                    RejectedBy = e.RejectedBy,
                    RejectedDate = e.RejectedDate,
                    RejectionReason = e.RejectionReason
                }).ToList();

                return Result.Success(new { data = dtos, page = request.Page, pageSize = request.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving expenses: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Pending Approvals
    public class GetPendingExpenseApprovalsQuery : IRequest<Result>
    {
        public int? BranchId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetPendingExpenseApprovalsQueryHandler(ERP_DbContext context) : IRequestHandler<GetPendingExpenseApprovalsQuery, Result>
    {
        public async Task<Result> Handle(GetPendingExpenseApprovalsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.Expenses
                    .Include(e => e.ChartOfAccount)
                    .Include(e => e.Customer)
                    .Where(e => !e.IsDeleted && e.Status == ExpenseStatus.Submitted);

                if (request.BranchId.HasValue)
                    query = query.Where(e => e.BranchId == request.BranchId);

                var expenses = await query
                    .OrderByDescending(e => e.SubmittedDate)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = expenses.Select(e => new ExpenseDto
                {
                    Id = e.Id,
                    CategoryId = e.CategoryId,
                    ClassId = e.ClassId,
                    Amount = e.Amount,
                    ExpenseDate = e.ExpenseDate,
                    Description = e.Description,
                    CustomerId = e.CustomerId,
                    CustomerName = e.Customer?.name,
                    IsReimbursable = e.IsReimbursable,
                    ChartOfAccountId = e.ChartOfAccountId,
                    ChartOfAccountName = e.ChartOfAccount?.AccountName,
                    BranchId = e.BranchId,
                    AttachmentPath = e.AttachmentPath,
                    Status = e.Status,
                    SubmittedBy = e.SubmittedBy,
                    SubmittedDate = e.SubmittedDate,
                    ApprovedBy = e.ApprovedBy,
                    ApprovedDate = e.ApprovedDate,
                    RejectedBy = e.RejectedBy,
                    RejectedDate = e.RejectedDate,
                    RejectionReason = e.RejectionReason
                }).ToList();

                return Result.Success(new { data = dtos, page = request.Page, pageSize = request.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving pending approvals: {ex.Message}");
            }
        }
    }
    #endregion
}
