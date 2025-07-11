using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class DuePayment : AuditableEntity
    {
        public int DuePaymentId { get; set; }
        public int SupplierDueId { get; set; }
        public int? CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public decimal ExchangeRateToDueCurrency { get; set; }
        public decimal AmmountPaid { get; set; }
        public decimal AmountInDueCurrency { get; set; }
        public DateTime paymentDate { get; set; }
    }
}
