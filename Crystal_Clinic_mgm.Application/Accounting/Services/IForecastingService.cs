using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IForecastingService
    {
        Task<Result> CreateForecastAsync(CreateForecastSnapshotDto dto, Guid userId);
        Task<Result> UpdateForecastAsync(UpdateForecastSnapshotDto dto);
        Task<Result> SubmitForecastAsync(int forecastId);
        Task<Result> ApproveForecastAsync(int forecastId, Guid userId);
        Task<Result> GetForecastAsync(int forecastId);
        Task<Result> GetAllForecastsAsync(string? scenario = null, int page = 1, int pageSize = 10);
        Task<Result> GetForecastByScenarioAsync(string scenario);
    }
}
