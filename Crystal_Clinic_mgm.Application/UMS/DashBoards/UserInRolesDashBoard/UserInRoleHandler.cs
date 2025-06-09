using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.DashBoards.UserInRolesDashBoard
{
    public class UserInRoleHandler : IRequestHandler<UserInRoleCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserRole> _genericRepositoryAsync;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGeneralHelperRepositoryAsync _helper;
        public UserInRoleHandler(
            IGenericRepositoryAsync<UMS_DbContext, UserRole> genericRepositoryAsync,
            ILoggedInUser loggedInUser,
            IGeneralHelperRepositoryAsync helper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _loggedInUser = loggedInUser;
            _helper = helper;
        }

        public async Task<JsonResult> Handle(UserInRoleCommand request, CancellationToken cancellationToken)
        {
            var language = GeneralHelper.SelectedLanauge(request.Language);
            List<int> childBranchIds = await _helper.GetChildBranchs(_loggedInUser.BranchId);
            var UserInrole = await _genericRepositoryAsync.FindByCondition(x => x.IsDeleted == false
            && x.User.IsDeleted == false
            && x.User.IsActive == true
            && (childBranchIds.Contains(x.User.BranchId ?? 0)
            || x.User.BranchId == _loggedInUser.BranchId)).Include(x => x.User)
            .Select(userrole => UserInRoleModel.Projection.Compile()
            .Invoke(userrole, language, _helper))
            .ToListAsync(cancellationToken);
            var CountData = UserInrole
                         .GroupBy(x => x.UserName)
                         .Select(rw => new UserInRoleModel
                         {
                             UserName = rw.Key,
                             Counts = rw.Count()
                         }).OrderBy(x => x.UserName);

            return new JsonResult(CountData);
        }
    }
}
