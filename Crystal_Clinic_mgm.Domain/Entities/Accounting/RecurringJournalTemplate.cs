namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class RecurringJournalTemplate : AuditableEntity
    {
        public int Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public RecurringFrequency Frequency { get; set; }
        public DateTime NextRunDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string Description { get; set; } = string.Empty;
        public ICollection<RecurringJournalLine> Lines { get; set; } = new List<RecurringJournalLine>();
    }
}
