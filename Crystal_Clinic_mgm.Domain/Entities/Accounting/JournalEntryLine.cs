using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class JournalEntryLine : AuditableEntity
    {
        public int Id { get; set; }
        public int JournalEntryId { get; set; }
        public JournalEntry JournalEntry { get; set; } = null!;
        public int ChartOfAccountId { get; set; }
        public ChartOfAccounts ChartOfAccount { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; } = 0;
        public decimal CreditAmount { get; set; } = 0;
        public int? CurrencyId { get; set; }
        public CurrencyType? Currency { get; set; }
        public decimal ExchangeRate { get; set; } = 1;
        public decimal AmountInBaseCurrency { get; set; } = 0;
        
        public int? EquityTransactionId { get; set; }
        public int? ExpenseId { get; set; }
        public int? PaymentId { get; set; }
        public int? SalesReceiptId { get; set; }
        public int? ReceiptId { get; set; }
    }
}
