namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class ChartOfAccounts : AuditableEntity
    {
        public int Id { get; set; }
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
        public AccountCategory AccountCategory { get; set; }
        public NormalBalanceType NormalBalance { get; set; }
        public bool IsSystemAccount { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public string Description { get; set; } = string.Empty;
        public int? ParentAccountId { get; set; }
        public ChartOfAccounts? ParentAccount { get; set; }
        public ICollection<ChartOfAccounts> ChildAccounts { get; set; } = new List<ChartOfAccounts>();
        public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
        public ICollection<GeneralLedger> GeneralLedgerEntries { get; set; } = new List<GeneralLedger>();
    }
}
