using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Queries.GetList;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Queries.GetList
{
    public class GetAdvancePaymentListHandler : IRequestHandler<GetAdvancePaymentListQuery, ResponseDataTable<GetAdvancePaymentListModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> _GenericRepositoryAsync;
        private readonly IMapper _Mapper;

        public GetAdvancePaymentListHandler(IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> genericRepositoryAsync, IMapper mapper)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _Mapper = mapper;
        }

        public async Task<ResponseDataTable<GetAdvancePaymentListModel>> Handle(GetAdvancePaymentListQuery request, CancellationToken cancellationToken)
        {
            var entity = await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted &&
           (request.MainAccountId == null || x.MainAccountId == request.MainAccountId)
           &&
           (string.IsNullOrEmpty(request.SearchBy) ||
             x.Employee!.EnglishFirstName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
             x.Employee!.EnglishSurName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
             x.Employee!.PashtoFirstName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
             x.Employee!.PashtoSurName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
             x.PayType!.EnglishName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
             x.PayType!.DariName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase) ||
             x.PayType!.PashtoName.Contains(request.SearchBy, StringComparison.CurrentCultureIgnoreCase)), request.PageIndex, request.PageSize, out int total)
           .Include(x => x.PayType)
           .Include(x => x.CurrencyType)
           .Include(x => x.Employee).ThenInclude(x => x!.Branch)
           .OrderByDescending(x => x.ModifiedOn).ToListAsync(cancellationToken);

            var records = _Mapper.Map<List<GetAdvancePaymentListModel>>(entity);

            return new ResponseDataTable<GetAdvancePaymentListModel>()
            {
                Data = records,
                TotalRecord = total,
                CurrantPage = request.PageIndex
            };
        }
    }
}
