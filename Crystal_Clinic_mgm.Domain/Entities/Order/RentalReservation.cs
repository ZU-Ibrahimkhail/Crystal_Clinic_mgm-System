// RentalReservation Entity
using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Domain.Entities.Order.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Order
{
    public class RentalReservation
    {
        [Key]
        public int ReservationId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int BranchId { get; set; }
        public decimal ReservedQuantity { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }

        // Navigation Properties
        public Orders Order { get; set; }
        public Item Item { get; set; }
    }
}
