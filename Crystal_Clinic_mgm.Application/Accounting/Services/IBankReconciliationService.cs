using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IBankReconciliationService
    {
        Task<Result> UploadBankStatementAsync(CreateBankStatementImportDto dto);
        Task<Result> AutoMatchTransactionsAsync(int bankStatementImportId, decimal amountTolerance = 0.01m, int dateTolerance = 5);
        Task<Result> ManualMatchTransactionAsync(CreateBankMatchDto dto);
        Task<Result> CloseBankReconciliationAsync(int bankStatementImportId);
        Task<Result> GetBankStatementImportAsync(int importId);
        Task<Result> GetAllBankStatementImportsAsync(int? bankAccountId = null, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10);
        Task<Result> GetReconciliationSummaryAsync(int bankStatementImportId);
        Task<Result> GetUnmatchedTransactionsAsync(int bankStatementImportId);
    }
}
