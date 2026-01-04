using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IChartOfAccountsService
    {
        Task<Result> CreateAccountAsync(CreateChartOfAccountsDto dto);
        Task<Result> UpdateAccountAsync(UpdateChartOfAccountsDto dto);
        Task<Result> DeleteAccountAsync(int id);
        Task<Result> GetAccountAsync(int id);
        Task<Result> GetAllAccountsAsync(bool? isActive = null, int? accountType = null, string? searchText = null);
        Task<Result> GetAccountsByTypeAsync(int accountType);
        Task<Result> MoveAccountAsync(int accountId, int? newParentAccountId);
        Task<Result> GetAccountHierarchyAsync(int? parentAccountId = null);
    }
}
