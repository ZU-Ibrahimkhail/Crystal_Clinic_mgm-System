using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetAllPermissions
{
    public class GetPermissionsListQueryHandler : IRequestHandler<GetPermissionsListQuery, DataTableResponse>
    {
        private readonly UMS_DbContext _DbContext;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Permission> _genericRepositoryAsync;

        public GetPermissionsListQueryHandler(UMS_DbContext context, IGenericRepositoryAsync<UMS_DbContext, Permission> genericRepositoryAsync)
        {
            _DbContext = context;
            _genericRepositoryAsync = genericRepositoryAsync;
        }

        public async Task<DataTableResponse> Handle(GetPermissionsListQuery request, CancellationToken cancellationToken)
        {


            var q = _genericRepositoryAsync.FindByCondition(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                string search = request.SearchBy.ToLower().Trim();
                q = q.Where(p =>
                                 p.Name.ToLower().Contains(search) ||
                                 p.Controller.ToLower().Contains(search) ||
                                 (p.Application != null && p.Application.Title.ToLower().Contains(search))
                              );
            }
            var data = q.Include(x => x.Application).Select(p => PermissionViewModel.Projection.Compile().Invoke(p, _DbContext));
            return await DataTable<PermissionViewModel>.Generate(data, request.PageSize, request.PageIndex);
        }
    }
}
