using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.AssetMS
{
    public class TradeTracking : AuditableEntity
    {
        public int ID { get; set; }
        public int CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public Guid MainAccountId { get; set; }
        public MainAccount? MainAccount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid UserId { get; set; }
        public float TradeAmount { get; set; }
        public float ProfitAmount { get; set; }
        public float LossAmount { get; set; }
    }
}
