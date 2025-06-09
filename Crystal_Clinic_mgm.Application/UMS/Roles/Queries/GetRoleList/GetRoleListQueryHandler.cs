using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList
{
    public class GetRoleListQueryHandler : IRequestHandler<GetRoleListQuery, DataTableResponse>
    {
        private readonly UMS_DbContext _DbContext;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> _GRepoRole;

        public GetRoleListQueryHandler(UMS_DbContext dbContext, IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> gRepoRole)
        {
            _DbContext = dbContext;
            _GRepoRole = gRepoRole;
        }

        public async Task<DataTableResponse> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var q = _GRepoRole.FindByCondition(r => r.IsDeleted == false);
            
            if (request.SearchBy != null)
            {
                q = q.Where(r => r.Name!.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase));
            }
            var data = q.Include(x => x.Application).Select(r => ListRoleViewModel.Projection.Compile().Invoke(r, _DbContext)).ToList();
            return await DataTable<ListRoleViewModel>.Generate(data, request.PageSize, request.PageIndex);
        }
    }
}
