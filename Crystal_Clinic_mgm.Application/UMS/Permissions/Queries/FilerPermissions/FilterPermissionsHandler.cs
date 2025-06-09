using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.FilerPermissions
{
    public class FilterPermissionsHandler : IRequestHandler<FilterPermissions, JsonResult>
    {
        private readonly UMS_DbContext _DbContext;
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Permission> _genericRepositoryAsync;
        public FilterPermissionsHandler(UMS_DbContext dbContext, IMessage message, IGenericRepositoryAsync<UMS_DbContext, Permission> genericRepositoryAsync)
        {
            _DbContext = dbContext;
            _message = message;
            _genericRepositoryAsync = genericRepositoryAsync;
        }
        public async Task<JsonResult> Handle(FilterPermissions request, CancellationToken cancellationToken)
        {
            try
            {
                var q = _genericRepositoryAsync.FindByCondition(x => x.IsDeleted == false);
                if (request.ApplicationId != 0)
                {
                    q = q.Where(p => p.ApplicationId == request.ApplicationId);
                }
                if (request.ControllerName != null)
                {
                    q = q.Where(p => p.Controller.ToLower().Contains(request.ControllerName.ToLower().Trim()));
                }
                var data = await q.OrderBy(x => x.ApplicationId).ThenBy(x => x.Controller).ThenBy(x => x.Method)
                            .Include(x => x.Application)
                            .Select(p => PermissionViewModel.Projection.Compile().Invoke(p, _DbContext))
                            .ToListAsync(cancellationToken);
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
    }
}
