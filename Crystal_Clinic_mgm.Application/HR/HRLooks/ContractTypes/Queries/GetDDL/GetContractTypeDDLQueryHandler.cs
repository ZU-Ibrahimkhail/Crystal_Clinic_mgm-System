using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Queries.GetDDL
{
    public class GetContractTypeDDLQueryHandler : IRequestHandler<GetContractTypeDDLQuery, List<GetDropDownGeneralModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractType> _genericRepositoryAsync;
        private readonly IMapper _mapper;

        public GetContractTypeDDLQueryHandler(IGenericRepositoryAsync<ERP_DbContext, ContractType> genericRepositoryAsync, IMapper mapper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<List<GetDropDownGeneralModel>> Handle(GetContractTypeDDLQuery request, CancellationToken cancellationToken)
        {

            var entity = await _genericRepositoryAsync.FindByCondition(x => x.IsDeleted == false).ToListAsync(cancellationToken);
            var records = _mapper.Map<List<GetDropDownGeneralModel>>(entity);
            return records;
        }
    }
}
