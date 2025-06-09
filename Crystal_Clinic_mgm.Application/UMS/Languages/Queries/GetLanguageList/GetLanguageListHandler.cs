using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Queries.GetLanguageList
{
    public class GetLanguageListHandler : IRequestHandler<GetLanguageListQuery, ResponseDataTable<GetDropDownGeneralModel>>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, Language> _genericRepository;
        private readonly IMapper _mapper;
        public GetLanguageListHandler(IGenericRepositoryAsync<UMS_DbContext, Language> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        public async Task<ResponseDataTable<GetDropDownGeneralModel>> Handle(GetLanguageListQuery request, CancellationToken cancellationToken)
        {
            string? searchBy = null;
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                searchBy = request.Name.ToLower().Trim();
            }
            var queryresult = await _genericRepository.FindByCondition(x => x.IsDeleted == false
            &&
            (searchBy == null ||
            x.EnglishName.ToLower().Contains(searchBy) ||
            x.PashtoName.ToLower().Contains(searchBy) ||
            x.DariName.ToLower().Contains(searchBy) ||
            x.Code.ToLower().Contains(searchBy)
             )
            ).OrderByDescending(x => x.ModifiedOn).ToListAsync(cancellationToken);

            var entity = _mapper.Map<List<GetDropDownGeneralModel>>(queryresult);
            var result = MyDataTable<GetDropDownGeneralModel>.Generate(entity, request.PageSize, request.PageIndex);
            return result;

        }
    }
}
