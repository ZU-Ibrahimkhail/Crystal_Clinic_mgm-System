using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.AccountTrackings.Queries.GetList
{
    public class GetAccountTrackingListHandler(IGenericRepositoryAsync<ERP_DbContext, AccountTracking> gRepoAccountTracking, IGeneralHelperRepositoryAsync helper) : IRequestHandler<GetAccountTrackingListQuery, ResponseDataTable<GetAccountTrackingListModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = gRepoAccountTracking;
        private readonly IGeneralHelperRepositoryAsync _helper = helper;

        public async Task<ResponseDataTable<GetAccountTrackingListModel>> Handle(GetAccountTrackingListQuery request, CancellationToken cancellationToken)
        {
            request.Language = GeneralHelper.SelectedLanauge(request.Language);
            var Data = _GRepoAccountTracking.FindByCondition(x => !x.IsDeleted && x.MainAccountId == request.MainAccountId &&
            x.TransactionDate > request.FromDate && x.TransactionDate <= request.ToDate &&
            (request.UserId == null || x.UserId == request.UserId) &&
            (string.IsNullOrEmpty(request.SearchBy) ||
                x.Description!.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                x.DebitAmount.ToString().Contains(request.SearchBy) ||
                x.CreditAmount.ToString().Contains(request.SearchBy) ||
                x.BalanceAmount.ToString().Contains(request.SearchBy))).Include(x => x.CurrencyType)
            .Select(x => new GetAccountTrackingListModel(x, _helper, request.Language).GetData());

            return await MyDataTable<GetAccountTrackingListModel>.Generate(Data, request.PageSize, request.PageIndex);
        }
    }
}
