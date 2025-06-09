using Microsoft.AspNetCore.Identity;
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class UserRole : IdentityUserRole<int>
    {

        public int ID { get; set; }
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public int RoleId { get; set; }
        public Guid UserId { get; set; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        public virtual ApplicationUser User { get; set; } = new();
        public virtual ApplicationRole Role { get; set; } = new();
        public string? Remarks { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }

    }
}
