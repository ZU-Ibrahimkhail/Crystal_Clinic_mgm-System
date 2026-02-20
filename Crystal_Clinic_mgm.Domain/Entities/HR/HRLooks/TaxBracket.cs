namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks
{
    public class TaxBracket : AuditableEntity
    {
        public int Id { get; set; }

        public int TaxConfigurationId { get; set; }
        public TaxConfiguration? TaxConfiguration { get; set; }

        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal FlatAmount { get; set; }
        public TaxCalculationType CalculationType { get; set; } = TaxCalculationType.StepByStep;
        public int BracketOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
