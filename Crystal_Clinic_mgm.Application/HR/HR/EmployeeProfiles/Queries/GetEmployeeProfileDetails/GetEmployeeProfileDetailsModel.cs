using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDetails
{
    public class GetEmployeeProfileDetailsModel
    {
        public int ID { get; set; }
        public string EnglishFirstName { get; set; } = string.Empty;
        public string PashtoFirstName { get; set; } = string.Empty;
        public string EnglishSurName { get; set; } = string.Empty;
        public string PashtoSurName { get; set; } = string.Empty;
        public string EnglishFatherName { get; set; } = string.Empty;
        public string PashtoFatherName { get; set; } = string.Empty;
        public string EnglishGrandFatherName { get; set; } = string.Empty;
        public string PashtoGrandFatherName { get; set; } = string.Empty;
        public int TazkiraTypeId { get; set; }
        public string TazkiraNo { get; set; } = string.Empty;
        public string JoldNo { get; set; } = string.Empty;
        public string PageNo { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string TemporaryAddress { get; set; } = string.Empty;
        public string PermenantAddress { get; set; } = string.Empty;
        public GetDropDownGeneralModel? Branch { get; set; }
        public string? Gender { get; set; }
        public string BloodGroup { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string? LeaveRemark { get; set; } = string.Empty;
        public string PersonalEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmergencyPhoneNumber { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
