namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class BankStatementLine : AuditableEntity
    {
        public int Id { get; set; }
        public int BankStatementImportId { get; set; }
        public BankStatementImport BankStatementImport { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal RunningBalance { get; set; }
        public string? BankCode { get; set; }
        public bool IsMatched { get; set; } = false;

        public ICollection<BankMatch> Matches { get; set; } = new List<BankMatch>();
    }
}
