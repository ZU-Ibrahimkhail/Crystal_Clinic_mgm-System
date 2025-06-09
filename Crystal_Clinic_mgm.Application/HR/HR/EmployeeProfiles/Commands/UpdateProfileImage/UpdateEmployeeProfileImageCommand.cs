using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.UpdateProfileImage
{
    public class UpdateEmployeeProfileImageCommand : IRequest<JsonResult>
    {
        [JsonIgnore]
        public int ID { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
    }
}
