namespace Crystal_Clinic_Mgm.Domain.Entities.Order.Look
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Porcelain Set", "Basmati Rice"
        public string Description { get; set; } = string.Empty;
        public bool IsSellable { get; set; }
        public bool IsRentable { get; set; }
        public string BaseUnit { get; set; } = string.Empty; // "kg", "set", "sqm"
        public decimal CurrentStock { get; set; }
        public decimal RealTimeAvailableStock { get; set; }
        public decimal ReorderLevel { get; set; }
        public string ImagePath { get; set; } = string.Empty;


        // Foreign Key
        public int BranchId { get; set; }
        public int CategoryId { get; set; }
        public ItemCategory? Category { get; set; }

        // Navigation
        public ICollection<ItemUnit> Units { get; set; } = [];
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public decimal CleaningStateQuantity { get; set; } = 0;
    }
}
