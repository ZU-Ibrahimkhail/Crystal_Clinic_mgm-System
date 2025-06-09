using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDetail
{
    public class GetMainAccountDetailHandler(IGenericRepositoryAsync<ERP_DbContext, MainAccount> genericRepositoryAsync, IMessage message, IMapper mapper) : IRequestHandler<GetMainAccountDetailQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GenericRepositoryAsync = genericRepositoryAsync;
        private readonly IMessage _message = message;
        private readonly IMapper _Mapper = mapper;

        public async Task<JsonResult> Handle(GetMainAccountDetailQuery request, CancellationToken cancellationToken)
        {



                var AllmainAccounts = _GenericRepositoryAsync.FindByCondition(x => x.IsDeleted == false).Include(x => x.CurrencyType);
                var mainAccount = AllmainAccounts.FirstOrDefault(x => x.ID == request.Id);
                if (mainAccount == null)
                    return _message.RecordNotFound();
                var resutl = _Mapper.Map<GetMainAccountDetailModel>(mainAccount);
                return new JsonResult(resutl);
        }
    }
}
