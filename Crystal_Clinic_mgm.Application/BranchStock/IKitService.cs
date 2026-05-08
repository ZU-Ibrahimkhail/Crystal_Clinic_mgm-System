using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public interface IKitService
    {
        /// <summary>
        /// Create Inventory Kit
        /// </summary>
        Task<Result> CreateKitAsync(CreateKitRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Detail of Inventory Kit by Id
        /// </summary>
        Task<Result> GetKitByIdAsync(int Id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets available inventory kits
        /// </summary>
        Task<Result> GetAvailableKitsAsync(int? branchId, int pageSize, int pageNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update inventory Kit
        /// </summary>
        Task<Result> UpdateKitAsync(UpdateKitRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete inventory kit by Id
        /// </summary>
        Task<Result> DeleteKitAsync(int Id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Consumes an inventory kit
        /// </summary>
        Task<KitConsumptionResult> ConsumeKitAsync(int kitId, int quantity, string referenceId, CancellationToken cancellationToken = default);
    }
}
