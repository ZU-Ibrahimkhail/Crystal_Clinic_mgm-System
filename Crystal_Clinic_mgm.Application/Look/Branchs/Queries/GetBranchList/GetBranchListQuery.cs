using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentList
{
    public class GetBranchListQuery : DataTableOption, IRequest<ResponseDataTable<GetBranchDetailModel>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public string? Name { get; set; }
        public int? Id { get; set; }
    }
}
