using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class Expense : AuditableEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int? ClassId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public Patient? Customer { get; set; }
        public bool IsReimbursable { get; set; } = false;
        public int? ChartOfAccountId { get; set; }
        public ChartOfAccounts? ChartOfAccount { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string AttachmentPath { get; set; } = string.Empty;
        public ExpenseStatus Status { get; set; } = ExpenseStatus.Draft;
        public Guid? SubmittedBy { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public Guid? RejectedBy { get; set; }
        public DateTime? RejectedDate { get; set; }
        public string? RejectionReason { get; set; }
    }

    public enum ExpenseStatus
    {
        Draft,
        Submitted,
        Approved,
        Rejected,
        Paid
    }
}
