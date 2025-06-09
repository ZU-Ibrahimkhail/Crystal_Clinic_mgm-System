using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionForEdit
{
    public class GetPermissionForEditQueryHandler : IRequestHandler<GetPermissionForEditQuery, JsonResult>
    {
        private readonly UMS_DbContext _DbContext;
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Permission> _GRepoPermission;
        private readonly IGenericRepositoryAsync<UMS_DbContext, RolePermission> _GRepoRolePermission;

        public GetPermissionForEditQueryHandler(UMS_DbContext dbContext, IMessage message, IGenericRepositoryAsync<UMS_DbContext, Permission> gRepoPermission, IGenericRepositoryAsync<UMS_DbContext, RolePermission> gRepoRolePermission)
        {
            _DbContext = dbContext;
            _message = message;
            _GRepoPermission = gRepoPermission;
            _GRepoRolePermission = gRepoRolePermission;
        }

        public async Task<JsonResult> Handle(GetPermissionForEditQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                return _message.RecordNotFound();
            }
            var Permission = await _GRepoPermission.FindByCondition(p => p.Id == request.Id && p.IsDeleted == false)
                                                     .Include(x => x.Application)
                                                     .Select(p => PermissionViewModel.Projection.Compile().Invoke(p, _DbContext))
                                                     .SingleOrDefaultAsync(cancellationToken);
            if (Permission == null)
            {
                return _message.RecordNotFound();
            }

            Permission.Roles = await _GRepoRolePermission.FindByCondition(x => x.IsDeleted == false && x.PermissionId == Permission.Id)
                                            .Include(r => r.Role).ThenInclude(x => x != null ? x.Application : null)
                                            .Select(r => ListRoleViewModel.Projection
                                            .Compile().Invoke(r.Role ?? new(), _DbContext)).ToListAsync(cancellationToken);

            return new JsonResult(Permission);
        }
    }
}
