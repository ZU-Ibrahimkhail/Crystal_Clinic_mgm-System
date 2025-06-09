using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.Report.Queries
{
    public class EmployeeReportQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public int? ProvinceId { get; set; }
        public int? BranchId { get; set; }
        public int? HealthStatusId { get; set; }
        public bool? HasAccount { get; set; }
        public bool NeedReport { get; set; } = false;
        public string? GenerateReportType { get; set; } = "pdf";
    }
}
