using MediatR;
using Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Queries.GetList
{
    public class GetWithdrawalTrackingListQuery : DataTableOption, IRequest<ResponseDataTable<GetWithdrawalTrackingListModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? SearchBy { get; set; }
        public Guid? MainAccountId { get; set; }
    }
}
