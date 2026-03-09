using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class PurchaseOrder : AuditableEntity
    {
        public int Id { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public Supplier? Vendor { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public POStatus Status { get; set; } = POStatus.Open;
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string? Attachment { get; set; }
        public ICollection<POLine> Lines { get; set; } = new List<POLine>();
    }
}
