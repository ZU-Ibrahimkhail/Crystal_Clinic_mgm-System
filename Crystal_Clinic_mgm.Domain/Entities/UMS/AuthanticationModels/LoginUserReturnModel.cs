namespace Crystal_Clinic_Mgm.Domain.Entities.UMS.AuthanticationModels
{
    public class LoginUserReturnModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiration { get; set; }
        public Guid ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PositionTitle { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsSuperAdmin { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public string? PhotoPath { get; set; } = string.Empty;
        public List<NameIdViewModel> Userrole { get; set; } = new List<NameIdViewModel>();
        public List<NameIdViewModel> Applicationuser { get; set; } = new List<NameIdViewModel>();
        public List<NameIdViewModel> Userpermission { get; set; } = new List<NameIdViewModel>();
        public int StatusCode { get; set; }
        public string Returnmessage { get; set; } = string.Empty;
        public int EmployeeProfileId { get; set; }
        public string? SingalRIP { get; set; } = string.Empty;

    }
}
