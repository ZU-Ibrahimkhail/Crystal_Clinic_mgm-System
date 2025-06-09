using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Application.Queries.GetApplicationList
{
    public class GetApplicationListQueryHandler : IRequestHandler<GetApplicatonListQuery, ResponseDataTable<GetApplicationListLookupModel>>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, Applications> _genericRepositoryAsync;
        public GetApplicationListQueryHandler(IGenericRepositoryAsync<UMS_DbContext, Applications> genericRepositoryAsync)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
        }
        public async Task<ResponseDataTable<GetApplicationListLookupModel>> Handle(GetApplicatonListQuery request, CancellationToken cancellationToken)
        {
            var query = await _genericRepositoryAsync.FindByCondition(x => x.IsDeleted == false)
                .Select(d => GetApplicationListLookupModel.Projection.Compile().Invoke(d)).ToListAsync(cancellationToken);
            var list = query.AsEnumerable();
            if (!string.IsNullOrEmpty(request.Title))
            {
                list = query.Where(x => x.Title.ToLower().Contains(request.Title.ToLower().Trim()));
            }

            var result = MyDataTable<GetApplicationListLookupModel>.Generate(list.OrderByDescending(x => x.ModifiedOn), request.PageSize, request.PageIndex);
            return result;


        }
    }
}
