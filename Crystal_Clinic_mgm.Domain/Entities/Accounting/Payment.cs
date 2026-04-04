using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class Payment : AuditableEntity
    {
        public int Id { get; set; }
        public int AccountsPayableId { get; set; }
        public AccountsPayable AccountsPayable { get; set; } = null!;
        public int? CurrencyId { get; set; }
        public CurrencyType? Currency { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentNumber { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal ExchangeRate { get; set; } = 1;
        public decimal AmountInBaseCurrency { get; set; }
    }
}
