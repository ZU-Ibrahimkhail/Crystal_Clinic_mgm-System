namespace Crystal_Clinic_Mgm.Domain.Entities.Look
{
    public class Branch : LookAndAuditableEntity
    {
        public int? ParentId { get; set; }
        public virtual Branch? Parent { get; set; }
        public bool IsActive { get; set; }
        public string? Address { get; set; }

    }
}
