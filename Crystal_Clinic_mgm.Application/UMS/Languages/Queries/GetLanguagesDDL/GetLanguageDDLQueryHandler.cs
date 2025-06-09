using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Queries.GetLanguagesDDL
{
    public class GetLanguageDDLQueryHandler : IRequestHandler<GetLanguageDDLQuery, List<GetDropDownGeneralModel>>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, Language> _genericRepository;
        private readonly IMapper _mapper;

        public GetLanguageDDLQueryHandler(IGenericRepositoryAsync<UMS_DbContext, Language> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        public async Task<List<GetDropDownGeneralModel>> Handle(GetLanguageDDLQuery request, CancellationToken cancellationToken)
        {

            var entity = await _genericRepository.FindByCondition(x => x.IsDeleted == false).ToListAsync(cancellationToken);
            var records = _mapper.Map<List<GetDropDownGeneralModel>>(entity);
            return records;
        }
    }
}
