using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class PositionTitle : LookAndAuditableEntity
    {
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public bool IsActive { get; set; } = true;


    }
}
