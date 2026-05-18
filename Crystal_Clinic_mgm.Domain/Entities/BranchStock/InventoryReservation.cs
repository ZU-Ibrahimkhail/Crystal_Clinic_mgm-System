using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class InventoryReservation : AuditableEntity
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public Visit Visit { get; set; }
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
        public string IdempotencyToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public Guid RequestedBy { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Active;
        public int? BranchId { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Navigation properties
        public ICollection<ReservedItem> ReservedItems { get; set; } = new List<ReservedItem>();
    }

    public enum ReservationStatus
    {
        Active,
        Committed,
        Released,
        Expired
    }
}