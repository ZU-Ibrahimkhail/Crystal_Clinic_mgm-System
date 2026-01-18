using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class Brand : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string BrandCode { get; set; } = string.Empty;
        public string ManufacturerName { get; set; } = string.Empty;
        public string CountryOfOrigin { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string ContactInfo { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}