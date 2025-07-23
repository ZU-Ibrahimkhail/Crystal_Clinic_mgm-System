using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.AssetMS
{
    public class AccountTracking : AuditableEntity
    {
        public int ID { get; set; }
        public int CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double BalanceAmount { get; set; }
        public Guid MainAccountId { get; set; }
        public MainAccount? MainAccount { get; set; }
        public TrackType trackType { get; set; }
        public TransactionStatus transactionStatus { get; set; } = TransactionStatus.COMPLETED;
        // For Transfere 
        public Guid? approvedBy { get; set; }
        public Guid? toUserId { get; set; }
        public Guid? fromUserId { get; set; }

    }
    public enum TrackType
    {
        EXPENSE,
        WITHDRAW,
        DEPOSIT,
        PAYROLL,
        TRANSFER,
        INCOME
    }

    public enum TransactionStatus
    {
        PENDING,
        APPROVED,
        COMPLETED,
        REJECTED,
        CANCELED,
    }
}
