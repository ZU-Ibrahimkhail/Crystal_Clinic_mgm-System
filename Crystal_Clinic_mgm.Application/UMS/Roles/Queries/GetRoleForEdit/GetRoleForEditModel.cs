namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleForEdit
{
    public class GetRoleForEditModel
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<int> PermissionIds { get; set; } = new();
    }
}
