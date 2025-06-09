using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.SendVerificationCode
{
    public class SendVerificationCodeCommand : IRequest<JsonResult>
    {
        public string Email { get; set; } = string.Empty;
    }
}
