using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.UMS.DashBoards.EachAndTotalUserInDepartmentDashBoard
{
    public class EachAndTotalUserInBranchDashBoardHandler : IRequestHandler<EachAndTotalUserInBranchDashBoardCommand, JsonResult>
    {
        private readonly IGenericDashboardRepository<ApplicationUser> _genericDashboardRepository;
        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GRepoBranch;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoApplicationUser;
        private readonly ILoggedInUser _loggedInUser;

        public EachAndTotalUserInBranchDashBoardHandler(IGenericDashboardRepository<ApplicationUser> genericDashboardRepository, IGenericRepositoryAsync<ERP_DbContext, Branch> gRepoUsers, ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> gRepoApplicationUser)
        {
            _genericDashboardRepository = genericDashboardRepository;
            _GRepoBranch = gRepoUsers;
            _loggedInUser = loggedInUser;
            _GRepoApplicationUser = gRepoApplicationUser;
        }

        public async Task<JsonResult> Handle(EachAndTotalUserInBranchDashBoardCommand request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            Localization localize = new();

            var userdeplist = _GRepoApplicationUser.FindByCondition(x => x.BranchId != null && x.IsDeleted == false && x.IsActive == true)
            .Select(x => x.BranchId).ToList();
            var branchlist = _GRepoBranch.FindByCondition(x => x.IsDeleted == false && userdeplist.Contains(x.ID)).ToList();
            var deplist = branchlist.Select(x => localize.GetName(language, x)).ToList();

            List<Expression<Func<ApplicationUser, bool>>> expressions = new();




            foreach (var user in branchlist)
            {
                expressions.Add(x => x.IsDeleted == false && x.IsActive == true && x.BranchId == user.ID && x.BranchId != null);
            };

            var Result = await _genericDashboardRepository.GetSpiderDashboardData(expressions, deplist, cancellationToken, Constants.ApplicationModule.UMS);

            return new JsonResult(Result);
        }
    }
}
