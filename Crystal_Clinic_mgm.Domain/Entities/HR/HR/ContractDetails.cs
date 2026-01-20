using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class ContractDetails : AuditableEntity
    {
        public int ID { get; set; }

        public int EmployeeProfileId { get; set; }
        public EmployeeProfile? EmployeeProfile { get; set; }
        public int ContractTypeId { get; set; }
        public ContractType? ContractType { get; set; }
        public int PositionTitleId { get; set; }
        public PositionTitle? PositionTitle { get; set; }
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public double SalaryAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<string>? AttachmentPath { get; set; }
    }
}
