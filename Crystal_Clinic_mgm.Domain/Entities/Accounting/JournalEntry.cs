using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class JournalEntry : AuditableEntity
    {
        public int Id { get; set; }
        public string EntryNumber { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public JournalEntryStatus Status { get; set; } = JournalEntryStatus.Draft;
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string ReferenceType { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string? Attachment { get; set; } = string.Empty;
        
        public int? EquityTransactionId { get; set; }
        public int? ExpenseId { get; set; }
        public int? PaymentId { get; set; }
        public int? SalesReceiptId { get; set; }
        public int? ReceiptId { get; set; }
        
        public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
        public ICollection<GeneralLedger> GeneralLedgerEntries { get; set; } = new List<GeneralLedger>();
    }
}
