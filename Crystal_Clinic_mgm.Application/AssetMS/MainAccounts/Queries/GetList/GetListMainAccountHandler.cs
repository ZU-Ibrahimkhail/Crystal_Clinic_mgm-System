using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetList
{
    public class GetMainAccountListHandler(IGenericRepositoryAsync<ERP_DbContext, MainAccount> genericRepositoryAsync, IMapper mapper, ILoggedInUser loggedInUser) : IRequestHandler<GetMainAccountListQuery, ResponseDataTable<GetMainAccountDetailModel>>
    {


        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GenericRepositoryAsync = genericRepositoryAsync;
        private readonly IMapper _Mapper = mapper;
        private readonly ILoggedInUser _LoggedInUser = loggedInUser;

        public async Task<ResponseDataTable<GetMainAccountDetailModel>> Handle(GetMainAccountListQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            string? SearchBy = request.SearchBy?.ToLower().Trim();
            return await Task.Run(() =>
            {

                var entity = _GenericRepositoryAsync
                    .FindByCondition(x => x.IsDeleted == false && ((x.OwnerUserId == _LoggedInUser.Id) || _LoggedInUser.IsSuperAdmin || _LoggedInUser.IsBranchAdmin) &&
                    (request.CurrencyTypeId == null || x.CurrencyTypeId == request.CurrencyTypeId) &&
                    (SearchBy == null ||
                    x.BalanceAmount.ToString().Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Description.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                    x.CurrencyType!.EnglishName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                    x.CurrencyType!.PashtoName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                    x.CurrencyType!.DariName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase)))
                    .OrderByDescending(x => x.ModifiedOn).Include(x => x.Parent).Include(x => x.CurrencyType).Include(x => x.Branch).Include(x => x.Parent);

                var DTO = _Mapper.Map<List<GetMainAccountDetailModel>>(entity);


                return MyDataTable<GetMainAccountDetailModel>.Generate(DTO, request.PageSize, request.PageIndex);
            });

        }
    }
}
