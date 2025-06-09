using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Order.Look;

public class OrderItem : AuditableEntity
{
    public int OrderItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReturnedQuantity { get; set; } = 0;
    public decimal DamagedQuantity { get; set; } = 0;
    public decimal OriginalPrice { get; set; } // From price list
    public decimal ActualPrice { get; set; } // After adjustments
    public decimal TotalPrice { get; set; } // After adjustments
    public bool IsPriceOverridden { get; set; }
    public bool IsRental { get; set; }
    public int? RentalDays { get; set; }
    public DateTime? RentalStartDate { get; set; }
    public DateTime? RentalEndDate { get; set; }
    public bool IsExternalItem { get; set; } // From another branch
    public int? SourceBranchId { get; set; }

    // Foreign Keys
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int? UnitId { get; set; }

    // Navigation
    public Orders? Order { get; set; }
    public Item? Item { get; set; }
    public ItemUnit? Unit { get; set; }
    public Branch? SourceBranch { get; set; }
}