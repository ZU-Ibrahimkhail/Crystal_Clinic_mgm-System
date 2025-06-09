using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetDetail
{
    public class GetExpenseTrackingDetailHandler(IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> genericRepositoryAsync, IMessage message, IMapper mapper) : IRequestHandler<GetExpenseTrackingDetailQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> _GenericRepositoryAsync = genericRepositoryAsync;
        private readonly IMessage _message = message;
        private readonly IMapper _Mapper = mapper;

        public async Task<JsonResult> Handle(GetExpenseTrackingDetailQuery request, CancellationToken cancellationToken)
        {

            var ExpenseTracking = await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted && x.ID == request.Id).Include(x => x.CurrencyType).Include(x => x.Branch).Include(x => x.MainAccount).Include(x => x.ExpenseType).FirstOrDefaultAsync(cancellationToken);
            if (ExpenseTracking == null)
                return _message.RecordNotFound();
            var resutl = _Mapper.Map<GetExpenseTrackingDetailModel>(ExpenseTracking);

            return new JsonResult(resutl);
        }

    }
}
