using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.General
{
    public class NewsNotification : AuditableEntity
    {
        public int ID { get; set; }
        public News? News { get; set; }
        public int NewsId { get; set; }
        public Branch? Branch { get; set; }
        public int? BranchId { get; set; }

    }
}
