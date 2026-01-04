using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class GeneralLedger : AuditableEntity
    {
        public int Id { get; set; }
        public int ChartOfAccountId { get; set; }
        public ChartOfAccounts ChartOfAccount { get; set; } = null!;
        public int JournalEntryId { get; set; }
        public JournalEntry JournalEntry { get; set; } = null!;
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; } = 0;
        public decimal CreditAmount { get; set; } = 0;
        public decimal Balance { get; set; } = 0;
    }
}
