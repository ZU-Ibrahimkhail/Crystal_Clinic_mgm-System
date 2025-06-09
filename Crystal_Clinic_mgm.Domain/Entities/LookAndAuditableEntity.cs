using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Crystal_Clinic_Mgm.Domain.Entities
{
    public class LookAndAuditableEntity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string EnglishName { get; set; } = string.Empty;
        [Required]
        public string PashtoName { get; set; } = string.Empty;
        [Required]
        public string DariName { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
