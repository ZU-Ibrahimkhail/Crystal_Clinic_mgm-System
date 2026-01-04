namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class RecurringJournalLine : AuditableEntity
    {
        public int Id { get; set; }
        public int RecurringJournalTemplateId { get; set; }
        public RecurringJournalTemplate RecurringJournalTemplate { get; set; } = null!;
        public int ChartOfAccountId { get; set; }
        public ChartOfAccounts ChartOfAccount { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; } = 0;
        public decimal CreditAmount { get; set; } = 0;
    }
}
