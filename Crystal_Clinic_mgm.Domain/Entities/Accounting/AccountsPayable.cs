using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class AccountsPayable : AuditableEntity
    {
        public int Id { get; set; }
        public int VendorBillId { get; set; }
        public VendorBill? VendorBill { get; set; }
        public int? CustomerId { get; set; }
        public Patient? Customer { get; set; }
        public int? PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public int VendorId { get; set; }
        public Supplier? Vendor { get; set; }
        public int? CurrencyId { get; set; }
        public CurrencyType? Currency { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int? ChartOfAccountId { get; set; }
        public ChartOfAccounts? ChartOfAccount { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public APType Type { get; set; }
        public APStatus Status { get; set; } = APStatus.Draft;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public decimal BalanceAmount { get; set; }
        public double CurrencyRate { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
        public string? Attachment { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
