using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetChildDDl
{
    public class GetMainAccountChildDDLModel
    {

        public Guid Id { get; set; }
        public int CurrencyTypeId { get; set; }
        public string? CurrencyType { get; set; }
        public string Code { get; set; } = string.Empty;
        public double BalanceAmount { get; set; }
        public Guid OwnerUserId { get; set; }
        public string? OwnerUserName { get; set; }
        public string? OwnerPhotoPath { get; set; }
    }
}
