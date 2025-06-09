using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.General.News.Queries.GetList
{
    public class GetNewsListHandler : IRequestHandler<GetNewsListQuery, ResponseDataTable<GetNewsListModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, NewsNotification> _GenericRepositoryAsync;
        private readonly IMapper _Mapper;
        private readonly ILoggedInUser _loggedInUser;

        public GetNewsListHandler(
            IGenericRepositoryAsync<ERP_DbContext, NewsNotification> genericRepositoryAsync,
            IMapper mapper,
            ILoggedInUser loggedInUser)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _Mapper = mapper;
            _loggedInUser = loggedInUser;
        }

        public async Task<ResponseDataTable<GetNewsListModel>> Handle(GetNewsListQuery request, CancellationToken cancellationToken)
        {
            var Language = GeneralHelper.SelectedLanauge(request.Language);
            return await Task.Run(() =>
            {

                var entity = _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted && (x.BranchId == null || x.BranchId == _loggedInUser.BranchId))
                             .Include(x => x.News).Select(x => x.News).OrderByDescending(x => x!.ModifiedOn);
                var records = _Mapper.Map<IEnumerable<GetNewsListModel>>(entity);
                if (!string.IsNullOrEmpty(request.SearchBy))
                {
                    var searchBy = request.SearchBy.ToLower().Trim();

                    records = records.Where(x =>
                        x.Title!.ToLower().Contains(searchBy) ||
                        x.Speaker!.ToLower().Contains(searchBy) ||
                        x.Description!.ToLower().Contains(searchBy));

                }
                return MyDataTable<GetNewsListModel>.Generate(records, request.PageSize, request.PageIndex);
            });



        }

    }
}
