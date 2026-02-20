namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class TaxConfiguration : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TaxType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<TaxBracket> Brackets { get; set; } = new List<TaxBracket>();
    }
}
