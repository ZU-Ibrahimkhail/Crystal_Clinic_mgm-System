using System.ComponentModel.DataAnnotations;

namespace Crystal_Clinic_Mgm.Domain.Entities.Order
{
    public class OrderService
    {
        [Key] 
        public int OrderServiceId { get; set; }
        public ServiceStatus Status { get; set; }    
        public int? AssignedEmployeeId { get; set; } 
        public decimal Quantity { get; set; }
        public decimal OriginalRate { get; set; }
        public decimal ActualRate { get; set; }
        public int Duration { get; set; } 
        public DurationType DurationType { get; set; }
        public DateTime? StartDate { get; set; }
        public int OrderId { get; set; }
        public Orders? Order { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

    }
}
