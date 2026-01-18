using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public class ReservationCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReservationCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5);

        public ReservationCleanupService(
            IServiceProvider serviceProvider,
            ILogger<ReservationCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reservation cleanup service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupExpiredReservations(stoppingToken);
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during reservation cleanup");
                    // Continue running despite errors
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
            }

            _logger.LogInformation("Reservation cleanup service stopped");
        }

        private async Task CleanupExpiredReservations(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ERP_DbContext>();
            var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();

            var expiredReservations = await context.Set<InventoryReservation>()
                .Where(r => r.Status == ReservationStatus.Active &&
                           r.ExpiresAt < DateTime.UtcNow &&
                           !r.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!expiredReservations.Any())
            {
                return;
            }

            _logger.LogInformation("Found {Count} expired reservations to clean up", expiredReservations.Count);

            foreach (var reservation in expiredReservations)
            {
                try
                {
                    var result = await inventoryService.ReleaseReservationAsync(reservation.Id, cancellationToken);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Successfully released expired reservation {ReservationId}", reservation.Id);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to release expired reservation {ReservationId}: {Error}",
                            reservation.Id, result.Error);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error releasing expired reservation {ReservationId}", reservation.Id);
                }
            }
        }
    }
}