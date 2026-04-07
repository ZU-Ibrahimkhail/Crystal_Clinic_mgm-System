using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class AccountsPayableDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int? ChartOfAccountId { get; set; }
        public string? ChartOfAccountName { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? CurrencyId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public APStatus Status { get; set; }
        public int? BranchId { get; set; }
        public double CurrencyRate { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
        public string? Attachment { get; set; }
        public List<PaymentDto> Payments { get; set; } = new();

    }

    public class CreateAccountsPayableDto
    {
        public int VendorId { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? BranchId { get; set; }
        public int? CurrencyId { get; set; }
        public int? ChartOfAccountId { get; set; }
        public APType Type { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public double CurrencyRate { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
        public string? Attachment { get; set; }
    }

    public class PaymentDto
    {
        public int Id { get; set; }
        public int AccountsPayableId { get; set; }
        public string PaymentNumber { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentMethodId { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int? CurrencyId { get; set; }
        public decimal ExchangeRate { get; set; } = 1;
        public decimal AmountInBaseCurrency { get; set; }
    }

    public class CreatePaymentDto
    {
        public int AccountsPayableId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentMethodId { get; set; }
        public string? Reference { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}
