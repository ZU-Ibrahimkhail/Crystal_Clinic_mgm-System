namespace Crystal_Clinic_Mgm.Application.Accounting.Services
{
    public interface IFinancialConfigurationService
    {
        int GetBaseCurrencyId();
        string GetBaseCurrencyCode();
    }
}
