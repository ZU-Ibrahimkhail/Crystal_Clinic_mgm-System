using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public class ValuationService : IValuationService
    {
        private readonly ERP_DbContext _context;
        private readonly ILogger<ValuationService> _logger;

        public ValuationService(
            ERP_DbContext context,
            ILogger<ValuationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task UpdateMovingAverageCostAsync(int itemId, decimal newCost, decimal quantity, CancellationToken cancellationToken = default)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                _logger.LogWarning("Item {ItemId} not found for cost update", itemId);
                return;
            }

            // Get current total value and quantity
            var currentStockValue = await GetCurrentStockValueAsync(itemId, cancellationToken);
            var currentQuantity = await GetCurrentStockQuantityAsync(itemId, cancellationToken);

            // Calculate new moving average cost
            decimal newTotalValue = currentStockValue + (newCost * quantity);
            decimal newTotalQuantity = currentQuantity + quantity;

            if (newTotalQuantity > 0)
            {
                item.UnitCost = newTotalValue / newTotalQuantity;
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Updated moving average cost for item {ItemId}: {NewCost}",
                    itemId, item.UnitCost);
            }
        }

        public async Task<decimal> GetMovingAverageCostAsync(int itemId, CancellationToken cancellationToken = default)
        {
            var item = await _context.Items.FindAsync(itemId);
            return item?.UnitCost ?? 0;
        }

        public async Task RecalculateAllMovingAverageCostsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting recalculation of all moving average costs");

            var items = await _context.Items
                .Where(i => !i.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                try
                {
                    var stockValue = await GetCurrentStockValueAsync(item.ItemId, cancellationToken);
                    var stockQuantity = await GetCurrentStockQuantityAsync(item.ItemId, cancellationToken);

                    if (stockQuantity > 0)
                    {
                        item.UnitCost = stockValue / stockQuantity;
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        item.UnitCost = 0;
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error recalculating cost for item {ItemId}", item.ItemId);
                }
            }

            _logger.LogInformation("Completed recalculation of moving average costs for {Count} items", items.Count);
        }

        public async Task<InventoryValuation> GetInventoryValuationAsync(int? branchId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Items
                .Where(i => !i.IsDeleted && i.IsActive);

            if (branchId.HasValue)
            {
                query = query.Where(i => i.BranchId == branchId.Value);
            }

            var items = await query
                .Include(i => i.Category)
                .ToListAsync(cancellationToken);

            var itemValuations = new List<ItemValuation>();
            decimal totalValue = 0;
            int totalItems = 0;

            foreach (var item in items)
            {
                var stockValuations = await GetStockValuationsAsync(item.ItemId, cancellationToken);
                var itemTotalValue = stockValuations.Sum(sv => sv.TotalValue);
                var itemTotalQuantity = stockValuations.Sum(sv => sv.Quantity);

                if (itemTotalQuantity > 0)
                {
                    itemValuations.Add(new ItemValuation
                    {
                        ItemId = item.ItemId,
                        ItemName = item.Name,
                        TotalQuantity = itemTotalQuantity,
                        MovingAverageCost = item.UnitCost,
                        TotalValue = itemTotalValue,
                        StockValuations = stockValuations
                    });

                    totalValue += itemTotalValue;
                    totalItems++;
                }
            }

            return new InventoryValuation
            {
                TotalValue = totalValue,
                TotalItems = totalItems,
                ItemValuations = itemValuations.OrderByDescending(iv => iv.TotalValue)
            };
        }

        private async Task<decimal> GetCurrentStockValueAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.Stocks
                .Where(s => s.itemId == itemId && !s.IsDeleted)
                .SumAsync(s => s.quantity * s.purchasePrice, cancellationToken);
        }

        private async Task<decimal> GetCurrentStockQuantityAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.Stocks
                .Where(s => s.itemId == itemId && !s.IsDeleted)
                .SumAsync(s => s.quantity, cancellationToken);
        }

        private async Task<IEnumerable<StockValuation>> GetStockValuationsAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.Stocks
                .Where(s => s.itemId == itemId && !s.IsDeleted)
                .Select(s => new StockValuation
                {
                    StockId = s.stockId,
                    LotNumber = s.LotNumber,
                    Quantity = s.quantity,
                    UnitCost = s.purchasePrice,
                    TotalValue = s.quantity * s.purchasePrice
                })
                .ToListAsync(cancellationToken);
        }
    }
}