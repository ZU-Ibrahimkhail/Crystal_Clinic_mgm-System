using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.AssetMS.AccountTrackings.Queries.GetList
{
    public class GetAccountTrackingListQuery : DataTableOption, IRequest<ResponseDataTable<GetAccountTrackingListModel>>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;
        [JsonIgnore]
        public Guid MainAccountId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now.AddDays(-90);
        public DateTime ToDate { get; set; } = DateTime.Now;
        public string SearchBy { get; set; } = string.Empty;

    }
}
