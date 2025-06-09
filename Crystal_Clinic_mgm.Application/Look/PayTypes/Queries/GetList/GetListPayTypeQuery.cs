using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Application.Look.PayTypes.Queries.GetDetail;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.PayTypes.Queries.GetList
{
    public class GetPayTypeListQuery : DataTableOption, IRequest<ResponseDataTable<GetPayTypeDetailModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? SearchBy { get; set; }
    }
}
