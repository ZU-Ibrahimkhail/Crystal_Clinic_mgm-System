namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    public class EmployeeDDLViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public string PersonalEmail { get; set; } = string.Empty;
        public bool HasAccount { get; set; }

    }
}
