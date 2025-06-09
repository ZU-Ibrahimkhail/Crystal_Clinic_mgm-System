using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.Services.Repositories;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.AssetMS.Dashboards.BarChartofAssetByAssetTypesAndUser
{
    public class BarChartofAssetByAssetTypesAndUserHandler(IGenericDashboardRepository<MainAccount, CurrencyType> dashboardRepository, IGenericRepositoryAsync<ERP_DbContext, AssetType> gRepoAssetType, IHttpContextAccessor contextAccessor) : IRequestHandler<BarChartofAssetByAssetTypesAndUserQuery, JsonResult>
    {
        private readonly IGenericDashboardRepository<MainAccount, CurrencyType> _dashboardRepository = dashboardRepository;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AssetType> _GRepoAssetType = gRepoAssetType;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        public async Task<JsonResult> Handle(BarChartofAssetByAssetTypesAndUserQuery request, CancellationToken cancellationToken)
        {
            Localization localize = new Localization(_contextAccessor);
            var assetTypes = await _GRepoAssetType.FindByCondition(x => !x.IsDeleted).ToListAsync(cancellationToken);
            List<string> BranchNames = [];

            List<Expression<Func<MainAccount, bool>>> expressions = new();
            foreach (var branch in assetTypes)
            {
                BranchNames.Add(localize.GetName(branch));
                expressions.Add(x => x.BranchId == branch.ID);
            }
            var result = await _dashboardRepository.GetBarChartData(expressions, BranchNames, cancellationToken);
            return new JsonResult(result);
        }
    }
}
