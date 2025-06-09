using Crystal_Clinic_Mgm.Application.UMS.User.Commands._2FA;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Domain.Entities.UMS.AuthanticationModels;

namespace Crystal_Clinic_Mgm.Application.Common.Identity
{
    public interface IUserService
    {
        Task<LoginUserReturnModel> Authenticate(SignInCommand model);
        Task<AuthenticateResponse?> RefreshToken(string language, string token, string ipAddress);
        Task<bool> RevokeToken(string token, string ipAddress);
        IEnumerable<ApplicationUser> GetAll();
        Task<ApplicationUser?> GetById(Guid id);
        Task<ApplicationUser?> GetTokenBy(string Token, string IpAddress);
        Task<Generate2FaQrCodeAndUriResponse> Generate2FaQrCodeAndUri(Guid userId);
        Task<VerifyTheCode2FaResponse> VeriyCodeAndEnable2Fa(Guid userId,string code);
        Task<VerifyTheCode2FaResponse> GenerateRecoveryCodes(Guid userId);
        Task<LoginUserReturnModel> LoginWith2Fa(LoginWith2FaRequest request);
        Task<LoginUserReturnModel> LoginWithRecoveryCode(LoginWithRecoveryCodeRequest request);
        Task<bool> Disable2Fa(Guid userId);


    }
}
