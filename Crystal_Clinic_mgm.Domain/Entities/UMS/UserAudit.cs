using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class UserAudit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public DateTime ActionOn { get; set; }
        public DateTime? ActionEnd { get; set; }
        //public int? BranchId { get; set; }
        // public Branch? Branch { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public bool Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public string BrowserName { get; set; } = string.Empty;
        public string BrowserVersion { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
    }
}
