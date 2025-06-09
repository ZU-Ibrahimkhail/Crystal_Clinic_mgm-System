using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.AssetTypes.Queries.GetDDL
{
    public class GetAssetTypeDDLHandler(IGenericRepositoryAsync<ERP_DbContext, AssetType> genericRepositoryAsync) : IRequestHandler<GetAssetTypeDDLQuery, List<GetDropDownGeneralModel>>
    {

        private readonly IGenericRepositoryAsync<ERP_DbContext, AssetType> _GenericRepositoryAsync = genericRepositoryAsync;

        public async Task<List<GetDropDownGeneralModel>> Handle(GetAssetTypeDDLQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            Localization localize = new();
            List<GetDropDownGeneralModel> branchs = await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted).Select(x => new GetDropDownGeneralModel()
            {
                Name = localize.GetName(language, x),
                Code = x.Code,
                Id = x.ID,
            }).ToListAsync(cancellationToken);
            return branchs;
        }
    }
}
