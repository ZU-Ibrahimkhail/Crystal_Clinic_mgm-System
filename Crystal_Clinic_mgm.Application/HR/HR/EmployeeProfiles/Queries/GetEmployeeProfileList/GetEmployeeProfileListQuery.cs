using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileList
{
    public class GetEmployeeProfileListQuery : DataTableOption, IRequest<ResponseDataTable<GetEmployeeProfileListModel>>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;
        public string? SearchBy { get; set; }
    }
}
