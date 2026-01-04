using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using MediatR;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class ForecastingService(IMediator mediator) : IForecastingService
    {
        public async Task<Result> CreateForecastAsync(CreateForecastSnapshotDto dto, int userId)
        {
            var command = new CreateForecastCommand { Dto = dto, CreatedByUserId = userId };
            return await mediator.Send(command);
        }

        public async Task<Result> UpdateForecastAsync(UpdateForecastSnapshotDto dto)
        {
            var command = new UpdateForecastCommand { Dto = dto };
            return await mediator.Send(command);
        }

        public async Task<Result> SubmitForecastAsync(int forecastId)
        {
            var command = new SubmitForecastCommand { ForecastId = forecastId };
            return await mediator.Send(command);
        }

        public async Task<Result> ApproveForecastAsync(int forecastId, int userId)
        {
            var command = new ApproveForecastCommand { ForecastId = forecastId, ApprovedByUserId = userId };
            return await mediator.Send(command);
        }

        public async Task<Result> GetForecastAsync(int forecastId)
        {
            var query = new GetForecastQuery { ForecastId = forecastId };
            return await mediator.Send(query);
        }

        public async Task<Result> GetAllForecastsAsync(string? scenario = null, int page = 1, int pageSize = 10)
        {
            var query = new GetAllForecastsQuery { Scenario = scenario, Page = page, PageSize = pageSize };
            return await mediator.Send(query);
        }

        public async Task<Result> GetForecastByScenarioAsync(string scenario)
        {
            var query = new GetForecastByScenarioQuery { Scenario = scenario };
            return await mediator.Send(query);
        }
    }
}
