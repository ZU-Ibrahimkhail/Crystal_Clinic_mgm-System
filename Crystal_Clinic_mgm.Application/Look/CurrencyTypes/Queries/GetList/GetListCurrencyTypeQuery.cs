using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetDetail;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetList
{
    public class GetCurrencyTypeListQuery : DataTableOption, IRequest<ResponseDataTable<GetCurrencyTypeDetailModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? SearchBy { get; set; }
    }
}
