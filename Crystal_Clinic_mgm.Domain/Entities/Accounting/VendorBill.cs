using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class VendorBill : AuditableEntity
    {
        public int Id { get; set; }
        public int? PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public int? paymentId { get; set; }
        public Payment? Payment { get; set; }
        public int VendorId { get; set; }
        public Supplier? Vendor { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string BillNumber { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public BillStatus Status { get; set; } = BillStatus.Unpaid;
    }
}
