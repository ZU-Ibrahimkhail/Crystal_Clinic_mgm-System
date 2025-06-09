using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails;
namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleDetail
{
    public class RoleDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ApplicationId { get; set; }
        public string Application { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<PermissionViewModel> Permissions { get; set; } = new();
    }
}
