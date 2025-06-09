using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionDDL
{
    public class GetPermissionDDLQueryHandler : IRequestHandler<GetPermissionDDLQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, Permission> _GRepoPermission;

        public GetPermissionDDLQueryHandler(IGenericRepositoryAsync<UMS_DbContext, Permission> gRepoPermission)
        {
            _GRepoPermission = gRepoPermission;
        }

        public async Task<JsonResult> Handle(GetPermissionDDLQuery request, CancellationToken cancellationToken)
        {
            var permations = await _GRepoPermission.FindByCondition(x => x.IsDeleted == false && x.ApplicationId == request.ApplicationId).Include(a => a.Application).Select(s => new
            {
                s.Id,
                s.Name,
                s.IsGlobal,
                s.Description
            }).ToListAsync(cancellationToken);
            return new JsonResult(permations);
        }
    }
}
