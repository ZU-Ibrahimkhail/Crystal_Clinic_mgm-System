using MediatR;
using Crystal_Clinic_Mgm.Domain;

namespace Crystal_Clinic_Mgm.Application.Events
{
    public class InventoryStockAdjustedEvent : INotification
    {
        public int ItemId { get; set; }
        public int? StockId { get; set; }
        public decimal Quantity { get; set; }
        public MovementType MovementType { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class InventoryKitConsumedEvent : INotification
    {
        public int KitId { get; set; }
        public string KitName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
        public IEnumerable<ConsumedItem> ConsumedItems { get; set; } = new List<ConsumedItem>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class InventoryStockExpiredEvent : INotification
    {
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;
        public decimal ExpiredQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
    }

    public class ConsumedItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}