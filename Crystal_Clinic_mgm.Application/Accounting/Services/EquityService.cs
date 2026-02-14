using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using MediatR;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class EquityService(IMediator mediator) : IEquityService
    {
        public async Task<Result> CreateShareholderAsync(CreateShareholderDto dto, CancellationToken cancellationToken = default)
        {
            var command = new CreateShareholderCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> UpdateShareholderAsync(UpdateShareholderDto dto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateShareholderCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> RecordEquityTransactionAsync(CreateEquityTransactionDto dto, CancellationToken cancellationToken = default)
        {
            var command = new RecordEquityTransactionCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> GetAllShareholdersAsync(bool? isActive = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetAllShareholdersQuery
            {
                IsActive = isActive,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetShareholderByIdAsync(int id, CancellationToken cancellationToken = default)
        
        {
            var query = new GetShareholderByIdQuery { Id = id };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetEquityTransactionsAsync(int? shareholderId = null, int? type = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetEquityTransactionsQuery
            {
                ShareholderId = shareholderId,
                Type = (EquityTransactionType?)type,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetEquityReportAsync(DateTime? asOfDate = null, CancellationToken cancellationToken = default)
        {
            var query = new GetEquityReportQuery { AsOfDate = asOfDate };
            return await mediator.Send(query, cancellationToken);
        }
    }
}
