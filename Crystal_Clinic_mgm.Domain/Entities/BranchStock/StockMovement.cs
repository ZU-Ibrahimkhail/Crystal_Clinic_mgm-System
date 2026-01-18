using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class StockMovement : AuditableEntity
    {
        public int StockMovementId { get; set; }
        public DateTime Date { get; set; }
        public MovementType MovementType { get; set; } // "In", "Out"
        public MovementReason Reason { get; set; }   // 🔄 Added (Sale/Return/Damage/Adjustment)
        public int? SourceBranchId { get; set; }     // 🔄 Added for inter-branch transfers
        public decimal Quantity { get; set; }
        public string Notes { get; set; } = string.Empty;

        // New fields for enhanced inventory management
        public int? StockId { get; set; } // Links the movement to a specific batch/lot
        public Guid? ProcessedBy { get; set; } // Links movement to the user who performed it
        public decimal UnitCost { get; set; } // Records the value of the item at the exact moment of movement
        public decimal TotalCost { get; set; } // UnitCost * Quantity (Audit record)
        public int? AdjustmentCategoryId { get; set; } // Links to adjustment category for detailed classification

        // Foreign Keys
        public int ItemId { get; set; }
        public Item? Item { get; set; }
        public Stock? Stock { get; set; }
        public AdjustmentCategory? AdjustmentCategory { get; set; }

        public int? OrderId { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
    }
}
