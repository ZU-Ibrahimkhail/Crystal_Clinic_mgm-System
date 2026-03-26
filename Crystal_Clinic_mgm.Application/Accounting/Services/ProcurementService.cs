using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using MediatR;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class ProcurementService(IMediator mediator) : IProcurementService
    {
        public async Task<Result> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto, CancellationToken cancellationToken = default)
        {
            var command = new CreatePurchaseOrderCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> UpdatePurchaseOrderAsync(UpdatePurchaseOrderDto dto, CancellationToken cancellationToken = default)
        {
            var command = new UpdatePurchaseOrderCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> ReceivePurchaseOrderAsync(int purchaseOrderId, CancellationToken cancellationToken = default)
        {
            var command = new ReceivePurchaseOrderCommand { PurchaseOrderId = purchaseOrderId };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> GetAllPurchaseOrdersAsync(int? vendorId = null, int? status = null, int? branchId = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetAllPurchaseOrdersQuery
            {
                VendorId = vendorId,
                Status = (POStatus?)status,
                BranchId = branchId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetPurchaseOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var query = new GetPurchaseOrderByIdQuery { Id = id };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> CreateVendorBillAsync(CreateVendorBillDto dto, CancellationToken cancellationToken = default)
        {
            var command = new CreateVendorBillCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> UpdateVendorBillAsync(UpdateVendorBillDto dto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateVendorBillCommand { Dto = dto };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> PayVendorBillAsync(int vendorBillId, decimal paymentAmount, int paymentMethodId, string reference, CancellationToken cancellationToken = default)
        {
            var command = new PayVendorBillCommand 
            { 
                VendorBillId = vendorBillId,
                PaymentAmount = paymentAmount, 
                PaymentMethodId = paymentMethodId, 
                Reference = reference
            };
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<Result> GetAllVendorBillsAsync(int? vendorId = null, int? status = null, int? branchId = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetAllVendorBillsQuery
            {
                VendorId = vendorId,
                Status = (BillStatus?)status,
                BranchId = branchId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return await mediator.Send(query, cancellationToken);
        }

        public async Task<Result> GetVendorBillByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var query = new GetVendorBillByIdQuery { Id = id };
            return await mediator.Send(query, cancellationToken);
        }
    }
}
