using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class Stock : AuditableEntity
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public int? ItemId { get; set; }
        public Item? Item { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public string BarCode { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }

        // New fields for enhanced inventory management
        public string LotNumber { get; set; } = string.Empty; // Mandatory for medical batch tracking and recalls
        public DateTime? ManufactureDate { get; set; } // Essential for shelf-life analysis
        public bool IsExpired { get; set; } // Indexed flag for fast filtering of unusable stock
        public decimal QuantityRemaining { get; set; } // Tracks usable stock separately from total quantity

        // IFRS/IAS Compliance fields
        public int? SiteId { get; set; } // Links to InventorySite
        public InventorySite? Site { get; set; }
        public int? PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public int? InvoiceId { get; set; }
        public decimal FreightCost { get; set; } // Transportation costs
        public decimal InsuranceCost { get; set; } // Insurance during transit
        public decimal ImportDuty { get; set; } // Customs duties
        public decimal OtherLandingCosts { get; set; } // Other costs to bring to location

        // Navigation properties
    }
}
