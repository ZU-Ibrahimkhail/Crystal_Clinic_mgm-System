namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileForEdit
{
    public class GetEmployeeProfileForEditModel
    {
        public int ID { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string EnglishFirstName { get; set; } = string.Empty;
        public string PashtoFirstName { get; set; } = string.Empty;
        public string EnglishSurName { get; set; } = string.Empty;
        public string PashtoSurName { get; set; } = string.Empty;
        public string EnglishFatherName { get; set; } = string.Empty;
        public string PashtoFatherName { get; set; } = string.Empty;
        public string EnglishGrandFatherName { get; set; } = string.Empty;
        public string PashtoGrandFatherName { get; set; } = string.Empty;
        //--Tazkira
        public int TazkiraTypeId { get; set; }
        public string TazkiraNo { get; set; } = string.Empty;
        public string JoldNo { get; set; } = string.Empty;
        public string PageNo { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string TemporaryAddress { get; set; } = string.Empty;
        public string PermenantAddress { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string? LeaveRemark { get; set; } = string.Empty;
        public string PersonalEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Gender { get; set; } = string.Empty;
        public string EmergencyPhoneNumber { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
