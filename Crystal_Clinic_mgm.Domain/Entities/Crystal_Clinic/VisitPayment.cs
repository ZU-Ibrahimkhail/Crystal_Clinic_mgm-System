using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class VisitPayment : AuditableEntity
    {
        public int visitPaymentId { get; set; }
        public int visitId { get; set; }
        public int? serviceId { get; set; }
        public Service? service { get; set; }
        public int? CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public decimal ExchangeRateToAFN { get; set; }
        public int sessionNumber { get; set; } 
        public decimal amountPaid { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public PaymentType paymentType { get; set; }
        public DateTime paymentDate { get; set; }
        public decimal AmountInAFN { get; set; }
        public decimal RefundAmountInAFN { get; set; } = 0;
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Completed,
        FullyRefunded,
        PartiallyRefunded
    }

    public enum PaymentType
    {
        Service,
        Medication,
        General
    }
}
