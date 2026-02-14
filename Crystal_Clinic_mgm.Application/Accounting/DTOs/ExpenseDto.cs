using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public bool IsReimbursable { get; set; }
        public int? ChartOfAccountId { get; set; }
        public string? ChartOfAccountName { get; set; }
        public int? BranchId { get; set; }
        public List<string>? AttachmentPath { get; set; } 
        public ExpenseStatus Status { get; set; }
        public Guid? SubmittedBy { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public Guid? RejectedBy { get; set; }
        public DateTime? RejectedDate { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class CreateExpenseDto
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public bool IsReimbursable { get; set; } = false;
        public int? ChartOfAccountId { get; set; }
        public int? BranchId { get; set; }
        public List<string>? AttachmentPath { get; set; } 
    }

    public class UpdateExpenseDto
    {
        public int ExpenseId { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public bool IsReimbursable { get; set; }
        public int? ChartOfAccountId { get; set; }
        public int? BranchId { get; set; }
        public List<string>? AttachmentPath { get; set; }
    }

    public class SubmitExpenseDto
    {
        public int Id { get; set; }
    }

    public class ApproveExpenseDto
    {
        public int Id { get; set; }
    }

    public class RejectExpenseDto
    {
        public int Id { get; set; }
        public string RejectionReason { get; set; } = string.Empty;
    }
}
