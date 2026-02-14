using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Expense
    public class CreateExpenseCommand : IRequest<Result>
    {
        public CreateExpenseDto Dto { get; set; } = null!;
        public Guid UserId { get; set; }
    }

    public class CreateExpenseCommandHandler(ERP_DbContext context) : IRequestHandler<CreateExpenseCommand, Result>
    {
        public async Task<Result> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var expense = new Expense
                {
                    CategoryId = request.Dto.CategoryId,
                    Amount = request.Dto.Amount,
                    ExpenseDate = request.Dto.ExpenseDate,
                    Description = request.Dto.Description,
                    CustomerId = request.Dto.CustomerId,
                    IsReimbursable = request.Dto.IsReimbursable,
                    ChartOfAccountId = request.Dto.ChartOfAccountId,
                    BranchId = request.Dto.BranchId,
                    AttachmentPath = request.Dto.AttachmentPath,
                    Status = ExpenseStatus.Draft,
                    CreatedBy = request.UserId
                };

                context.Expenses.Add(expense);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = expense.Id, status = ExpenseStatus.Draft });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating expense: {ex.Message}");
            }
        }
    }
    #endregion

    #region Update Expense
    public class UpdateExpenseCommand : IRequest<Result>
    {
        public UpdateExpenseDto Dto { get; set; } = null!;
    }

    public class UpdateExpenseCommandHandler(ERP_DbContext context) : IRequestHandler<UpdateExpenseCommand, Result>
    {
        public async Task<Result> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var expense = await context.Expenses
                    .FirstOrDefaultAsync(e => e.Id == request.Dto.Id && !e.IsDeleted, cancellationToken);

                if (expense == null)
                    return Result.Fail("Expense not found");

                if (expense.Status != ExpenseStatus.Draft)
                    return Result.Fail("Only draft expenses can be updated");

                expense.CategoryId = request.Dto.CategoryId;
                expense.Amount = request.Dto.Amount;
                expense.ExpenseDate = request.Dto.ExpenseDate;
                expense.Description = request.Dto.Description;
                expense.CustomerId = request.Dto.CustomerId;
                expense.IsReimbursable = request.Dto.IsReimbursable;
                expense.ChartOfAccountId = request.Dto.ChartOfAccountId;
                expense.BranchId = request.Dto.BranchId;
                expense.AttachmentPath = request.Dto.AttachmentPath;

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = expense.Id, status = expense.Status });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating expense: {ex.Message}");
            }
        }
    }
    #endregion

    #region Submit Expense
    public class SubmitExpenseCommand : IRequest<Result>
    {
        public int ExpenseId { get; set; }
        public Guid SubmittedBy { get; set; }
    }

    public class SubmitExpenseCommandHandler(ERP_DbContext context) : IRequestHandler<SubmitExpenseCommand, Result>
    {
        public async Task<Result> Handle(SubmitExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var expense = await context.Expenses
                    .FirstOrDefaultAsync(e => e.Id == request.ExpenseId && !e.IsDeleted, cancellationToken);

                if (expense == null)
                    return Result.Fail("Expense not found");

                if (expense.Status != ExpenseStatus.Draft)
                    return Result.Fail("Only draft expenses can be submitted");

                expense.Status = ExpenseStatus.Submitted;
                expense.SubmittedBy = request.SubmittedBy;
                expense.SubmittedDate = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = expense.Id, status = ExpenseStatus.Submitted });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error submitting expense: {ex.Message}");
            }
        }
    }
    #endregion

    #region Approve Expense
    public class ApproveExpenseCommand : IRequest<Result>
    {
        public int ExpenseId { get; set; }
        public Guid ApprovedBy { get; set; }
    }

    public class ApproveExpenseCommandHandler(ERP_DbContext context) : IRequestHandler<ApproveExpenseCommand, Result>
    {
        public async Task<Result> Handle(ApproveExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var expense = await context.Expenses
                    .FirstOrDefaultAsync(e => e.Id == request.ExpenseId && !e.IsDeleted, cancellationToken);

                if (expense == null)
                    return Result.Fail("Expense not found");

                if (expense.Status != ExpenseStatus.Submitted)
                    return Result.Fail("Only submitted expenses can be approved");

                expense.Status = ExpenseStatus.Approved;
                expense.ApprovedBy = request.ApprovedBy;
                expense.ApprovedDate = DateTime.UtcNow;

                if (expense.ChartOfAccountId.HasValue)
                {
                    var journalEntry = new JournalEntry
                    {
                        Description = $"Expense Approval: {expense.Description}",
                        Status = JournalEntryStatus.Posted,
                        EntryDate = DateTime.UtcNow
                    };

                    var debitLine = new JournalEntryLine
                    {
                        ChartOfAccountId = expense.ChartOfAccountId.Value,
                        DebitAmount = expense.Amount,
                        CreditAmount = 0,
                        Description = expense.Description
                    };
                    journalEntry.JournalEntryLines.Add(debitLine);

                    // Get cash account from company profile
                    var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                    if (companyProfile == null)
                        return Result.Fail("Company profile not found. Please initialize the company profile first.");

                    var creditLine = new JournalEntryLine
                    {
                        ChartOfAccountId = companyProfile.CashAccountId,
                        DebitAmount = 0,
                        CreditAmount = expense.Amount,
                        Description = expense.Description
                    };
                    journalEntry.JournalEntryLines.Add(creditLine);

                    context.JournalEntries.Add(journalEntry);
                }

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = expense.Id, status = ExpenseStatus.Approved });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error approving expense: {ex.Message}");
            }
        }
    }
    #endregion

    #region Reject Expense
    public class RejectExpenseCommand : IRequest<Result>
    {
        public int ExpenseId { get; set; }
        public Guid RejectedBy { get; set; }
        public string RejectionReason { get; set; } = string.Empty;
    }

    public class RejectExpenseCommandHandler(ERP_DbContext context) : IRequestHandler<RejectExpenseCommand, Result>
    {
        public async Task<Result> Handle(RejectExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var expense = await context.Expenses
                    .FirstOrDefaultAsync(e => e.Id == request.ExpenseId && !e.IsDeleted, cancellationToken);

                if (expense == null)
                    return Result.Fail("Expense not found");

                if (expense.Status != ExpenseStatus.Submitted)
                    return Result.Fail("Only submitted expenses can be rejected");

                expense.Status = ExpenseStatus.Rejected;
                expense.RejectedBy = request.RejectedBy;
                expense.RejectedDate = DateTime.UtcNow;
                expense.RejectionReason = request.RejectionReason;

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = expense.Id, status = ExpenseStatus.Rejected, reason = request.RejectionReason });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error rejecting expense: {ex.Message}");
            }
        }
    }
    #endregion
}
