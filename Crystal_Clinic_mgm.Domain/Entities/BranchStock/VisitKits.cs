using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class VisitKits : AuditableEntity
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public int? ServiceSessionId { get; set; }
        public int KitId { get; set; }
        public InventoryKit InventoryKit { get; set; } = null!;
        public Visit Visit { get; set; } = null!;
        public ServiceSessions ServiceSessions { get; set; } = null!;
    }
}
