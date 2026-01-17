using System.ComponentModel.DataAnnotations;

namespace Crystal_Clinic_Mgm.Domain.Entities.Look
{
    public class Attachments : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string FileExtention { get; set; } = string.Empty;
        public AttachmentType AttachmentType { get; set; }
        public string AttachmentDescription { get; set; } = string.Empty;
    }
}
