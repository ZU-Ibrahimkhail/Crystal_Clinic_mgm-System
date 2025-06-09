using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class PayrollTracking : AuditableEntity
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeProfile? Employee { get; set; }
        public int ContractDetailsId { get; set; }
        public ContractDetails? ContractDetails { get; set; }
        public int? PayTypeId { get; set; }
        public PayType? PayType { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int? CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public DateTime Date { get; set; }
        public double BaseSalary { get; set; }
        public double AdvanceDeduction { get; set; }
        public double NetSalary { get; set; }
        public Guid? PayedBy { get; set; }
        public bool IsPayed { get; set; } = false;
    }
}
