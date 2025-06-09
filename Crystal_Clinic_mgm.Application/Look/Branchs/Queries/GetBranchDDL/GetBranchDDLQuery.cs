using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDDL
{
    public class GetBranchDDLQuery : IRequest<List<GetDropDownGeneralModel>>
    {

        [JsonIgnore]
        public string? Language { get; set; }
        [JsonIgnore]
        public string? AllowedBranch { get; set; }


    }
}
