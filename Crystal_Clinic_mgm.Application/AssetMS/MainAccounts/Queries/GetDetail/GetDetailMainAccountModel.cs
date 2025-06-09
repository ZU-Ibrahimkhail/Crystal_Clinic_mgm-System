using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDetail
{
    public class GetMainAccountDetailModel
    {

        public Guid Id { get; set; }
        public int CurrencyTypeId { get; set; }
        public string? CurrencyType { get; set; }
        public int AssetTypeId { get; set; }
        public string? AssetType { get; set; }
        public DateTime DepositDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public Guid OwnerUserId { get; set; }
        public string OwnerUserName { get; set; } = string.Empty;
        public double TotalDebitAmount { get; set; }
        public double TotalCreditAmount { get; set; }
        public double BalanceAmount { get; set; }
        public Guid? ParentId { get; set; }

    }
}
