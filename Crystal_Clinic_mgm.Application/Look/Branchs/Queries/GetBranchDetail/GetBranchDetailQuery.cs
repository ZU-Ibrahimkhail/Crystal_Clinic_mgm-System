using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail
{
    public class GetBranchDetailQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public int Id { get; set; }
    }
}
