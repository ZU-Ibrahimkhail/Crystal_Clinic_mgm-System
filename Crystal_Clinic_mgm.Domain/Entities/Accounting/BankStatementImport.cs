namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class BankStatementImport : AuditableEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public int BankAccountId { get; set; }
        public DateTime StatementPeriodStart { get; set; }
        public DateTime StatementPeriodEnd { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public int TotalTransactions { get; set; }
        public string FileFormat { get; set; } = string.Empty;
        public DateTime ImportDate { get; set; }
        public BankImportStatus Status { get; set; } = BankImportStatus.Pending;
        public string? ErrorMessage { get; set; }

        public ICollection<BankStatementLine> Lines { get; set; } = new List<BankStatementLine>();
        public ICollection<BankMatch> Matches { get; set; } = new List<BankMatch>();
    }

    public enum BankImportStatus
    {
        Pending,
        Processing,
        Completed,
        Failed
    }
}
