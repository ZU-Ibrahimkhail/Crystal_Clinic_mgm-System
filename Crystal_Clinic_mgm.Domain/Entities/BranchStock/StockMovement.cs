using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class StockMovement
    {
        public int StockMovementId { get; set; }
        public DateTime Date { get; set; }
        public MovementType MovementType { get; set; } // "In", "Out"
        public MovementReason Reason { get; set; }   // 🔄 Added (Sale/Return/Damage/Adjustment)
        public int? SourceBranchId { get; set; }     // 🔄 Added for inter-branch transfers
        public decimal Quantity { get; set; }
        public string Notes { get; set; } = string.Empty;

        // Foreign Keys
        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public int? OrderId { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
    }
}
