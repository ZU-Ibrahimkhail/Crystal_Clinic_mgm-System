namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{

    public class RolePermission : AuditableEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public virtual ApplicationRole? Role { get; set; }
        public virtual Permission? Permission { get; set; }
    }
}
