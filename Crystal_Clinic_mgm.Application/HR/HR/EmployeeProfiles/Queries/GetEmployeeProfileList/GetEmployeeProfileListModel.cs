namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileList
{
    public class GetEmployeeProfileListModel
    {
        public int Id { get; set; }
        public int CurrencyTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string PersonalEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
        public bool HasAccount { get; set; }
        public string PhotoPath { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;

    }
}
