using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Application.Queries.GetApplicationDDL
{
    public class GetApplicationDDLQueryHandler : IRequestHandler<GetApplicationDDLQuery, List<GetApplicationDDLModel>>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, Applications> _genericRepositoryAsync;
        public GetApplicationDDLQueryHandler(IGenericRepositoryAsync<UMS_DbContext, Applications> genericRepositoryAsync)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
        }
        public async Task<List<GetApplicationDDLModel>> Handle(GetApplicationDDLQuery request, CancellationToken cancellationToken)
        {
            var entity = await _genericRepositoryAsync.FindByCondition(x => x.IsDeleted == false).Select(x => new GetApplicationDDLModel()
            {
                Id = x.ID,
                ApplicationName = x.Title,
            }).ToListAsync(cancellationToken);
            return entity;
        }
    }
}