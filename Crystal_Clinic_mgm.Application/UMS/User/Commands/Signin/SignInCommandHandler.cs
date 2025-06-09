using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Identity;
using Crystal_Clinic_Mgm.Domain.Entities.UMS.AuthanticationModels;


namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin
{
    public class LoginErrorReturnModel
    {
        public string Error { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }

    public class SignInCommandHandler : IRequestHandler<SignInCommand, JsonResult>
    {
        private readonly IUserService _userService;
        public SignInCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<JsonResult> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            LoginUserReturnModel user;
            user = await _userService.Authenticate(request);
            if (user.UserName == null)
            {
                LoginErrorReturnModel error = new()
                {
                    Error = user.Returnmessage,
                    StatusCode = user.StatusCode
                };
                return new JsonResult(error);
            }
            return new JsonResult(user);

        }
    }
}
