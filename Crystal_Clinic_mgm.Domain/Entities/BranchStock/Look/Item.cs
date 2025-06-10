namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look
{
    public class Item : AuditableEntity
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Asprin", "witning cream", "washing machine"
        public string Description { get; set; } = string.Empty;
        public string BaseUnit { get; set; } = string.Empty; // "count", "set", "sqm"
        public decimal CurrentStock { get; set; } // for total available stock 
        public decimal? UseableStock { get; set; } // for healty stock example we have 5 washing machine only 3 is healthy 
        public decimal ReorderLevel { get; set; } // when to order or purchase this item 
        public string ImagePath { get; set; } = string.Empty;


        // Foreign Key
        public int BranchId { get; set; }
        public int CategoryId { get; set; }
        public ItemCategory? Category { get; set; } // example Medicine, Machine, etc
    }
}
