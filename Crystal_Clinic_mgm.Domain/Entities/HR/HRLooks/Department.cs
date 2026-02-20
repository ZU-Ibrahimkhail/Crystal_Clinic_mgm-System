namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class Department : LookAndAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? DeptCode { get; set; }
        public int? ParentDepartmentId { get; set; }
        public Department? ParentDepartment { get; set; }
        public int? HeadEmployeeId { get; set; }

        public ICollection<Department> ChildDepartments { get; set; } = new List<Department>();
        public ICollection<HR.EmployeeProfile> Employees { get; set; } = new List<HR.EmployeeProfile>();
    }
}
