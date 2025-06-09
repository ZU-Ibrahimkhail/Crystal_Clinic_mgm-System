using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.General.News.Queries.NewsDashboard
{
    public class NewsDashboardHandler : IRequestHandler<NewsDashboardQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, NewsNotification> _GenericRepositoryAsync;
        private readonly IMapper _Mapper;
        private readonly ILoggedInUser _loggedInUser;


        public NewsDashboardHandler(
            IGenericRepositoryAsync<ERP_DbContext, NewsNotification> genericRepositoryAsync,
            IMapper mapper,
            ILoggedInUser loggedInUser)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _Mapper = mapper;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(NewsDashboardQuery request, CancellationToken cancellationToken)
        {

            var entity = await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted
                                                                        && (x.BranchId == null || x.BranchId == _loggedInUser.BranchId)
                                                                        && x.News!.NewsDate.Date == DateTime.Now.Date
                                                                        && (x.News.EndTime == null || x.News.EndTime.Value.TimeOfDay >= DateTime.Now.TimeOfDay))
                                                                        .Include(x => x.News)
                                                                        .Select(x => x.News)
                                                                        .OrderBy(x => x!.StartTime)
                                                                        .ToListAsync(cancellationToken);

            var records = _Mapper.Map<List<NewsDashboardModel>>(entity);

            return new JsonResult(records);
        }

    }
}
