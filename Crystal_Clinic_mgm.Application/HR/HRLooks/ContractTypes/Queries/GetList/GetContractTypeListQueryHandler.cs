using AutoMapper;
using MediatR;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Queries.GetList
{
    public class GetContractTypeListQueryHandler : IRequestHandler<GetContractTypeListQuery, ResponseDataTable<GeneralLookListModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractType> _genericRepositoryAsync;
        private readonly IMapper _mapper;

        public GetContractTypeListQueryHandler(IGenericRepositoryAsync<ERP_DbContext, ContractType> genericRepositoryAsync, IMapper mapper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<ResponseDataTable<GeneralLookListModel>> Handle(GetContractTypeListQuery request, CancellationToken cancellationToken)
        {
            var GradeStep = await Task.Run(() =>
            {
                string? searchBy = null;
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    searchBy = request.Name.ToLower().Trim();
                }
                return _genericRepositoryAsync.FindByCondition(x => x.IsDeleted == false
                                    &&
                                    (searchBy == null ||
                                    x.EnglishName.ToLower().Contains(searchBy) ||
                                    x.PashtoName.ToLower().Contains(searchBy) ||
                                    x.DariName.ToLower().Contains(searchBy) ||
                                    x.Code.ToLower().Contains(searchBy)
                                    )).OrderByDescending(x => x.ModifiedOn);
            });



            var entity = _mapper.Map<List<GeneralLookListModel>>(GradeStep);

            var result = MyDataTable<GeneralLookListModel>.Generate(entity, request.PageSize, request.PageIndex);
            return result;
        }
    }
}
