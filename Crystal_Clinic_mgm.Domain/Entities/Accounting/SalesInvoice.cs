using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class SalesInvoice : AuditableEntity
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public Patient? Customer { get; set; }
        public int? VisitId { get; set; }
        public Visit? Visit { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public SalesStatus Status { get; set; } = SalesStatus.Draft;
        public string SalesArea { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string? Notes { get; set; }
        public string? Attachment { get; set; } = string.Empty;
        public ICollection<SalesInvoiceLine> Lines { get; set; } = new List<SalesInvoiceLine>();
        public ICollection<SalesReceipt> Receipts { get; set; } = new List<SalesReceipt>();
    }
}
