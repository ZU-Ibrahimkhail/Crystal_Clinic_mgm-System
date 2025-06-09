using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Data;

namespace Crystal_Clinic_Mgm.Application.UMS.UserReport.RolesInfo.Queries
{
    public class GetRoleInfoListQueryHandler : IRequestHandler<GetRoleInfoListQuery, JsonResult>
    {
        private readonly UMS_DbContext _DbContext;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> _GRepoRole;

        public GetRoleInfoListQueryHandler(UMS_DbContext dbContext, IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> gRepoRole)
        {
            _DbContext = dbContext;
            _GRepoRole = gRepoRole;
        }
        public async Task<JsonResult> Handle(GetRoleInfoListQuery request, CancellationToken cancellationToken)
        {
            var query = _GRepoRole.FindByCondition(r => r.IsDeleted == false
                                                 && (request.ApplicationId == 0 || r.ApplicationId == request.ApplicationId)
                                                  ).Include(x => x.Application);
            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                var searchBy = request.SearchBy.ToLower().Trim();
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ApplicationRole, Applications?>)query.Where(r => r.Name!.Contains(searchBy, StringComparison.CurrentCultureIgnoreCase));
            }
            var result = await query.Select(r => ListRoleViewModel.Projection.Compile().Invoke(r, _DbContext)).ToListAsync();
            return new JsonResult(result);
        }
    }
}
