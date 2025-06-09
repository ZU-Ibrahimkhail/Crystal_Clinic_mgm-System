using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

public class Orders : AuditableEntity
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty; // Auto-generated (e.g., ORD-2023-0001)
    public DateTime OrderDate { get; set; }
    public DateTime? ScheduledDate { get; set; } // New field for scheduling
    public OrderType OrderType { get; set; } // Changed to enum
    public decimal OriginalTotal { get; set; }
    public decimal AdjustedTotal { get; set; } // For manual adjustments
    public decimal PaidAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Draft;// Changed to enum
    public string DeliveryNotes { get; set; } = string.Empty;

    // Foreign Keys
    public int CustomerId { get; set; }
    public int? EmployeeId { get; set; } // Who created the order
    public int? BranchDetailId { get; set; }

    // Navigation
    public Customer? Customer { get; set; }
    public EmployeeProfile? Employee { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public ICollection<OrderService> OrderServices { get; set; } = [];
    public ICollection<OrderPayment> Payments { get; set; } = [];
    public ICollection<OrderAdjustment> Adjustments { get; set; } = [];
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }
}

