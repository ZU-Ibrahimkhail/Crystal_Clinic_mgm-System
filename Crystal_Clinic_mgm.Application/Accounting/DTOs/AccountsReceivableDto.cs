using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class AccountsReceivableDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public int? VisitId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public ARStatus Status { get; set; }
        public int? BranchId { get; set; }
        public double CurrencyRate { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
        public string? Attachment { get; set; } = string.Empty;
        public int? ChartOfAccountId { get; set; }
        public int? CurrencyId { get; set; }
        public List<ReceiptDto> Receipts { get; set; } = new();
    }

    public class CreateAccountsReceivableDto
    {
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int? ChartOfAccountId { get; set; }
        public int? BranchId { get; set; }
        public int? VisitId { get; set; }
        public int? CurrencyId { get; set; }
        public double  CurrencyRate { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
        public string? Attachment { get; set; } = string.Empty;
    }

    public class ReceiptDto
    {
        public int Id { get; set; }
        public int AccountsReceivableId { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
        public DateTime ReceiptDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMethodId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int? OriginalReceiptId { get; set; }
    }

    public class CreateReceiptDto
    {
        public int AccountsReceivableId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal Amount { get; set; }  // Amount received (can trigger overpayment processing)
        public int PaymentMethodId { get; set; }
        public string? Reference { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string Attachment { get; set; } = string.Empty;
    }
}
