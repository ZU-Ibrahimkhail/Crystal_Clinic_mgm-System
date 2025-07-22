using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class SupplierDue : AuditableEntity
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        //public int ItemId { get; set; }
        //public int StockId { get; set; }
        //public Stock? Stock { get; set; }
        public decimal DueAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public int CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public List<DuePayment> Payments { get; set; } = [];
    }
}
