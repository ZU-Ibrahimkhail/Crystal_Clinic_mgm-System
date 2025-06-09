
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class Permission : AuditableEntity
    {
        public Permission()
        {
            //RolePermission = new HashSet<RolePermission>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string ActionCategory { get; set; } = string.Empty;
        public int ApplicationId { get; set; }
        public Applications? Application { set; get; }
        public bool IsGlobal { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
