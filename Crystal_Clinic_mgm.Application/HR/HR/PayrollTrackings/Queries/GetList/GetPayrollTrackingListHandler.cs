using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetList
{
    public class GetPayrollTrackingListHandler(IGenericRepositoryAsync<ERP_DbContext, PayrollTracking> genericRepositoryAsync, IMapper mapper) : IRequestHandler<GetPayrollTrackingListQuery, ResponseDataTable<GetPayrollTrackingListModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, PayrollTracking> _GenericRepositoryAsync = genericRepositoryAsync;
        private readonly IMapper _Mapper = mapper;

        public async Task<ResponseDataTable<GetPayrollTrackingListModel>> Handle(GetPayrollTrackingListQuery request, CancellationToken cancellationToken)
        {
            var entity = await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted &&
            (request.BranchId == null || x.BranchId == request.BranchId)
            &&
            (string.IsNullOrEmpty(request.SearchBy) ||
              x.Employee!.EnglishFirstName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
              x.Employee!.EnglishSurName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
              x.Employee!.PashtoFirstName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
              x.Employee!.PashtoSurName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
              x.ContractDetails!.PositionTitle!.EnglishName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
              x.ContractDetails!.PositionTitle!.DariName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
              x.ContractDetails!.PositionTitle!.PashtoName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase)), request.PageIndex, request.PageSize, out int total)
            .Include(x => x.Branch)
            .Include(x => x.PayType)
            .Include(x => x.CurrencyType)
            .Include(x => x.Employee)
            .Include(x => x.ContractDetails).ThenInclude(x => x!.PositionTitle)
            .OrderByDescending(x => x.ModifiedOn).ToListAsync(cancellationToken);


            var records = _Mapper.Map<List<GetPayrollTrackingListModel>>(entity);

            return new ResponseDataTable<GetPayrollTrackingListModel>()
            {
                Data = records,
                TotalRecord = total,
                CurrantPage = request.PageIndex
            };
        }
    }
}
