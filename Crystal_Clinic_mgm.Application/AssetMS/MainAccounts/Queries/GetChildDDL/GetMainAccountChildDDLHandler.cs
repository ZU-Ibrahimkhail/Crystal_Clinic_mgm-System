using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetChildDDl
{
    public class GetMainAccountChildDDLHandler(IGenericRepositoryAsync<ERP_DbContext, MainAccount> genericRepositoryAsync, IMapper mapper, ILoggedInUser loggedInUser) : IRequestHandler<GetMainAccountChildDDLQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GenericRepositoryAsync = genericRepositoryAsync;
        private readonly IMapper _Mapper = mapper;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;

        public async Task<JsonResult> Handle(GetMainAccountChildDDLQuery request, CancellationToken cancellationToken)
        {



            var AllmainAccounts = await _GenericRepositoryAsync.FindByCondition(x => x.IsDeleted == false && ((x.OwnerUserId == _loggedInUser.Id && request.MainAccountId == null) || x.ParentId == request.MainAccountId))
                .Include(x => x.CurrencyType).ToListAsync(cancellationToken);
            var resutl = _Mapper.Map<List<GetMainAccountChildDDLModel>>(AllmainAccounts);
            return new JsonResult(resutl);
        }
    }
}
