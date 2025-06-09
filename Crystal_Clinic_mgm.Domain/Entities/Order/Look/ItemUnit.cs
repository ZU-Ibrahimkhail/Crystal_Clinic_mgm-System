using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Domain.Entities.Order.Look
{
    public class ItemUnit
    {
        [Key]
        public int unitId { get; set; }
        public string UnitName { get; set; } = string.Empty; // e.g., "packet", "box"
        public decimal ConversionFactor { get; set; } // e.g., 1 packet = 5kg

        // Foreign Key
        public int ItemId { get; set; }
        [JsonIgnore]
        public Item? Item { get; set; }
        public int? DailyRentalPrice { get; set; }
        public int SellingPrice { get; set; }
    }
}
