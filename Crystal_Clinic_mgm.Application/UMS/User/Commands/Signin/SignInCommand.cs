using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin
{
    public class SignInCommand : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;

        [DefaultValue("IsSuperAdmin@SuperAdmin.com")]
        public string Email { get; set; } = string.Empty;
        [DefaultValue("Crystal_ClinicMgm12345!@#$%")]
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public string? IpAddress { get; set; }
        public bool RememberMe { get; set; }
        //public string? lang { get; set; }

    }
}
