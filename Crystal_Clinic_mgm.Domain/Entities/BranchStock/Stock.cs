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
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public decimal purchasePrice { get; set; }
        public decimal sellPrice { get; set; }
        public DateTime purchaseDate { get; set; }
        public string batchNumber { get; set; } = string.Empty;
        public string barCode { get; set; } = string.Empty;
        public DateTime expiryDate { get; set; }

        // New fields for enhanced inventory management
        public string LotNumber { get; set; } = string.Empty; // Mandatory for medical batch tracking and recalls
        public DateTime? ManufactureDate { get; set; } // Essential for shelf-life analysis
        public bool IsExpired { get; set; } // Indexed flag for fast filtering of unusable stock
        public decimal QuantityRemaining { get; set; } // Tracks usable stock separately from total quantity

        // IFRS/IAS Compliance fields
        public int? SiteId { get; set; } // Links to InventorySite
        public int? PurchaseOrderId { get; set; }
        public int? InvoiceId { get; set; }
        public decimal FreightCost { get; set; } // Transportation costs
        public decimal InsuranceCost { get; set; } // Insurance during transit
        public decimal ImportDuty { get; set; } // Customs duties
        public decimal OtherLandingCosts { get; set; } // Other costs to bring to location

        // Navigation properties
        public InventorySite? Site { get; set; }
    }
}
