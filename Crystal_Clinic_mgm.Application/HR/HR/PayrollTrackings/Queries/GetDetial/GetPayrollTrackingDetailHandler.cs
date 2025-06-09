using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetDetial
{
    public class GetPayrollTrackingDetailHandler : IRequestHandler<GetPayrollTrackingDetailQuery, GetPayrollTrackingListModel>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, PayrollTracking> _GenericRepositoryAsync;
        private readonly IMapper _Mapper;

        public GetPayrollTrackingDetailHandler(IGenericRepositoryAsync<ERP_DbContext, PayrollTracking> genericRepositoryAsync, IMapper mapper)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _Mapper = mapper;
        }

        public async Task<GetPayrollTrackingListModel> Handle(GetPayrollTrackingDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted && x.ID == request.Id)
            .Include(x => x.Branch)
            .Include(x => x.PayType)
            .Include(x => x.CurrencyType)
            .Include(x => x.Employee)
            .Include(x => x.ContractDetails).ThenInclude(x => x!.PositionTitle)
            .OrderByDescending(x => x.ModifiedOn).FirstOrDefaultAsync(cancellationToken);

            return _Mapper.Map<GetPayrollTrackingListModel>(entity);
        }
    }
}