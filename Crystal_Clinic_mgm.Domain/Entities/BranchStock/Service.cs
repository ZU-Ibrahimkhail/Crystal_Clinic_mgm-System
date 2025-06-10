namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class Service : AuditableEntity
    {
        public int ServiceId { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
        public decimal sessionRate { get; set; }
        public string? ImagePath { get; set; }

    }
}
