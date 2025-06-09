using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ChangePassword
{
    public class UserChangePasswordCommand : IRequest<JsonResult>
    {
        public string ID { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

}
