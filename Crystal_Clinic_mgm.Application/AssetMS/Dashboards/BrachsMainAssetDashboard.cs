using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetChildDDl;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.Dashboards
{
    #region Request

    public class BranchsMainAccountDashboardQuery : IRequest<JsonResult>
    {
    }
    #endregion

    #region Handler
    public class BranchsMainAccountDashboardHandler(IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccountTracking, IGeneralHelperRepositoryAsync _helper, ILoggedInUser _loggedInUser, IMapper _Mapper) : IRequestHandler<BranchsMainAccountDashboardQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(BranchsMainAccountDashboardQuery request, CancellationToken cancellationToken)
        {

            var ChildBranchIdList = await _helper.GetChildBranchs(_loggedInUser.BranchId);

            var AllmainAccounts = await _GRepoMainAccountTracking.FindByCondition(x => x.IsDeleted == false && ((x.OwnerUserId == _loggedInUser.Id) || ChildBranchIdList.Contains(x.BranchId ?? 0)))
                .Include(x => x.CurrencyType).ToListAsync(cancellationToken);
            var resutl = _Mapper.Map<List<GetMainAccountChildDDLModel>>(AllmainAccounts);
            return new JsonResult(resutl);
        }
    }
    #endregion


}
