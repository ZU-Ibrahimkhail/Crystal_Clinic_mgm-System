using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.Dashboard.ActiveUsersSpiderChart
{
    public class ActiveUsersSpiderChartQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
    }
}
