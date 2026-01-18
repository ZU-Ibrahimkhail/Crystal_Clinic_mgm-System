using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class SalesEstimate : AuditableEntity
    {
        public int Id { get; set; }
        public string EstimateNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public DateTime EstimateDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public EstimateStatus Status { get; set; } = EstimateStatus.Draft;
        public string Notes { get; set; } = string.Empty;
        public int? ConvertedToInvoiceId { get; set; }
        public SalesInvoice? ConvertedToInvoice { get; set; }
        public int? BranchId { get; set; }

        // Navigation properties
        public ICollection<SalesEstimateLine> EstimateLines { get; set; } = new List<SalesEstimateLine>();
    }

    public enum EstimateStatus
    {
        Draft,
        Sent,
        Accepted,
        Rejected,
        Expired,
        Converted
    }
}