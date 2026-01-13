using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class Receipt : AuditableEntity
    {
        public int Id { get; set; }
        public int AccountsReceivableId { get; set; }
        public AccountsReceivable AccountsReceivable { get; set; } = null!;
        public string ReceiptNumber { get; set; } = string.Empty;
        public DateTime ReceiptDate { get; set; }

        // Transaction Type
        public TransactionType TransactionType { get; set; } = TransactionType.Receipt;

        // Amount (always positive, context determines direction)
        public decimal Amount { get; set; }

        public int PaymentMethodId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int? CurrencyId { get; set; }
        public CurrencyType? Currency { get; set; }
        public decimal ExchangeRate { get; set; } = 1;
        public decimal AmountInBaseCurrency { get; set; }

        // Reference to original receipt (for refunds only)
        public int? OriginalReceiptId { get; set; }
        public Receipt? OriginalReceipt { get; set; }
    }
}
