using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Queries
{
    #region Get Forecast
    public class GetForecastQuery : IRequest<Result>
    {
        public int ForecastId { get; set; }
    }

    public class GetForecastQueryHandler(ERP_DbContext context) : IRequestHandler<GetForecastQuery, Result>
    {
        public async Task<Result> Handle(GetForecastQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var forecast = await context.ForecastSnapshots
                    .Include(f => f.Lines)
                    .FirstOrDefaultAsync(f => f.Id == request.ForecastId && !f.IsDeleted, cancellationToken);

                if (forecast == null)
                    return Result.Fail("Forecast not found");

                var dto = new ForecastSnapshotDto
                {
                    Id = forecast.Id,
                    Name = forecast.Name,
                    Scenario = forecast.Scenario,
                    SnapshotDate = forecast.SnapshotDate,
                    HorizonMonths = forecast.HorizonMonths,
                    ForecastStartDate = forecast.ForecastStartDate,
                    ForecastEndDate = forecast.ForecastEndDate,
                    ProjectedCashBalance = forecast.ProjectedCashBalance,
                    ProjectedAccountsReceivable = forecast.ProjectedAccountsReceivable,
                    ProjectedAccountsPayable = forecast.ProjectedAccountsPayable,
                    ProjectedNetIncome = forecast.ProjectedNetIncome,
                    Status = forecast.Status,
                    CreatedByUserId = forecast.CreatedByUserId,
                    ApprovedByUserId = forecast.ApprovedByUserId,
                    ApprovedDate = forecast.ApprovedDate,
                    Notes = forecast.Notes,
                    Lines = forecast.Lines.Select(l => new ForecastLineDto
                    {
                        Id = l.Id,
                        ForecastMonth = l.ForecastMonth,
                        ForecastDate = l.ForecastDate,
                        MetricType = l.MetricType,
                        MetricName = l.MetricName,
                        ProjectedValue = l.ProjectedValue,
                        VariancePercentage = l.VariancePercentage,
                        Notes = l.Notes
                    }).ToList()
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving forecast: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get All Forecasts
    public class GetAllForecastsQuery : IRequest<Result>
    {
        public string? Scenario { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllForecastsQueryHandler(ERP_DbContext context) : IRequestHandler<GetAllForecastsQuery, Result>
    {
        public async Task<Result> Handle(GetAllForecastsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.ForecastSnapshots
                    .Include(f => f.Lines)
                    .Where(f => !f.IsDeleted);

                if (!string.IsNullOrEmpty(request.Scenario))
                    query = query.Where(f => f.Scenario == request.Scenario);

                var forecasts = await query
                    .OrderByDescending(f => f.SnapshotDate)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = forecasts.Select(f => new ForecastSnapshotDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Scenario = f.Scenario,
                    SnapshotDate = f.SnapshotDate,
                    HorizonMonths = f.HorizonMonths,
                    ForecastStartDate = f.ForecastStartDate,
                    ForecastEndDate = f.ForecastEndDate,
                    ProjectedCashBalance = f.ProjectedCashBalance,
                    ProjectedAccountsReceivable = f.ProjectedAccountsReceivable,
                    ProjectedAccountsPayable = f.ProjectedAccountsPayable,
                    ProjectedNetIncome = f.ProjectedNetIncome,
                    Status = f.Status,
                    CreatedByUserId = f.CreatedByUserId,
                    ApprovedByUserId = f.ApprovedByUserId,
                    ApprovedDate = f.ApprovedDate,
                    Notes = f.Notes,
                    Lines = f.Lines.Select(l => new ForecastLineDto
                    {
                        Id = l.Id,
                        ForecastMonth = l.ForecastMonth,
                        ForecastDate = l.ForecastDate,
                        MetricType = l.MetricType,
                        MetricName = l.MetricName,
                        ProjectedValue = l.ProjectedValue,
                        VariancePercentage = l.VariancePercentage,
                        Notes = l.Notes
                    }).ToList()
                }).ToList();

                return Result.Success(new { data = dtos, page = request.Page, pageSize = request.PageSize });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving forecasts: {ex.Message}");
            }
        }
    }
    #endregion

    #region Get Forecast By Scenario
    public class GetForecastByScenarioQuery : IRequest<Result>
    {
        public string Scenario { get; set; } = string.Empty;
    }

    public class GetForecastByScenarioQueryHandler(ERP_DbContext context) : IRequestHandler<GetForecastByScenarioQuery, Result>
    {
        public async Task<Result> Handle(GetForecastByScenarioQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var forecast = await context.ForecastSnapshots
                    .Include(f => f.Lines)
                    .Where(f => !f.IsDeleted && f.Scenario == request.Scenario)
                    .OrderByDescending(f => f.SnapshotDate)
                    .FirstOrDefaultAsync(cancellationToken);

                if (forecast == null)
                    return Result.Fail("Forecast not found for this scenario");

                var dto = new ForecastSnapshotDto
                {
                    Id = forecast.Id,
                    Name = forecast.Name,
                    Scenario = forecast.Scenario,
                    SnapshotDate = forecast.SnapshotDate,
                    HorizonMonths = forecast.HorizonMonths,
                    ForecastStartDate = forecast.ForecastStartDate,
                    ForecastEndDate = forecast.ForecastEndDate,
                    ProjectedCashBalance = forecast.ProjectedCashBalance,
                    ProjectedAccountsReceivable = forecast.ProjectedAccountsReceivable,
                    ProjectedAccountsPayable = forecast.ProjectedAccountsPayable,
                    ProjectedNetIncome = forecast.ProjectedNetIncome,
                    Status = forecast.Status,
                    CreatedByUserId = forecast.CreatedByUserId,
                    ApprovedByUserId = forecast.ApprovedByUserId,
                    ApprovedDate = forecast.ApprovedDate,
                    Notes = forecast.Notes,
                    Lines = forecast.Lines.Select(l => new ForecastLineDto
                    {
                        Id = l.Id,
                        ForecastMonth = l.ForecastMonth,
                        ForecastDate = l.ForecastDate,
                        MetricType = l.MetricType,
                        MetricName = l.MetricName,
                        ProjectedValue = l.ProjectedValue,
                        VariancePercentage = l.VariancePercentage,
                        Notes = l.Notes
                    }).ToList()
                };

                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving forecast: {ex.Message}");
            }
        }
    }
    #endregion
}
