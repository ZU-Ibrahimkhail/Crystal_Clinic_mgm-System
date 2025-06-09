using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands._2FA
{
    public class LoginWithRecoveryCodeRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RecoveryCode { get; set; } = string.Empty;

        [JsonIgnore]
        public string? IpAddress { get; set; }
        [JsonIgnore]
        public string? Language { get; set; }
    }

    public class LoginWithRecoveryCodeResponse
    {
        public string Token { get; set; }
    }

}
