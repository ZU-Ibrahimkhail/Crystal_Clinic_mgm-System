using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetList
{
    public class GetContractDetailsListHandler : IRequestHandler<GetContractDetailsListQuery, ResponseDataTable<GetContractDetailsListModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _GenericRepositoryAsync;
        private readonly IMapper _Mapper;

        public GetContractDetailsListHandler(IGenericRepositoryAsync<ERP_DbContext, ContractDetails> genericRepositoryAsync, IMapper mapper)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _Mapper = mapper;
        }

        public async Task<ResponseDataTable<GetContractDetailsListModel>> Handle(GetContractDetailsListQuery request, CancellationToken cancellationToken)
        {
            var entity = _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted && x.EmployeeProfileId == request.EmployeeProfileId)
                                                .OrderByDescending(x => x.ModifiedOn)
                                                .Include(x => x.EmployeeProfile)
                                                .Include(x => x.ContractType)
                                                .Include(x => x.PositionTitle)
                                                .Include(x => x.Branch);
            var records = _Mapper.Map<IEnumerable<GetContractDetailsListModel>>(entity);

            return await Task.Run(() =>
            {

                if (!string.IsNullOrEmpty(request.SearchBy))
                {
                    var searchBy = request.SearchBy.ToLower().Trim();

                    records = records.Where(x =>
                        x.ContractType.ToLower().Contains(searchBy) ||
                        x.PositionTitle.ToLower().Contains(searchBy) ||
                        x.Branch.ToLower().Contains(searchBy)
                        );

                }

                return MyDataTable<GetContractDetailsListModel>.Generate(records, request.PageSize, request.PageIndex);
            });

        }
    }
}
