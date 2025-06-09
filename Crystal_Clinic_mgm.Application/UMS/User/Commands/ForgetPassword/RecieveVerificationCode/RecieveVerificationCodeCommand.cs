using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.RecieveVerificationCode
{
    public class RecieveVerificationCodeCommand : IRequest<JsonResult>
    {
        public string Email { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
    }
}
