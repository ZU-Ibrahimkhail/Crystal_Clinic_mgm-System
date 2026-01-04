using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Commands;
using Crystal_Clinic_Mgm.Application.Accounting.Queries;
using Crystal_Clinic_Mgm.Domain;
using MediatR;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class ChartOfAccountsService(IMediator mediator) : IChartOfAccountsService
    {
        public async Task<Result> CreateAccountAsync(CreateChartOfAccountsDto dto)
        {
            var command = new CreateChartOfAccountsCommand { Dto = dto };
            return await mediator.Send(command);
        }

        public async Task<Result> UpdateAccountAsync(UpdateChartOfAccountsDto dto)
        {
            var command = new UpdateChartOfAccountsCommand { Dto = dto };
            return await mediator.Send(command);
        }

        public async Task<Result> DeleteAccountAsync(int id)
        {
            var command = new DeleteChartOfAccountsCommand { Id = id };
            return await mediator.Send(command);
        }

        public async Task<Result> GetAccountAsync(int id)
        {
            var query = new GetChartOfAccountsByIdQuery { Id = id };
            return await mediator.Send(query);
        }

        public async Task<Result> GetAllAccountsAsync(bool? isActive = null, int? accountType = null, string? searchText = null)
        {
            var query = new GetAllChartOfAccountsQuery { IsActive = isActive, AccountType = accountType, SearchText = searchText };
            return await mediator.Send(query);
        }

        public async Task<Result> GetAccountsByTypeAsync(int accountType)
        {
            var query = new GetAccountsByTypeQuery { AccountType = accountType };
            return await mediator.Send(query);
        }

        public async Task<Result> MoveAccountAsync(int accountId, int? newParentAccountId)
        {
            var command = new MoveChartOfAccountCommand { AccountId = accountId, NewParentAccountId = newParentAccountId };
            return await mediator.Send(command);
        }

        public async Task<Result> GetAccountHierarchyAsync(int? parentAccountId = null)
        {
            var query = new GetAccountHierarchyQuery { ParentAccountId = parentAccountId };
            return await mediator.Send(query);
        }
    }
}
