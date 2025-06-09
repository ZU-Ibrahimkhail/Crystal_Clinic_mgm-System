namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    public class GetEmployeeModel
    {
        public int Id { get; set; }
        public string EnglishName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string EnglishFatherName { get; set; } = string.Empty;
        public string DariFatherName { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public int BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public int PositionId { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;

    }

}
