namespace Crystal_Clinic_Mgm.Domain.Entities.Order
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Kitchen Rental"
        public string Description { get; set; } = string.Empty;
        public bool IsRentable { get; set; }
        public decimal DailyRate { get; set; }
        public decimal HourlyRate { get; set; }
        public string? ImagePath { get; set; }

        // Navigation
        public ICollection<OrderService> OrderServices { get; set; } = [];
    }
}
