using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Application.Look.AssetTypes.Queries.GetDetail;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.AssetTypes.Queries.GetList
{
    public class GetAssetTypeListQuery : DataTableOption, IRequest<ResponseDataTable<GetAssetTypeDetailModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? SearchBy { get; set; }
    }
}
