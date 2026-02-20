namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class PayrollComponent : LookAndAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public PayrollComponentType Type { get; set; }
        public ComponentCalculationType CalculationType { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public int? ChartOfAccountId { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<HR.EmployeePayrollComponent> EmployeeComponents { get; set; } = new List<HR.EmployeePayrollComponent>();
    }
}
