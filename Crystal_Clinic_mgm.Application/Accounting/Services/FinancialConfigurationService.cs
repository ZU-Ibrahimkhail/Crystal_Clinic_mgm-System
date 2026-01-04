using Crystal_Clinic_Mgm.Application.Common.Configuration;
using Microsoft.Extensions.Options;

namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public class FinancialConfigurationService(IOptions<FinancialSettings> options) : IFinancialConfigurationService
    {
        private readonly FinancialSettings _settings = options.Value;

        public int GetBaseCurrencyId() => _settings.BaseCurrencyId;

        public string GetBaseCurrencyCode() => _settings.BaseCurrencyCode;
    }
}
