using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetDetail;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetList
{
    public class GetPartnersListQuery : DataTableOption, IRequest<ResponseDataTable<GetPartnersDetailModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? SearchBy { get; set; }
    }
}
