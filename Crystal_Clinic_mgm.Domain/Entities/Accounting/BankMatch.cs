namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class BankMatch : AuditableEntity
    {
        public int Id { get; set; }
        public int BankStatementImportId { get; set; }
        public BankStatementImport BankStatementImport { get; set; } = null!;
        public int BankStatementLineId { get; set; }
        public BankStatementLine BankStatementLine { get; set; } = null!;
        public int? GeneralLedgerId { get; set; }
        public GeneralLedger? GeneralLedger { get; set; }
        public int? JournalEntryId { get; set; }
        public JournalEntry? JournalEntry { get; set; }
        public decimal MatchedAmount { get; set; }
        public DateTime MatchDate { get; set; }
        public BankMatchStatus Status { get; set; } = BankMatchStatus.Proposed;
        public string? Notes { get; set; }
        public int? ReviewedBy { get; set; }
    }

    public enum BankMatchStatus
    {
        Proposed,
        Approved,
        Rejected,
        Adjusted
    }
}
