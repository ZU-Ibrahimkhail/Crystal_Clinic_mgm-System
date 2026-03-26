using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IProcurementService
    {
        Task<Result> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto, CancellationToken cancellationToken = default);
        Task<Result> UpdatePurchaseOrderAsync(UpdatePurchaseOrderDto dto, CancellationToken cancellationToken = default);
        Task<Result> ReceivePurchaseOrderAsync(int purchaseOrderId, CancellationToken cancellationToken = default);
        Task<Result> GetAllPurchaseOrdersAsync(int? vendorId = null, int? status = null, int? branchId = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<Result> GetPurchaseOrderByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Result> CreateVendorBillAsync(CreateVendorBillDto dto, CancellationToken cancellationToken = default);
        Task<Result> UpdateVendorBillAsync(UpdateVendorBillDto dto, CancellationToken cancellationToken = default);
        Task<Result> PayVendorBillAsync(int vendorBillId, decimal paymentAmount, int paymentMethodId, string reference, CancellationToken cancellationToken = default);
        Task<Result> GetAllVendorBillsAsync(int? vendorId = null, int? status = null, int? branchId = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<Result> GetVendorBillByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
