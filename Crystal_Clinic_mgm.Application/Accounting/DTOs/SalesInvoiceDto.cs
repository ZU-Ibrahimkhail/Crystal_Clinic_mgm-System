using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class SalesInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public SalesStatus Status { get; set; }
        public string SalesArea { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public string? Attachment { get; set; }

    }

    public class CreateSalesInvoiceDto
    {
        public int CustomerId { get; set; }
        public int? PatientId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string SalesArea { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? Notes { get; set; }
        public string? Attachment { get; set; }
        public List<SalesInvoiceLineDto> Lines { get; set; } = new();
    }

    public class UpdateSalesInvoiceDto
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string SalesArea { get; set; } = string.Empty;
        public List<SalesInvoiceLineDto> Lines { get; set; } = new();
    }

    public class SalesInvoiceLineDto
    {
        //public int Id { get; set; } The Id should be created automatically
        public int? ItemId { get; set; }
        public int? ServiceId { get; set; }
        public int? InventoryItemId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
    }

    public class SalesReceiptDto
    {
        public int Id { get; set; }
        public int SalesInvoiceId { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal AmountReceived { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string Reference { get; set; } = string.Empty;
    }

    public class CreateSalesReceiptDto
    {
        public int SalesInvoiceId { get; set; }
        public decimal AmountReceived { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string? Reference { get; set; }
    }
}
