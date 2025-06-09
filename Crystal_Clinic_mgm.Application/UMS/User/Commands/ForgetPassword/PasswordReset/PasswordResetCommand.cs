using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.PasswordReset
{
    public class PasswordResetCommand : IRequest<JsonResult>
    {
        public string ID { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
