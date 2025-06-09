using Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin
{
    public class SignedInUserDetails
    {
        public int? BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public List<PermissionViewModel> Permissions { get; set; } = new();
    }
}
