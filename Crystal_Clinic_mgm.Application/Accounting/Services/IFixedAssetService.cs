using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IFixedAssetService
    {
        Task<Result> CreateFixedAssetAsync(CreateFixedAssetDto dto, CancellationToken cancellationToken = default);
        Task<Result> UpdateFixedAssetAsync(UpdateFixedAssetDto dto, CancellationToken cancellationToken = default);
        Task<Result> RecordDepreciationAsync(int fixedAssetId, decimal depreciationAmount, CancellationToken cancellationToken = default);
        Task<Result> DeactivateFixedAssetAsync(int fixedAssetId, CancellationToken cancellationToken = default);
        Task<Result> GetAllFixedAssetsAsync(int? categoryId = null, int? branchId = null, bool? isActive = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<Result> GetFixedAssetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> GetDepreciationScheduleAsync(int fixedAssetId, CancellationToken cancellationToken = default);
        Task<Result> GetAllDepreciatingAssetsAsync(int? branchId = null, CancellationToken cancellationToken = default);
    }
}
