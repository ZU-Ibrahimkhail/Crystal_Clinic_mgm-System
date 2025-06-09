using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands._2FA
{
    public class LoginWith2FaRequest
    {
        [DefaultValue("IsSuperAdmin@SuperAdmin.com")]
        public string Email { get; set; }
        [DefaultValue("Crystal_ClinicMgm12345!@#$%")]
        public string Password { get; set; } 
        public string TwoFactorCode { get; set; }
        public bool RememberMe { get; set; }
        [JsonIgnore]
        public string? IpAddress { get; set; }
        [JsonIgnore]
        public string? Language { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }

}
