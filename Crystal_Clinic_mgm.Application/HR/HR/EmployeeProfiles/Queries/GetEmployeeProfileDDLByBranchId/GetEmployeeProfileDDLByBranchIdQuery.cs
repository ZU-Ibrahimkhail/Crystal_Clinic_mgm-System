using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDDLByDepartmentId
{
    public class GetEmployeeProfileDDLByBranchIdQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;
        public int BranchId { get; set; }
    }
}
