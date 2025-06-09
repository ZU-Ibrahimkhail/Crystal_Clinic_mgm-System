using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentAndChildDepartments
{
    public class GetBranchAndChildBranchQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public int BranchId { get; set; }
    }
}
