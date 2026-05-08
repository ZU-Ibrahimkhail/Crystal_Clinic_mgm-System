namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class POLine : AuditableEntity
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public int ItemId { get; set; }
        public string ItemDescription { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal ReceivedQuantity { get; set; } = 0;
        public DateTime? ItemExpiry { get; set; }
        public string? BarCode { get; set; } = string.Empty;
        public decimal? ExpectedSalePrice { get; set; }
    }
}
