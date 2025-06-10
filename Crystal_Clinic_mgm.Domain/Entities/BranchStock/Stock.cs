using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class Stock : AuditableEntity
    {
        public int stockId { get; set; }
        public int quantity { get; set; }
        public int itemId { get; set; }
        public int BranchId { get; set; }
        public Item? item { get; set; }
        public decimal purchasePrice { get; set; }
        public decimal sellPrice { get; set; }
        public DateTime purchaseDate { get; set; }
        public string batchNumber { get; set; } = string.Empty;
        public string barCode { get; set; } = string.Empty;
        public DateTime expiryDate { get; set; }
    }
}
