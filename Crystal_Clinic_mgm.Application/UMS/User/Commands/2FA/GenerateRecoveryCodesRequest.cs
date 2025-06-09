using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands._2FA
{
    public class GenerateRecoveryCodesRequest
    {
        public string UserId { get; set; }
    }

    public class GenerateRecoveryCodesResponse
    {
        public IEnumerable<string> RecoveryCodes { get; set; }
    }

}
