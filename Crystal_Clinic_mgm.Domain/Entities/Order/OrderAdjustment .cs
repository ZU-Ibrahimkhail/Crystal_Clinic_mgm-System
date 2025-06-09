using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crystal_Clinic_Mgm.Domain.Entities.Order
{
    public class OrderAdjustment : AuditableEntity
    {
        public int AdjustmentId { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public AdjustmentType AdjustmentType { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }

        // Foreign Keys
        public int OrderId { get; set; }
        public int? ItemId { get; set; } // Null for order-level adjustments
        public int? ServiceId { get; set; } // Null for order-level adjustments

        // Navigation
        public Orders Order { get; set; }
        public OrderItem? OrderItem { get; set; }
        public OrderService? OrderService { get; set; }
    }

 
}
