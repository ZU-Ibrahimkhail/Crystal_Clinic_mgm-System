using Microsoft.AspNetCore.Identity;
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class ApplicationRole : IdentityRole<int>
    {
        public readonly int TotalPermissions;

        public int ApplicationId { get; set; }
        public Applications? Application { get; set; }
        public string RoleDescription { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public string? Remarks { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        //public virtual ICollection<UserRole> UsersRoles { get; set; }
        //public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
