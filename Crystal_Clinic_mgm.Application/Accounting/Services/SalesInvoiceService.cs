using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using MediatR;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class SalesInvoiceService(IMediator mediator) : ISalesInvoiceService
    {
        public async Task<Result> CreateSalesInvoiceAsync(CreateSalesInvoiceDto dto, CancellationToken cancellationToken = default)
        {
            var command = new CreateSalesInvoiceCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> IssueSalesInvoiceAsync(int salesInvoiceId, CancellationToken cancellationToken = default)
        {
            var command = new IssueSalesInvoiceCommand { SalesInvoiceId = salesInvoiceId };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> RecordSalesReceiptAsync(CreateSalesReceiptDto dto, CancellationToken cancellationToken = default)
        {
            var command = new RecordSalesReceiptCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> GetAllSalesInvoicesAsync(int? visitId = null, int? customerId = null, int? status = null, int? branchId = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetAllSalesInvoicesQuery
            {
                CustomerId = customerId,
                Status = (SalesStatus?)status,
                BranchId = branchId,
                VisitId = visitId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetSalesInvoiceByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var query = new GetSalesInvoiceByIdQuery { Id = id };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetSalesReceiptsByInvoiceAsync(int salesInvoiceId, CancellationToken cancellationToken = default)
        {
            var query = new GetSalesReceiptsByInvoiceQuery { SalesInvoiceId = salesInvoiceId };
            return await mediator.Send(query, cancellationToken);
        }
    }
}
