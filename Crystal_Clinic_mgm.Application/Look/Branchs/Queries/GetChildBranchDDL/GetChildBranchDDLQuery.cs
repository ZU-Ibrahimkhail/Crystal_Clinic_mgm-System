using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetChildDepartmentDDL
{
    public class GetChildBranchDDLQuery : IRequest<List<GetDropDownGeneralModel>>
    {


        public string? AllowedBranch { get; set; }
        [JsonIgnore]
        public string? Language { get; set; }

    }
}
