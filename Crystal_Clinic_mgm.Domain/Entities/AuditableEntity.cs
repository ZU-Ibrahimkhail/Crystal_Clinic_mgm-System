using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Crystal_Clinic_Mgm.Domain.Entities
{
    public class AuditableEntity
    {
        [Required]
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? Remarks { get; set; }
    }
}
