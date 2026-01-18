using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Forecast
    public class CreateForecastCommand : IRequest<Result>
    {
        public CreateForecastSnapshotDto Dto { get; set; } = null!;
        public Guid CreatedByUserId { get; set; }
    }

    public class CreateForecastCommandHandler(ERP_DbContext context) : IRequestHandler<CreateForecastCommand, Result>
    {
        public async Task<Result> Handle(CreateForecastCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var forecast = new ForecastSnapshot
                {
                    Name = request.Dto.Name,
                    Scenario = request.Dto.Scenario,
                    SnapshotDate = DateTime.UtcNow,
                    HorizonMonths = request.Dto.HorizonMonths,
                    ForecastStartDate = DateTime.UtcNow,
                    ForecastEndDate = DateTime.UtcNow.AddMonths(request.Dto.HorizonMonths),
                    Status = ForecastStatus.Draft,
                    CreatedByUserId = request.CreatedByUserId,
                    Notes = request.Dto.Notes
                };

                var currentBalance = await context.GeneralLedgers
                    .Where(g => !g.IsDeleted && g.ChartOfAccount.AccountType == AccountType.Asset)
                    .SumAsync(g => g.DebitAmount - g.CreditAmount, cancellationToken);

                forecast.ProjectedCashBalance = currentBalance;

                var currentAR = await context.AccountsReceivables
                    .Where(a => !a.IsDeleted && a.Status != ARStatus.Paid)
                    .SumAsync(a => a.BalanceAmount, cancellationToken);

                forecast.ProjectedAccountsReceivable = currentAR;

                var currentAP = await context.AccountsPayables
                    .Where(a => !a.IsDeleted && a.Status != APStatus.Paid)
                    .SumAsync(a => a.BalanceAmount, cancellationToken);

                forecast.ProjectedAccountsPayable = currentAP;

                context.ForecastSnapshots.Add(forecast);
                await context.SaveChangesAsync(cancellationToken);

                GenerateForecastLines(forecast, request.Dto.GrowthPercentage ?? 0);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = forecast.Id, status = ForecastStatus.Draft });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating forecast: {ex.Message}");
            }
        }

        private void GenerateForecastLines(ForecastSnapshot forecast, decimal growthPercentage)
        {
            for (int month = 1; month <= forecast.HorizonMonths; month++)
            {
                var forecastDate = forecast.ForecastStartDate.AddMonths(month);
                var growthFactor = 1 + (growthPercentage / 100);

                var cashLine = new ForecastLine
                {
                    ForecastSnapshotId = forecast.Id,
                    ForecastMonth = month,
                    ForecastDate = forecastDate,
                    MetricType = ForecastMetricType.CashFlow.ToString(),
                    MetricName = "Projected Cash Balance",
                    ProjectedValue = forecast.ProjectedCashBalance * (decimal)Math.Pow((double)growthFactor, month)
                };

                var arLine = new ForecastLine
                {
                    ForecastSnapshotId = forecast.Id,
                    ForecastMonth = month,
                    ForecastDate = forecastDate,
                    MetricType = ForecastMetricType.AccountsReceivable.ToString(),
                    MetricName = "Projected Accounts Receivable",
                    ProjectedValue = forecast.ProjectedAccountsReceivable * (decimal)Math.Pow((double)growthFactor, month)
                };

                var apLine = new ForecastLine
                {
                    ForecastSnapshotId = forecast.Id,
                    ForecastMonth = month,
                    ForecastDate = forecastDate,
                    MetricType = ForecastMetricType.AccountsPayable.ToString(),
                    MetricName = "Projected Accounts Payable",
                    ProjectedValue = forecast.ProjectedAccountsPayable * (decimal)Math.Pow((double)growthFactor, month)
                };

                forecast.Lines.Add(cashLine);
                forecast.Lines.Add(arLine);
                forecast.Lines.Add(apLine);
            }
        }
    }
    #endregion

    #region Update Forecast
    public class UpdateForecastCommand : IRequest<Result>
    {
        public UpdateForecastSnapshotDto Dto { get; set; } = null!;
    }

    public class UpdateForecastCommandHandler(ERP_DbContext context) : IRequestHandler<UpdateForecastCommand, Result>
    {
        public async Task<Result> Handle(UpdateForecastCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var forecast = await context.ForecastSnapshots
                    .FirstOrDefaultAsync(f => f.Id == request.Dto.Id && !f.IsDeleted, cancellationToken);

                if (forecast == null)
                    return Result.Fail("Forecast not found");

                if (forecast.Status != ForecastStatus.Draft)
                    return Result.Fail("Only draft forecasts can be updated");

                forecast.Name = request.Dto.Name;
                forecast.Scenario = request.Dto.Scenario;
                forecast.Notes = request.Dto.Notes;

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = forecast.Id, status = forecast.Status });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating forecast: {ex.Message}");
            }
        }
    }
    #endregion

    #region Submit Forecast
    public class SubmitForecastCommand : IRequest<Result>
    {
        public int ForecastId { get; set; }
    }

    public class SubmitForecastCommandHandler(ERP_DbContext context) : IRequestHandler<SubmitForecastCommand, Result>
    {
        public async Task<Result> Handle(SubmitForecastCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var forecast = await context.ForecastSnapshots
                    .FirstOrDefaultAsync(f => f.Id == request.ForecastId && !f.IsDeleted, cancellationToken);

                if (forecast == null)
                    return Result.Fail("Forecast not found");

                forecast.Status = ForecastStatus.Submitted;

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = forecast.Id, status = ForecastStatus.Submitted });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error submitting forecast: {ex.Message}");
            }
        }
    }
    #endregion

    #region Approve Forecast
    public class ApproveForecastCommand : IRequest<Result>
    {
        public int ForecastId { get; set; }
        public Guid ApprovedByUserId { get; set; }
    }

    public class ApproveForecastCommandHandler(ERP_DbContext context) : IRequestHandler<ApproveForecastCommand, Result>
    {
        public async Task<Result> Handle(ApproveForecastCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var forecast = await context.ForecastSnapshots
                    .FirstOrDefaultAsync(f => f.Id == request.ForecastId && !f.IsDeleted, cancellationToken);

                if (forecast == null)
                    return Result.Fail("Forecast not found");

                forecast.Status = ForecastStatus.Approved;
                forecast.ApprovedByUserId = request.ApprovedByUserId;
                forecast.ApprovedDate = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(new { id = forecast.Id, status = ForecastStatus.Approved });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error approving forecast: {ex.Message}");
            }
        }
    }
    #endregion
}
