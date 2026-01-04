using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using MediatR;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class FixedAssetService(IMediator mediator) : IFixedAssetService
    {
        public async Task<Result> CreateFixedAssetAsync(CreateFixedAssetDto dto, CancellationToken cancellationToken = default)
        {
            var command = new CreateFixedAssetCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> UpdateFixedAssetAsync(UpdateFixedAssetDto dto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateFixedAssetCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> RecordDepreciationAsync(int fixedAssetId, decimal depreciationAmount, CancellationToken cancellationToken = default)
        {
            var command = new RecordDepreciationCommand
            {
                FixedAssetId = fixedAssetId,
                DepreciationAmount = depreciationAmount
            };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> DeactivateFixedAssetAsync(int fixedAssetId, CancellationToken cancellationToken = default)
        {
            var command = new DeactivateFixedAssetCommand { FixedAssetId = fixedAssetId };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> GetAllFixedAssetsAsync(int? categoryId = null, int? branchId = null, bool? isActive = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetAllFixedAssetsQuery
            {
                CategoryId = categoryId,
                BranchId = branchId,
                IsActive = isActive,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetFixedAssetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var query = new GetFixedAssetByIdQuery { Id = id };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetDepreciationScheduleAsync(int fixedAssetId, CancellationToken cancellationToken = default)
        {
            var query = new GetDepreciationScheduleQuery { FixedAssetId = fixedAssetId };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetAllDepreciatingAssetsAsync(int? branchId = null, CancellationToken cancellationToken = default)
        {
            var query = new GetAllDepreciatingAssetsQuery { BranchId = branchId };
            return await mediator.Send(query, cancellationToken);
        }
    }
}
