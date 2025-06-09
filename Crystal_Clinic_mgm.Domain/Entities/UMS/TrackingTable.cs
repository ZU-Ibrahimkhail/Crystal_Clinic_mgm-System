using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class TrackingTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? UserAuditId { get; set; }
        public UserAudit? UserAudit { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int? ApplicationId { get; set; }
        public Applications? Application { get; set; }
        public int? PermissionId { get; set; }
        public bool IsAccessed { get; set; }
        public DateTime RequestTime { get; set; }
        public string ErrorDetails { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
