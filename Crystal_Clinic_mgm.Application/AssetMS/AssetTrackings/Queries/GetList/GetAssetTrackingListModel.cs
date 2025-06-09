using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;

namespace Crystal_Clinic_Mgm.Application.AssetMS.AccountTrackings.Queries.GetList
{
    public class GetAccountTrackingListModel(AccountTracking AccountTracking, IGeneralHelperRepositoryAsync _helper, string language)
    {
        public int CurrencyTypeId { get; set; } = AccountTracking.CurrencyTypeId;
        public string CurrencyType { get; set; } = new Localization().GetName(language, AccountTracking.CurrencyType);
        public Guid UserId { get; set; } = AccountTracking.UserId;
        public string UserName { get; set; } = _helper.GetUserName(language, AccountTracking.UserId);
        public DateTime TransactionDate { get; set; } = AccountTracking.TransactionDate;
        public double DebitAmount { get; set; } = AccountTracking.DebitAmount;
        public double CreditAmount { get; set; } = AccountTracking.CreditAmount;
        public double BalanceAmount { get; set; } = AccountTracking.BalanceAmount;
        public string Description { get; set; } = AccountTracking.Description ?? string.Empty;
        public TrackType trackType { get; set; } = AccountTracking.trackType;
        public GetAccountTrackingListModel GetData()
        {
            return this;
        }

    }
}
