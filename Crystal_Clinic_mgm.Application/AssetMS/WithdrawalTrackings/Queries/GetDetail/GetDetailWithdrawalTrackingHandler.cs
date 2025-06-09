using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetDetail
{
    public class GetWithdrawalTrackingDetailHandler(IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> genericRepositoryAsync, IMessage message, IMapper mapper) : IRequestHandler<GetWithdrawalTrackingDetailQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> _GenericRepositoryAsync = genericRepositoryAsync;
        private readonly IMessage _message = message;
        private readonly IMapper _Mapper = mapper;

        public async Task<JsonResult> Handle(GetWithdrawalTrackingDetailQuery request, CancellationToken cancellationToken)
        {

            var WithdrawalTracking = await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted && x.ID == request.Id).Include(x => x.CurrencyType).Include(x => x.Branch).Include(x => x.MainAccount).FirstOrDefaultAsync(cancellationToken);
            if (WithdrawalTracking == null)
                return _message.RecordNotFound();
            var resutl = _Mapper.Map<GetWithdrawalTrackingDetailModel>(WithdrawalTracking);

            return new JsonResult(resutl);
        }

    }
}
