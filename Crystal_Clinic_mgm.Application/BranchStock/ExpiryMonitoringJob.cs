using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Application.Events;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public class ExpiryMonitoringJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiryMonitoringJob> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Run daily

        public ExpiryMonitoringJob(
            IServiceProvider serviceProvider,
            ILogger<ExpiryMonitoringJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Expiry monitoring job started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckForExpiredStock(stoppingToken);
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during expiry monitoring");
                    // Continue running despite errors
                    await Task.Delay(_checkInterval, stoppingToken);
                }
            }

            _logger.LogInformation("Expiry monitoring job stopped");
        }

        private async Task CheckForExpiredStock(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ERP_DbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var now = DateTime.UtcNow;

            // Find stock that has expired but is not yet marked as expired
            var newlyExpiredStock = await context.Set<Stock>()
                .Where(s => !s.IsDeleted &&
                           !s.IsExpired &&
                           s.ExpiryDate < now &&
                           s.QuantityRemaining > 0)
                .Include(s => s.Item)
                .ToListAsync(cancellationToken);

            if (!newlyExpiredStock.Any())
            {
                _logger.LogInformation("No newly expired stock found");
                return;
            }

            _logger.LogWarning("Found {Count} newly expired stock items", newlyExpiredStock.Count);

            foreach (var stock in newlyExpiredStock)
            {
                try
                {
                    // Mark as expired
                    stock.IsExpired = true;
                    stock.ModifiedOn = now;

                    // Publish expiry event
                    var expiryEvent = new InventoryStockExpiredEvent
                    {
                        StockId = stock.StockId,
                        ItemId = stock.ItemId ?? 0,
                        ItemName = stock.Item?.Name ?? "Unknown",
                        LotNumber = stock.LotNumber,
                        ExpiredQuantity = stock.QuantityRemaining,
                        UnitCost = stock.PurchasePrice,
                        TotalValue = stock.QuantityRemaining * stock.PurchasePrice,
                        ExpiryDate = stock.ExpiryDate
                    };

                    await mediator.Publish(expiryEvent, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    _logger.LogWarning("Marked stock {StockId} as expired: {ItemName}, Lot: {LotNumber}",
                        stock.StockId, stock.Item?.Name, stock.LotNumber);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing expired stock {StockId}", stock.StockId);
                }
            }

            // Also check for stock expiring soon (within 30 days) for alerts
            var soonToExpireStock = await context.Set<Stock>()
                .Where(s => !s.IsDeleted &&
                           !s.IsExpired &&
                           s.ExpiryDate >= now &&
                           s.ExpiryDate <= now.AddDays(30) &&
                           s.QuantityRemaining > 0)
                .Include(s => s.Item)
                .ToListAsync(cancellationToken);

            if (soonToExpireStock.Any())
            {
                _logger.LogInformation($"Found {soonToExpireStock.Count()} stock items expiring within 30 days");

                // Here you could publish alerts or notifications
                // For now, just log them
                foreach (var stock in soonToExpireStock)
                {
                    var daysUntilExpiry = (stock.ExpiryDate - now).TotalDays;
                    _logger.LogWarning("Stock expiring soon: {ItemName}, Lot: {LotNumber}, Days left: {Days:F1}",
                        stock.Item?.Name, stock.LotNumber, daysUntilExpiry);
                }
            }
        }
    }
}