using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Look
{
    public class CurrencyExchangeRate : AuditableEntity
    {
        public int CurrencyExchangeRateId { get; set; }
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public decimal ExchangeRate { get; set; } // e.g., 1 USD = 75 AFN
        public CurrencyType FromCurrency { get; set; }
        public CurrencyType ToCurrency { get; set; }
    }
}