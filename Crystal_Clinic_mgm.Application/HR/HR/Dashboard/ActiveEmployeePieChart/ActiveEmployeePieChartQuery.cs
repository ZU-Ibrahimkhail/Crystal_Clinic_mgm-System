using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.Dashboard.ActiveEmployeePieChart
{
    public class ActiveEmployeePieChartQuery : IRequest<List<SpiderData>>
    {
        [JsonIgnore]
        public string? Language { get; set; }
    }
}
