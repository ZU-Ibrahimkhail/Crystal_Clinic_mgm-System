using Microsoft.AspNetCore.Identity;
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public int? EmployeeId { get; set; }
        public int? BranchId { get; set; }
        public bool IsActive { get; set; }
        public int SuccessLoginCount { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string? Remarks { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsSuperAdmin { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsBranchAdmin { get; set; } = false;
    }
}
