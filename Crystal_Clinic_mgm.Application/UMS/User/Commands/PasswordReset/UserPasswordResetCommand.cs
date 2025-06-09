using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.PasswordReset
{
    public class UserPasswordResetCommand : IRequest<JsonResult>
    {
        public string ID { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
