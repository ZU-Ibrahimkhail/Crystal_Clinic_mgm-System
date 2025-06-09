using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class AdvancePayment : AuditableEntity
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }
        public int PayTypeId { get; set; }
        public PayType? PayType { get; set; }
        public int CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public Guid MainAccountId { get; set; }
        public MainAccount? MainAccount { get; set; }
        public DateTime AdvanceDate { get; set; }
        public double AdvanceAmount { get; set; }
        public double RemainingBalance { get; set; }
        public double EachInstallmentAmount { get; set; }
        public Guid PayedBy { get; set; }
    }
}
