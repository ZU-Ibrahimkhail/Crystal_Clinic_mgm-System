using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.AssetMS
{
    public class MainAccount : AuditableEntity
    {
        public Guid ID { get; set; }
        public int CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        //public int? AssetTypeId { get; set; }
        //public AssetType? AssetType { get; set; }
        public DateTime DepositDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid OwnerUserId { get; set; }
        public double TotalDebitAmount { get; set; } = 0;
        public double TotalCreditAmount { get; set; } = 0;
        public double BalanceAmount { get; set; } = 0;
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public Guid? ParentId { get; set; }
        public MainAccount? Parent { get; set; }
    }
}
