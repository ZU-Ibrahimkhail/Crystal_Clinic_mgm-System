namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class ReservedItem : AuditableEntity
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int ItemId { get; set; }
        public int StockId { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public bool IsCommited { get; set; }

        // Navigation properties
        public InventoryReservation? Reservation { get; set; }
        public Look.Item? Item { get; set; }
        public Stock? Stock { get; set; }
    }
}