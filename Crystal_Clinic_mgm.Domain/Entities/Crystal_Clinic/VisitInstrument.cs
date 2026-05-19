using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class VisitInstrument : AuditableEntity
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public Visit Visit { get; set; } = null!;
        public int? KitId { get; set; }
        public InventoryKit? InventoryKit { get; set; }
        public int? ServiceSessionsId { get; set; }
        public ServiceSessions? ServiceSessions { get; set; }
        public int? ItemId { get; set; }
        public Item? Item { get; set; }
        public bool IsFreeForPatient { get; set; }
        public int? InventoryReservationId { get; set; }
        public InventoryReservation? InventoryReservation { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
