using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class AccountsReceivable : AuditableEntity
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public Patient? Customer { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public decimal BalanceAmount { get; set; }
        public ARStatus Status { get; set; } = ARStatus.Draft;
        public int? ChartOfAccountId { get; set; }
        public ChartOfAccounts? ChartOfAccount { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int? CurrencyId { get; set; }
        public CurrencyType? Currency { get; set; }
        public int? VisitId { get; set; }
        public Visit? Visit { get; set; }
        public double CurrencyRate { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
        public string? Attachment { get; set; } = string.Empty;
        public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
    }
}
