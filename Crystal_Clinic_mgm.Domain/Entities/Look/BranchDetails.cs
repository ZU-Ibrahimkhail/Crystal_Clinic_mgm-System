using System.ComponentModel.DataAnnotations;

namespace Crystal_Clinic_Mgm.Domain.Entities.Look
{
    public class BranchDetails
    {
        [Key]
        public int Id { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string Title { get; set; } = string.Empty;
        public string HeaderNote { get; set; } = string.Empty;
        public string FooterNote { get; set; } = string.Empty;
        public string PhoneNumbers { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
