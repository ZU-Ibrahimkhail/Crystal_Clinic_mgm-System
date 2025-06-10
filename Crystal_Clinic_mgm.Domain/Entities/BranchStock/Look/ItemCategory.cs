using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look
{
    public class ItemCategory : AuditableEntity
    {
        [Key]
        public int categoryId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Dishes", "Eatables"
        public string Description { get; set; } = string.Empty;

        // Navigation
        [JsonIgnore]
        public ICollection<Item> Items { get; set; } = [];
    }
}
