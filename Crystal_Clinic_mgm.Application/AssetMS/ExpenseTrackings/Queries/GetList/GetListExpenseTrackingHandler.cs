using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetList
{
    public class GetExpenseTrackingListHandler(IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> genericRepositoryAsync, IMapper mapper, ILoggedInUser loggedInUser) : IRequestHandler<GetExpenseTrackingListQuery, ResponseDataTable<GetExpenseTrackingListModel>>
    {


        private readonly IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> _GenericRepositoryAsync = genericRepositoryAsync;
        private readonly IMapper _Mapper = mapper;
        private readonly ILoggedInUser _LoggedInUser = loggedInUser;

        public async Task<ResponseDataTable<GetExpenseTrackingListModel>> Handle(GetExpenseTrackingListQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            string? SearchBy = request.SearchBy?.ToLower();
            return await Task.Run(() =>
            {

                var entity = _GenericRepositoryAsync
                    .FindByCondition(x => x.IsDeleted == false &&
                    (x.BranchId == _LoggedInUser.BranchId || _LoggedInUser.IsSuperAdmin) &&
                    (request.MainAccountId == null || x.MainAccountId == request.MainAccountId) &&
                    (SearchBy == null ||
                         x.Amount.ToString().Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                         x.Description.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                         x.ExpenseType!.EnglishName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                         x.ExpenseType!.PashtoName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
                         x.ExpenseType!.DariName.Contains(SearchBy, StringComparison.CurrentCultureIgnoreCase)))
                    .OrderByDescending(x => x.ModifiedOn).Include(x => x.CurrencyType).Include(x => x.Branch).Include(x => x.ExpenseType);
                var DTO = _Mapper.Map<List<GetExpenseTrackingListModel>>(entity);


                return MyDataTable<GetExpenseTrackingListModel>.Generate(DTO, request.PageSize, request.PageIndex);
            });

        }
    }
}
