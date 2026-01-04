using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class AccountsPayable : AuditableEntity
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public Supplier? Vendor { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public decimal BalanceAmount { get; set; }
        public APStatus Status { get; set; } = APStatus.Open;
        public int? ChartOfAccountId { get; set; }
        public ChartOfAccounts? ChartOfAccount { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int? CurrencyId { get; set; }
        public CurrencyType? Currency { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
