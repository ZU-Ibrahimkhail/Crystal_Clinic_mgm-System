using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text.Json.Serialization;


namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDDL
{
    public class GetEmployeeProfileDDLQuery : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;
        [DefaultValue(true)]
        public bool GetAll { get; set; }
    }
}
