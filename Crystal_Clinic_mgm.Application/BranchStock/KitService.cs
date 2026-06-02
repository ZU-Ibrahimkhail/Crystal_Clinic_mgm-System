using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using MediatR;


namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public class KitService(IMediator mediator) : IKitService
    {
        public Task<KitConsumptionResult> ConsumeKitAsync(int VisitKitId, int quantity, string referenceId, CancellationToken cancellationToken = default)
        {
            var command = new ConsumeKitCommand { VisitKitId = VisitKitId, Quantity = quantity, ReferenceId = referenceId };
            return mediator.Send(command, cancellationToken);
        }

        public async Task<Result> CreateKitAsync(CreateKitRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateKitCommand { Dto = request };
            return await mediator.Send(command, cancellationToken);
        }

        public Task<Result> DeleteKitAsync(int Id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteKitCommand { Id = Id };
            return mediator.Send(command, cancellationToken);
        }

        public async Task<Result> GetAvailableKitsAsync(int? branchId, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
        {
            var query = new GetKitListQuery { BranchId = branchId, PageNumber = pageNumber, PageSize = pageSize };
            return await mediator.Send(query, cancellationToken);
        }

        public Task<Result> GetKitByIdAsync(int Id, CancellationToken cancellationToken = default)
        {
            var query = new GetKitByIdQuery { Id = Id };
            return mediator.Send(query, cancellationToken);
        }

        public Task<Result> UpdateKitAsync(UpdateKitRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateKitCommand { Dto = request };
            return mediator.Send(command, cancellationToken);
        }
    }
}
