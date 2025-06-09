using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleDetail
{
    public class GetRoleDetailQueryHandler : IRequestHandler<GetRoleDetailQuery, JsonResult>
    {
        private readonly UMS_DbContext _DbContext;
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> _GRepoRole;
        private readonly IGenericRepositoryAsync<UMS_DbContext, RolePermission> _GRepoRolePermission;

        public GetRoleDetailQueryHandler(UMS_DbContext dbContext, IMessage message, IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> gRepoRole, IGenericRepositoryAsync<UMS_DbContext, RolePermission> gRepoRolePermission)
        {
            _DbContext = dbContext;
            _message = message;
            _GRepoRole = gRepoRole;
            _GRepoRolePermission = gRepoRolePermission;
        }

        public async Task<JsonResult> Handle(GetRoleDetailQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                return _message.RecordNotFound();
            }
            try
            {
                var role = await _GRepoRole.FindByCondition(r => r.Id == request.Id && r.IsDeleted == false)
                                                          .Select(r => new RoleDetailsViewModel
                                                          {
                                                              Id = r.Id,
                                                              Name = r.Name ?? string.Empty,
                                                              Description = r.RoleDescription,
                                                              ApplicationId = r.ApplicationId,
                                                              Application = r.Application!.Abbrevation

                                                          }).SingleOrDefaultAsync(cancellationToken);
                if (role == null)
                {
                    return _message.RecordNotFound();
                }
                role.Permissions = await _GRepoRolePermission.FindByCondition(rp => rp.RoleId == role.Id && rp.IsDeleted == false)
                                                        .Include(x => x.Permission).ThenInclude(x => x != null ? x.Application : null)
                                                        .Select(rp => PermissionViewModel.Projection.Compile().Invoke(rp!.Permission!, _DbContext))
                                                        //.OrderBy(rp => rp.ApplicationId)
                                                        //.ThenBy(rp => rp.Controller)
                                                        //.ThenBy(rp => rp.Action)
                                                        .ToListAsync(cancellationToken);

                return new JsonResult(role);
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.Message.ToString());
            }
        }
    }
}
