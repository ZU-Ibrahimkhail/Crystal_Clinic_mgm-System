using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public interface IValuationService
    {
        /// <summary>
        /// Updates the moving average cost for an item after a stock movement
        /// </summary>
        Task UpdateMovingAverageCostAsync(int itemId, decimal newCost, decimal quantity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the current moving average cost for an item
        /// </summary>
        Task<decimal> GetMovingAverageCostAsync(int itemId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Recalculates moving average cost for all items (maintenance operation)
        /// </summary>
        Task RecalculateAllMovingAverageCostsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets inventory valuation report
        /// </summary>
        Task<InventoryValuation> GetInventoryValuationAsync(int? branchId = null, CancellationToken cancellationToken = default);
    }

    public class InventoryValuation
    {
        public decimal TotalValue { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<ItemValuation> ItemValuations { get; set; } = new List<ItemValuation>();
    }

    public class ItemValuation
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal TotalQuantity { get; set; }
        public decimal MovingAverageCost { get; set; }
        public decimal TotalValue { get; set; }
        public IEnumerable<StockValuation> StockValuations { get; set; } = new List<StockValuation>();
    }

    public class StockValuation
    {
        public int StockId { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
    }
}