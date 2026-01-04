namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class Shareholder : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal OwnershipPercentage { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal TotalDrawings { get; set; } = 0;
        public string ContactInfo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<EquityTransaction> Transactions { get; set; } = new List<EquityTransaction>();
    }
}
