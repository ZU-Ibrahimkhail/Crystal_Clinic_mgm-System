using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface ISalesInvoiceService
    {
        Task<Result> CreateSalesInvoiceAsync(CreateSalesInvoiceDto dto, CancellationToken cancellationToken = default);
        Task<Result> IssueSalesInvoiceAsync(int salesInvoiceId, CancellationToken cancellationToken = default);
        Task<Result> RecordSalesReceiptAsync(CreateSalesReceiptDto dto, CancellationToken cancellationToken = default);
        Task<Result> DeleteSalesInvoiceAsync(int salesInvoiceId, CancellationToken cancellationToken = default);
        Task<Result> VoidSalesInvoiceAsync(int salesInvoiceId, string reason, CancellationToken cancellationToken = default);
        Task<Result> RefundSalesInvoiceAsync(int salesInvoiceId, decimal refundAmount, string reason, int paymentMethod, CancellationToken cancellationToken = default);
        Task<Result> GetAllSalesInvoicesAsync(int? visitId = null, int? customerId = null, int? status = null, int? branchId = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<Result> GetSalesInvoiceByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> GetSalesReceiptsByInvoiceAsync(int salesInvoiceId, CancellationToken cancellationToken = default);
    }
}
