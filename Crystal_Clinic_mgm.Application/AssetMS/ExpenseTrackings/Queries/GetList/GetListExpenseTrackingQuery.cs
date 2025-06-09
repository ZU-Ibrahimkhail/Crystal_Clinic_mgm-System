using MediatR;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Queries.GetList
{
    public class GetExpenseTrackingListQuery : DataTableOption, IRequest<ResponseDataTable<GetExpenseTrackingListModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? SearchBy { get; set; }
        public Guid? MainAccountId { get; set; }
    }
}
