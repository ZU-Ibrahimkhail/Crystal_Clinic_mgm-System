using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetList
{
    public class GetWithdrawalTrackingListHandler(IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> genericRepositoryAsync, IMapper mapper, ILoggedInUser loggedInUser) : IRequestHandler<GetWithdrawalTrackingListQuery, ResponseDataTable<GetWithdrawalTrackingListModel>>
    {


        private readonly IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> _GenericRepositoryAsync = genericRepositoryAsync;
        private readonly IMapper _Mapper = mapper;
        private readonly ILoggedInUser _LoggedInUser = loggedInUser;
        public async Task<ResponseDataTable<GetWithdrawalTrackingListModel>> Handle(GetWithdrawalTrackingListQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            string? SearchBy = request.SearchBy?.ToLower();
            return await Task.Run(() =>
            {

                var entity = _GenericRepositoryAsync
                    .FindByCondition(x => x.IsDeleted == false &&
                    (x.UserId == _LoggedInUser.Id || _LoggedInUser.IsSuperAdmin) &&
                    (request.MainAccountId == null || x.MainAccountId == request.MainAccountId) &&
                    (SearchBy == null ||
                         x.WithdrawalAmount.ToString().Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                         x.Description.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                         x.Branch!.EnglishName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                         x.Branch!.PashtoName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                         x.Branch!.DariName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase)))
                    .OrderByDescending(x => x.ModifiedOn).Include(x => x.CurrencyType).Include(x => x.Branch).Include(x => x.MainAccount);
                var DTO = _Mapper.Map<List<GetWithdrawalTrackingListModel>>(entity);


                return MyDataTable<GetWithdrawalTrackingListModel>.Generate(DTO, request.PageSize, request.PageIndex);
            });

        }
    }
}
