using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.AssetMS
{
    public class ExpenseTracking : AuditableEntity
    {
        public int ID { get; set; }
        public int CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public int ExpenseTypeId { get; set; }
        public ExpenseType? ExpenseType { get; set; }
        public Guid MainAccountId { get; set; }
        public MainAccount? MainAccount { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public List<string>? AttachmentPath { get; set; }
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid UserId { get; set; }
    }
}
