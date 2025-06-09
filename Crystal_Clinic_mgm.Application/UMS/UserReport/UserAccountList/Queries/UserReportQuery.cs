using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.UserReport.UserAccountList.Queries
{
    public class UserReportQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public int? BranchId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsManager { get; set; }

    }
}
