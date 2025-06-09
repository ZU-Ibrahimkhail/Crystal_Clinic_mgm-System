using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;

namespace Crystal_Clinic_Mgm.Application.UMS.UserReport.UserRolePermission.Queries
{
    public class UserRolePermissionModel
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Branch { get; set; }
        public int? EmployeeId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public int Language { get; set; }
        public bool IsActive { get; set; }
        public List<ListRoleViewModel> UserRoles { get; set; } = new();
    }
}
