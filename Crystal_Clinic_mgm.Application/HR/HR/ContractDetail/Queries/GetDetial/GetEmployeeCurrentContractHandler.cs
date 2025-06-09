using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetList;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Queries.GetDetial
{
    public class GetEmployeeCurrentContractHandler : IRequestHandler<GetEmployeeCurrentContractQuery, GetContractDetailsListModel>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _GenericRepositoryAsync;
        private readonly IMapper _Mapper;

        public GetEmployeeCurrentContractHandler(IGenericRepositoryAsync<ERP_DbContext, ContractDetails> genericRepositoryAsync, IMapper mapper)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _Mapper = mapper;
        }

        public async Task<GetContractDetailsListModel> Handle(GetEmployeeCurrentContractQuery request, CancellationToken cancellationToken)
        {
            var entity = _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted && x.EmployeeProfileId == request.EmployeeProfileId && x.IsActive)
                                              .OrderByDescending(x => x.CreatedOn)
                                              .Include(x => x.EmployeeProfile)
                                              .Include(x => x.ContractType)
                                              .Include(x => x.PositionTitle)
                                              .Include(x => x.Branch)
                                              .FirstOrDefault();

            return await Task.Run(() =>
            {
                var records = _Mapper.Map<GetContractDetailsListModel>(entity);
                return records;
            });
        }
    }
}