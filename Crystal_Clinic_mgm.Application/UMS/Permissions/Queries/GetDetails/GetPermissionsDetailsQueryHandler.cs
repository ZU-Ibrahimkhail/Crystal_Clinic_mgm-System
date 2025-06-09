using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails
{
    public class GetPermissionsDetailsQueryHandler : IRequestHandler<GetPermissionsDetailsQuery, JsonResult>
    {
        private readonly UMS_DbContext _DbContext;
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Permission> _GRepoPermission;
        private readonly IGenericRepositoryAsync<UMS_DbContext, RolePermission> _GRepoRolePermission;

        public GetPermissionsDetailsQueryHandler(UMS_DbContext dbContext, IMessage message, IGenericRepositoryAsync<UMS_DbContext, Permission> gRepoPermission, IGenericRepositoryAsync<UMS_DbContext, RolePermission> gRepoRolePermission)
        {
            _DbContext = dbContext;
            _message = message;
            _GRepoPermission = gRepoPermission;
            _GRepoRolePermission = gRepoRolePermission;
        }

        public async Task<JsonResult> Handle(GetPermissionsDetailsQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                return _message.RecordNotFound();
            }
            int rolecount = _GRepoRolePermission.FindByCondition(rp => rp.PermissionId == request.Id).Count();
            PermissionViewModel Permission = new();
            if (rolecount > 0)
            {
                var permission = await _GRepoPermission.FindByCondition(p => p.Id == request.Id && p.IsDeleted == false)
                                                        .Include(x => x.Application)
                                                        .Select(p => PermissionViewModel.Projection.Compile().Invoke(p, _DbContext))
                                                        .SingleOrDefaultAsync(cancellationToken);
                Permission = permission ?? new();
                Permission.Roles = await _GRepoRolePermission.FindByCondition(rp => rp.PermissionId == Permission.Id && rp.IsDeleted == false)
                                                .Include(r => r.Role).ThenInclude(x => x != null ? x.Application : null)
                                                .Select(rp => ListRoleViewModel.Projection.Compile().Invoke(rp.Role ?? new(), _DbContext)
                                                ).ToListAsync(cancellationToken);
            }
            else
            {
                var permission = await _GRepoPermission.FindByCondition(p => p.Id == request.Id && p.IsDeleted == false)
                                               .Select(p => PermissionViewModel.Projection.Compile().Invoke(p, _DbContext))
                                               .SingleOrDefaultAsync(cancellationToken);
                Permission = permission ?? new();
            }
            if (Permission == null)
            {
                return _message.RecordNotFound();
            }

            return new JsonResult(Permission);
        }
    }
}
