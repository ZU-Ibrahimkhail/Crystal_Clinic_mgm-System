
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class UserAllowedDocTypesSecurityLevels : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Guid UserId { get; set; }
        public string? AllowedBranchId { get; set; }

    }
}
