using MediatR;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting.Events
{
    public class SalesEstimateConvertedEvent : INotification
    {
        public int EstimateId { get; set; }
        public string EstimateNumber { get; set; } = string.Empty;
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public DateTime ConvertedAt { get; set; } = DateTime.UtcNow;
    }

    public class SalesInvoicePaidEvent : INotification
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RemainingBalance { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    }
}