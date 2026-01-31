using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look
{
    public class Item : AuditableEntity
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Asprin", "witning cream", "washing machine"
        public string Description { get; set; } = string.Empty;
        public string BaseUnit { get; set; } = string.Empty; // "count", "set", "sqm"
        public decimal CurrentStock { get; set; } // for total available stock
        public decimal? UseableStock { get; set; } // for healty stock example we have 5 washing machine only 3 is healthy
        public decimal ReorderLevel { get; set; } // when to order or purchase this item
        public string? ImagePath { get; set; } 

        // New fields for enhanced inventory management
        public string ItemCode { get; set; } = string.Empty; // Unique identifier for SKU/Stock management
        public string Barcode { get; set; } = string.Empty; // Supports scanner integration
        public bool RequiresExpiration { get; set; } // Toggles mandatory expiry date logic for medications
        public decimal UnitCost { get; set; } // Stores the Moving Average Cost (MAC) for financial valuation
        public bool IsActive { get; set; } = true; // Soft-disable items no longer used

        // IFRS/IAS Compliance fields
        public bool IsInventoryItem { get; set; } = true; // False for service items without inventory tracking
        public ValuationMethod ValuationMethod { get; set; } = ValuationMethod.WeightedAverage;
        public string CostComponents { get; set; } = "{}"; // JSON storage for detailed cost breakdown
        public DateTime? LastNRVAssessment { get; set; }
        public decimal NRVAmount { get; set; } // Net Realizable Value
        public decimal WriteDownAmount { get; set; } // Accumulated write-downs

        // Foreign Keys
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int? CategoryId { get; set; }
        public ItemCategory? Category { get; set; } // example Medicine, Machine, etc
        public int? BrandId { get; set; } // Links to Brand table
        public Brand? Brand { get; set; }
    }
}
