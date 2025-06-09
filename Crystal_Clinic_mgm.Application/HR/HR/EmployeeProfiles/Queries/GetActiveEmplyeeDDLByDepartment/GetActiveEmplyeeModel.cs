namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetActiveEmplyeeDDLByDepartment
{
    public class GetActiveEmplyeeModel
    {
        public int ContractId { get; set; }
        public int EmployeeProfileId { get; set; }
        public string EmployeeFullName { get; set; } = string.Empty;
        public string PashtoFirstName { get; set; } = string.Empty;
        public string PashtoSurName { get; set; } = string.Empty;
        public string EmployeePosition { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
    }
}
