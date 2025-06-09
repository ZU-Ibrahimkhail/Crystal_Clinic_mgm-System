using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetDDL
{
    public class GetPositionTitleDDLQueryHandler : IRequestHandler<GetPositionTitleDDLQuery, List<GetPositionTitleDDLModel>>
    {

        private readonly IGenericRepositoryAsync<ERP_DbContext, PositionTitle> _genericRepositoryAsync;
        private readonly IMapper _mapper;

        public GetPositionTitleDDLQueryHandler(IGenericRepositoryAsync<ERP_DbContext, PositionTitle> genericRepositoryAsync, IMapper mapper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<List<GetPositionTitleDDLModel>> Handle(GetPositionTitleDDLQuery request, CancellationToken cancellationToken)
        {

            var entity = await _genericRepositoryAsync.FindByCondition(x =>
                                                                       x.IsDeleted == false &&
                                                                       x.BranchId == request.BranchId)
                                                                        .ToListAsync(cancellationToken);
            var records = _mapper.Map<List<GetPositionTitleDDLModel>>(entity);
            return records;
        }
    }
}
