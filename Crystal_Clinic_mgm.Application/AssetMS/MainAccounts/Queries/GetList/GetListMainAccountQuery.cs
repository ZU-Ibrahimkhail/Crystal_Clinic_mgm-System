using MediatR;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetList
{
    public class GetMainAccountListQuery : DataTableOption, IRequest<ResponseDataTable<GetMainAccountDetailModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? SearchBy { get; set; }
        public int? CurrencyTypeId { get; set; }
    }
}
