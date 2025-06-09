using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace Crystal_Clinic_Mgm.Application.Common.Jobs
{


    public class DailyJob : BackgroundService
    {
        private readonly ILogger<DailyJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _scheduledTime = new(1, 0, 0);

        public DailyJob(ILogger<DailyJob> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Run immediately on startup (first execution)
            await ExecuteDailyTask(stoppingToken);

            // Calculate next run time (today at 2 AM or tomorrow if already passed)
            var nextRun = DateTime.Now.Add(_scheduledTime);
            if (nextRun < DateTime.Now)
                nextRun = nextRun.AddHours(1);

            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = nextRun - DateTime.Now;
                if (delay > TimeSpan.Zero)
                {
                    _logger.LogInformation($"Next execution at {nextRun}");
                    await Task.Delay(delay, stoppingToken);
                }

                if (DateTime.Now >= nextRun)
                {
                    await ExecuteDailyTask(stoppingToken);
                    nextRun = nextRun.AddHours(1);
                }
            }
        }

        private async Task ExecuteDailyTask(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ERP_DbContext>();

            _logger.LogInformation($"Daily task executed at: {DateTime.Now}");
            try
            {

                var jobs = await dbContext.ItemCleaningJob
                    .Where(x => x.dateToBeRestocked <= DateTime.Now.Date)
                    .Include(x => x.item)
                    .Include(x => x.unit)
                    .ToListAsync(stoppingToken);
                var executionStrategy = dbContext.Database.CreateExecutionStrategy();

                await executionStrategy.ExecuteAsync(async () =>
                {
                    foreach (var job in jobs)
                    {
                        if (job.item != null)
                        {
                            var baseQuantity = job.unit!.ConversionFactor * job.quantity;
                            if (baseQuantity > job.item.CleaningStateQuantity)
                                baseQuantity = job.item.CleaningStateQuantity;

                            job.item.CleaningStateQuantity -= baseQuantity;
                            job.item.RealTimeAvailableStock += baseQuantity;

                            dbContext.Entry(job.item).Property(x => x.CleaningStateQuantity).IsModified = true;
                            dbContext.Entry(job.item).Property(x => x.RealTimeAvailableStock).IsModified = true;
                        }
                        dbContext.ItemCleaningJob.Remove(job);
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("an exception happens during the job:{}", ex);
            }

        }
    }
}
