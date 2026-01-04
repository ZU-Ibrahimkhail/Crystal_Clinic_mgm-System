namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class EquityTransaction : AuditableEntity
    {
        public int Id { get; set; }
        public int ShareholderId { get; set; }
        public Shareholder Shareholder { get; set; } = null!;
        public EquityTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }
}
