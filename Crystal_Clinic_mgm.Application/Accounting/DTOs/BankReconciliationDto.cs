using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class BankStatementImportDto
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
        public BankImportStatus Status { get; set; }
        public List<BankStatementLineDto> Lines { get; set; } = new();
    }

    public class CreateBankStatementImportDto
    {
        public string FileName { get; set; } = string.Empty;
        public int BankAccountId { get; set; }
        public DateTime StatementPeriodStart { get; set; }
        public DateTime StatementPeriodEnd { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public string FileFormat { get; set; } = string.Empty;
        public List<CreateBankStatementLineDto> Lines { get; set; } = new();
    }

    public class BankStatementLineDto
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal RunningBalance { get; set; }
        public string? BankCode { get; set; }
        public bool IsMatched { get; set; }
    }

    public class CreateBankStatementLineDto
    {
        public DateTime TransactionDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal RunningBalance { get; set; }
        public string? BankCode { get; set; }
    }

    public class BankMatchDto
    {
        public int Id { get; set; }
        public int BankStatementLineId { get; set; }
        public int? GeneralLedgerId { get; set; }
        public int? JournalEntryId { get; set; }
        public decimal MatchedAmount { get; set; }
        public DateTime MatchDate { get; set; }
        public BankMatchStatus Status { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateBankMatchDto
    {
        public int BankStatementImportId { get; set; }
        public int BankStatementLineId { get; set; }
        public int? GeneralLedgerId { get; set; }
        public int? JournalEntryId { get; set; }
        public decimal MatchedAmount { get; set; }
        public string? Notes { get; set; }
    }

    public class BankReconciliationSummaryDto
    {
        public int ImportId { get; set; }
        public decimal StatementClosingBalance { get; set; }
        public decimal BookBalance { get; set; }
        public decimal Difference { get; set; }
        public int MatchedCount { get; set; }
        public int UnmatchedCount { get; set; }
        public DateTime AsOfDate { get; set; }
    }
}
