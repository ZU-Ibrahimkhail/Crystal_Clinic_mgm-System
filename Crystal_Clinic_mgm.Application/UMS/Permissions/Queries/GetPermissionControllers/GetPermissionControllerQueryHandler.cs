using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionControllers
{
    public class GetPermissionControllerQueryHandler : IRequestHandler<GetPermissionControllerQuery, JsonResult>
    {
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Permission> _GRepoPermission;

        public GetPermissionControllerQueryHandler(IMessage message, IGenericRepositoryAsync<UMS_DbContext, Permission> gRepoPermission)
        {
            _message = message;
            _GRepoPermission = gRepoPermission;
        }

        public async Task<JsonResult> Handle(GetPermissionControllerQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var q = _GRepoPermission.FindByCondition(x => x.IsDeleted == false);
                if (request.ApplicationId != 0)
                {
                    q = q.Where(p => p.ApplicationId == request.ApplicationId);
                }
                var data = await q.GroupBy(a => a.Controller).Select(p => p.Key).ToListAsync(cancellationToken);
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
    }
}
