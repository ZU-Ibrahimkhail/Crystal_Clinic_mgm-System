using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class SalesReceipt : AuditableEntity
    {
        public int Id { get; set; }
        public int SalesInvoiceId { get; set; }
        public SalesInvoice SalesInvoice { get; set; } = null!;
        public string ReceiptNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public Patient? Customer { get; set; }
        public decimal AmountReceived { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string Reference { get; set; } = string.Empty;
    }
}
