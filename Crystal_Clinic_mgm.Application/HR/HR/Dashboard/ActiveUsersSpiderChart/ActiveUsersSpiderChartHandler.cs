using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.HR.HR.Dashboard.ActiveUsersSpiderChart
{
    public class ActiveUsersSpiderChartHandler : IRequestHandler<ActiveUsersSpiderChartQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _repository;
        private readonly IGeneralHelperRepositoryAsync _helper;
        private readonly IGenericDashboardRepository<EmployeeProfile> _dashboardRepository;
        private readonly ILoggedInUser _loggedInUser;

        public ActiveUsersSpiderChartHandler(
            IGenericRepositoryAsync<ERP_DbContext, Branch> repository,
            IGeneralHelperRepositoryAsync helper,
            ILoggedInUser loggedInUser,
            IGenericDashboardRepository<EmployeeProfile> dashboardRepository)
        {
            _repository = repository;
            _helper = helper;
            _loggedInUser = loggedInUser;
            _dashboardRepository = dashboardRepository;
        }

        public async Task<JsonResult> Handle(ActiveUsersSpiderChartQuery request, CancellationToken cancellationToken)
        {
            var ChildBranchs = await _helper.GetChildBranchs(_loggedInUser.BranchId).ConfigureAwait(false);
            ChildBranchs.Add(_loggedInUser.BranchId);
            string language = GeneralHelper.SelectedLanauge(request.Language);

            Localization localize = new();
            var ChildBranchNames = await _repository.FindByCondition(x => !x.IsDeleted && ChildBranchs.Contains(x.ID))
                                                  .Select(x => localize.GetName(language, x))
                                                  .ToListAsync(cancellationToken).ConfigureAwait(false);



            List<Expression<Func<EmployeeProfile, bool>>> expressions = new();
            foreach (var BranchId in ChildBranchs)
            {
                expressions.Add(x => x.IsDeleted && x.IsActive && x.HasAccount && BranchId == x.BranchId);
            };


            var result = await _dashboardRepository.GetSpiderDashboardData(expressions, ChildBranchNames, cancellationToken).ConfigureAwait(false); ;
            return new JsonResult(result);

        }
    }
}
