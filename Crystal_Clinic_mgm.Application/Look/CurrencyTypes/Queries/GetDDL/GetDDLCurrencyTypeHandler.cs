using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.Constants;

namespace Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetDDL
{
    public class GetCurrencyTypeDDLHandler(IGenericRepositoryAsync<ERP_DbContext, CurrencyType> genericRepositoryAsync, IGenericRepositoryAsync<ERP_DbContext, CurrencyExchangeRate> genericRepositoryCurrencyExchangeRate) : IRequestHandler<GetCurrencyTypeDDLQuery, List<GetDropDownGeneralModel>>
    {


        public async Task<List<GetDropDownGeneralModel>> Handle(GetCurrencyTypeDDLQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            List<int> currencyIds = [];
            Localization localize = new();
            if (request.ExchangeRateDate.HasValue)
            {
                currencyIds = [.. genericRepositoryCurrencyExchangeRate.FindByCondition(x => !x.IsDeleted && x.CreatedOn.Date == request.ExchangeRateDate.GetValueOrDefault().Date && x.ToCurrencyId == Constants.CurrencyTypes.AFN).Select(x => x.FromCurrencyId)];
            }
            List<GetDropDownGeneralModel> branchs = await genericRepositoryAsync.FindByCondition(x => !x.IsDeleted && (currencyIds.Count == 0 || currencyIds.Contains(x.ID) || x.ID == Constants.CurrencyTypes.AFN)).Select(x => new GetDropDownGeneralModel()
            {
                Name = localize.GetName(language, x),
                Code = x.Code,
                Id = x.ID,
            }).ToListAsync(cancellationToken);
            return branchs;
        }
    }
}
