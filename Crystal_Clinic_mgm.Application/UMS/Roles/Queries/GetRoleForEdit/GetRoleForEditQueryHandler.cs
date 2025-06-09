using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleForEdit
{
    public class GetRoleForEditQueryHandler : IRequestHandler<GetRoleForEditQuery, JsonResult>
    {
        private readonly UMS_DbContext _context;
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> _GRepoRole;
        private readonly IGenericRepositoryAsync<UMS_DbContext, RolePermission> _GRepoRolePermission;

        public GetRoleForEditQueryHandler(UMS_DbContext context, IMessage message, IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> gRepoRole, IGenericRepositoryAsync<UMS_DbContext, RolePermission> gRepoRolePermission)
        {
            _context = context;
            _message = message;
            _GRepoRole = gRepoRole;
            _GRepoRolePermission = gRepoRolePermission;
        }

        public async Task<JsonResult> Handle(GetRoleForEditQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                return _message.RecordNotFound();
            }
            try
            {
                var Role = await _GRepoRole.FindByCondition(x => x.Id == request.Id && x.IsDeleted == false)
                                                          .Select(r => new GetRoleForEditModel
                                                          {
                                                              Id = r.Id,
                                                              RequestId = r.Id,
                                                              Name = r.Name ?? string.Empty,
                                                              Description = r.RoleDescription,
                                                              ApplicationId = r.ApplicationId,

                                                          })
                                                          .SingleOrDefaultAsync(cancellationToken);
                if (Role == null)
                {
                    return _message.RecordNotFound();
                }
                Role.PermissionIds = await _GRepoRolePermission.FindByCondition(rp => rp.RoleId == Role.Id && rp.IsDeleted == false)
                                                       .Select(p => p.PermissionId).ToListAsync(cancellationToken);

                return new JsonResult(Role);
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.Message.ToString());
            }
        }
    }
}
