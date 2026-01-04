using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IEquityService
    {
        Task<Result> CreateShareholderAsync(CreateShareholderDto dto, CancellationToken cancellationToken = default);
        Task<Result> UpdateShareholderAsync(UpdateShareholderDto dto, CancellationToken cancellationToken = default);
        Task<Result> RecordEquityTransactionAsync(CreateEquityTransactionDto dto, CancellationToken cancellationToken = default);
        Task<Result> GetAllShareholdersAsync(bool? isActive = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<Result> GetShareholderByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> GetEquityTransactionsAsync(int? shareholderId = null, int? type = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<Result> GetEquityReportAsync(DateTime? asOfDate = null, CancellationToken cancellationToken = default);
    }
}
