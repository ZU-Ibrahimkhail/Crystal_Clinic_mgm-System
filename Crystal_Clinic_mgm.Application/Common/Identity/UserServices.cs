using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands._2FA;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin;
using Crystal_Clinic_Mgm.Application.UMS.UsersAudit.Create;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Domain.Entities.UMS.AuthanticationModels;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;
using System.Text;

namespace Crystal_Clinic_Mgm.Application.Common.Identity
{
    public class UserService : IUserService
    {
        #region Dependancy injection
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UMS_DbContext _umsDbContext;
        private readonly ERP_DbContext _ERPDbContext;
        private readonly TokenProvider _tokenProvider;
        private readonly IMediator _mediator;
        readonly UMSLocalizeMessage _umsLocalizeMessage;
        readonly Localization Localize = new();
        private readonly IGeneralHelperRepositoryAsync _helper;
        private bool TwoFactorAuthenticationVerified = false;
        public UserService(
            UserManager<ApplicationUser> userManager,
            UMS_DbContext umsDbContext,
            TokenProvider tokenProvider,
            IMediator mediator,
            IStringLocalizer<UMSValidationResource> umslocalizer,
            ERP_DbContext eRPDbContext,
            IGeneralHelperRepositoryAsync helper)
        {
            _userManager = userManager;
            _umsDbContext = umsDbContext;
            _tokenProvider = tokenProvider;
            _mediator = mediator;
            _umsLocalizeMessage = new(umslocalizer);
            _ERPDbContext = eRPDbContext;
            _helper = helper;
        }
        #endregion

        #region Authenticate
        public async Task<LoginUserReturnModel> Authenticate(SignInCommand model)
        {
            ApplicationUser? user;
            string message = "";
            var useraudit = new CreateUserAuditCommand
            {
                UserName = model.Email,
                Action = "Login"
            };
            LoginUserReturnModel loginUserReturnModel = new();

            //if (model.Email.Contains('@'))
                user = await _userManager.Users.Where(x => x.Email == model.Email && x.IsDeleted == false && x.IsActive).SingleOrDefaultAsync();
            //else
                //user = await _userManager.Users.Where(x => x.UserName.ToLower() == model.Email.ToLower() && x.IsDeleted == false).SingleOrDefaultAsync();

            //  var resultsss = await _UserManager.CreateAsync(User, request.Password);
            if (user == null || (user.TwoFactorEnabled && !TwoFactorAuthenticationVerified))
            {
                // Log the result
                message = user != null && user.TwoFactorEnabled ? "User Enabled Two Factor Authentication" : _umsLocalizeMessage.UserNotFound;
                useraudit.Message = message;
                useraudit.Result = false;
                await _mediator.Send(useraudit);
                loginUserReturnModel.Returnmessage = message;
                loginUserReturnModel.StatusCode = user != null && user.TwoFactorEnabled ? 302 : 401;
                return loginUserReturnModel;
            }
            var Isuserexist = await _userManager.CheckPasswordAsync(user, model.Password);
            List<NameIdViewModel> userrole = [], applicationuser = [], userpermission = [];

            if (!Constants.CheckPassword || user != null && Isuserexist == true)
            {
                useraudit.UserId = user.Id;
                if (!user.IsActive)
                {
                    //log the result
                    message = _umsLocalizeMessage.AccountLocked;
                    useraudit.Result = false;
                    useraudit.Message = message;
                    await _mediator.Send(useraudit);
                    loginUserReturnModel.Returnmessage = message;
                    loginUserReturnModel.StatusCode = 401;
                    return loginUserReturnModel; //_message.InternalSystemError(message);
                }
                message = _umsLocalizeMessage.LoggedIn;
                loginUserReturnModel.Returnmessage = message;
                useraudit.Message = message;
                useraudit.Result = true;
                int userauditId = await _mediator.Send(useraudit);

                #region Finding user's Role, Application and permissions
                if (!user.IsSuperAdmin)
                {
                    userrole = await _umsDbContext.UserRoles.Where(x => x.UserId == user.Id).Select(x => new NameIdViewModel
                    {
                        Id = x.Role.Id.ToString(),
                        Name = x.Role.Name
                    }).ToListAsync();

                    applicationuser = await _umsDbContext.UserRoles.Where(x => x.UserId == user.Id).Select(x => new NameIdViewModel
                    {
                        Id = x.Role.Application!.ID.ToString(),
                        Name = x.Role.Application!.Abbrevation,
                    }).ToListAsync();

                    userpermission = (from userRoles in _umsDbContext.UserRoles.Where(ur => ur.UserId == user.Id)
                                      join rolePermission in _umsDbContext.RolePermission on userRoles.RoleId equals rolePermission.RoleId
                                      join permission in _umsDbContext.Permission on rolePermission.PermissionId equals permission.Id
                                      select new NameIdViewModel
                                      {
                                          Id = permission.Id.ToString(),
                                          Name = permission.Name
                                      }).ToList();
                }
                #endregion

                #region Finding the corresponding Employee and representator 
                var employee = _ERPDbContext.EmployeeProfiles.Find(user.EmployeeId);
                #endregion

                var jwtToken = await _tokenProvider.BuildToken(user, employee, userauditId);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                loginUserReturnModel.Token = token.ToString(); //user.Token;
                loginUserReturnModel.TokenExpiration = DateTime.Now.AddMinutes(2);
                loginUserReturnModel.ID = user.Id;
                loginUserReturnModel.UserName = user.UserName ?? string.Empty;
                //loginUserReturnModel.PositionTitle = Localize.GetName(model.Language, employee?.Position);
                loginUserReturnModel.Email = user.Email ?? string.Empty;
                loginUserReturnModel.IsSuperAdmin = user.IsSuperAdmin;
                loginUserReturnModel.IsActive = user.IsActive;
                loginUserReturnModel.PhoneNumber = user.PhoneNumber ?? string.Empty;
                loginUserReturnModel.BranchId = user.BranchId;
                loginUserReturnModel.Branch = Localize.GetName(model.Language, _ERPDbContext.Branchs.Find(user.BranchId)); //user.Branch;
                loginUserReturnModel.PhotoPath = employee?.PhotoPath;
                loginUserReturnModel.StatusCode = 200;
                loginUserReturnModel.Userrole = userrole;
                loginUserReturnModel.Applicationuser = applicationuser;
                loginUserReturnModel.Userpermission = userpermission;
                loginUserReturnModel.EmployeeProfileId = employee?.ID ?? 0;
                loginUserReturnModel.SingalRIP = AppConfig.SignalRIP;
                // authentication successful so generate jwt and refresh tokens
                string IpAddress = model.IpAddress ?? "";
                var refreshToken = _tokenProvider.GenerateRefreshToken(user, employee, IpAddress, userauditId);
                user.RefreshToken = token;
                _umsDbContext.Users.Update(user);
                _umsDbContext.SaveChanges();
                refreshToken.Token = token;
                refreshToken.IsActive = true;
                _umsDbContext.RefreshTokenes.Add(refreshToken);
                _umsDbContext.SaveChanges();
                return loginUserReturnModel;
            }
            else
            {
                message = _umsLocalizeMessage.UserNotFound;
                useraudit.Message = message;
                useraudit.Result = false;
                await _mediator.Send(useraudit);
                loginUserReturnModel.Returnmessage = message;
                loginUserReturnModel.StatusCode = 401;
                return loginUserReturnModel;
            }
        }
        #endregion

        #region Refresh Token
        public async Task<AuthenticateResponse?> RefreshToken(string language, string token, string ipAddress)
        {
            string message = "";
            var useraudit = new CreateUserAuditCommand
            {
                Action = "RefreshToken"
            };


            var user = await _umsDbContext.Users.SingleOrDefaultAsync(t => t.RefreshToken == token && t.IsActive == true && t.IsDeleted == false);

            if (user == null) return null;

            RefreshToken? refreshToken = await _umsDbContext.RefreshTokenes.SingleOrDefaultAsync(x => x.Token == user.RefreshToken && x.IsActive && x.IsExpired == false);

            if (refreshToken == null)
                return null;
            //if (DateTime.Now > refreshToken.Expires)
            //    return null;
            #region Finding the corresponding Employee and representator 
            var employee = _ERPDbContext.EmployeeProfiles.Find(user.EmployeeId);
            #endregion

            // replace old refresh token with a new one and save
            message = _umsLocalizeMessage.LoggedIn;
            useraudit.Message = message;
            useraudit.Result = true;
            useraudit.UserName = user.UserName ?? string.Empty;
            useraudit.UserId = user.Id;
            int userauditId = await _mediator.Send(useraudit);

            var newRefreshToken = _tokenProvider.GenerateRefreshToken(user, employee, ipAddress, userauditId);
            refreshToken.Token = newRefreshToken.Token;
            refreshToken.ReplacedByToken = token;
            user.RefreshToken = newRefreshToken.Token;
            _umsDbContext.Users.Update(user);
            _umsDbContext.RefreshTokenes.Update(refreshToken);
            _umsDbContext.SaveChanges();
            var contractDetails = _ERPDbContext.ContractDetails
                .Where(x => x.EmployeeProfileId == user.EmployeeId && !x.IsDeleted && x.IsActive).Include(x => x.PositionTitle).FirstOrDefault();
            var response = new AuthenticateResponse(user, employee, Localize.GetName(language, contractDetails?.PositionTitle), token, newRefreshToken.Token);
            //response.PositionTitle = Localize.GetName(language, employee?.Position);
            response.TokenExpiration = newRefreshToken.Expires;
            return response;
        }
        #endregion

        #region Revoke Token
        public async Task<bool> RevokeToken(string token, string ipAddress)
        {
            var user = await _umsDbContext.Users.SingleOrDefaultAsync(u => u.RefreshToken == token && u.IsActive == true && u.IsDeleted == false);

            if (user == null) return false;
            var refreshToken = await _umsDbContext.RefreshTokenes.SingleOrDefaultAsync(x => x.Token == user.RefreshToken && x.CreatedByIp == ipAddress && x.IsActive == true && x.IsExpired == false);

            if (refreshToken == null) return false;
            else
            {
                refreshToken.Revoked = DateTime.Now;
                refreshToken.RevokedByIp = ipAddress;
                user.RefreshToken = null;
                _umsDbContext.Update(user);
                _umsDbContext.Update(refreshToken);
                await _umsDbContext.SaveChangesAsync();
                return true;
            }
        }
        #endregion

        #region Get All Users
        public IEnumerable<ApplicationUser> GetAll()
        {
            List<ApplicationUser> userlist = new();
            userlist = _umsDbContext.Users.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

            if (userlist != null)
                return userlist;
            else
                return new List<ApplicationUser>();
        }
        #endregion

        #region Get User By Id
        public async Task<ApplicationUser?> GetById(Guid id)
        {
            ApplicationUser user;
            if (id != Guid.Empty)
            {
                user = await _userManager.FindByIdAsync(id.ToString()) ?? new ApplicationUser();
                if (user.IsDeleted == true || user.IsActive == false)
                    return null;
                if (user != null)
                    return user;
                else
                    return null;
            }
            else
                return null;
        }
        #endregion

        #region Get Token By Token and IP address
        public async Task<ApplicationUser?> GetTokenBy(string Token, string ipAddress)
        {
            ApplicationUser? user = null;
            RefreshToken? tokens;
            if (!string.IsNullOrEmpty(Token))
            {
                tokens = await _umsDbContext.RefreshTokenes.SingleOrDefaultAsync(u => u.Token == Token && u.IsActive == true && u.CreatedByIp == ipAddress);
                if (tokens != null && DateTime.Now > tokens.Expires)
                    return null;
                if (tokens != null)
                    user = await _umsDbContext.Users.SingleOrDefaultAsync(x =>
                                             x.RefreshToken == Token && x.IsDeleted == false);
                else if (tokens != null && user != null)
                {
                    tokens.IsExpired = true;
                    user.RefreshToken = "";
                    _umsDbContext.Update(tokens);
                    _umsDbContext.Update(user);
                    await _umsDbContext.SaveChangesAsync();
                    return null;
                }
                if (user != null)
                    return user;
                else
                    return null;
            }
            else
                return null;


        }


        #endregion

        #region 2FA Codes
        public async Task<Generate2FaQrCodeAndUriResponse> Generate2FaQrCodeAndUri(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new();
            }
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var sharedKey = FormatKey(unformattedKey ?? string.Empty);
            var authenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);

            return new Generate2FaQrCodeAndUriResponse
            {
                SharedKey = sharedKey,
                AuthenticatorUri = authenticatorUri
            };
        }

        public async Task<VerifyTheCode2FaResponse> VeriyCodeAndEnable2Fa(Guid userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new("User not found!");
            }

            // Strip spaces and hyphens from the verification code
            var verificationCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2FaTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2FaTokenValid)
            {
                return new("Verification code is invalid.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            //_logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            return new VerifyTheCode2FaResponse()
            {
                StatusMessage = "Your authenticator app has been verified.",
                RecoveryCodes = recoveryCodes
            };
        }

        public async Task<VerifyTheCode2FaResponse> GenerateRecoveryCodes(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new("User not found");
            }

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                return new("2FA is not enabled for this user.");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            return new VerifyTheCode2FaResponse { RecoveryCodes = recoveryCodes, StatusMessage = "Success" };
        }

        public async Task<LoginUserReturnModel> LoginWith2Fa(LoginWith2FaRequest request)
        {
            LoginUserReturnModel loginUserReturnModel = new();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                loginUserReturnModel.Returnmessage = "User Not found!";
                loginUserReturnModel.StatusCode = 401;
                return loginUserReturnModel;
            }

            //var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);
            //if (!passwordCheck)
            //{
            //    loginUserReturnModel.Returnmessage = "Invalid login attempt.";
            //    return loginUserReturnModel;
            //}

            var is2FaEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            if (!is2FaEnabled)
            {
                loginUserReturnModel.Returnmessage = "2FA is not enabled.";
                loginUserReturnModel.StatusCode = 401;
                return loginUserReturnModel;
            }

            var twoFactorCode = request.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);


            var result = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, twoFactorCode);

            if (result)
            {
                TwoFactorAuthenticationVerified = true;
                return await Authenticate(new SignInCommand
                {
                    RememberMe = request.RememberMe,
                    Email = request.Email,
                    Password = request.Password,
                    IpAddress = request.IpAddress,
                    Language = request.Language ?? Constants.Language.English

                });
            }
            else if (user.LockoutEnabled && user.LockoutEnd > DateTime.Now)
            {

                loginUserReturnModel.Returnmessage = "User account locked out.";
                loginUserReturnModel.StatusCode = 401;
                return loginUserReturnModel;
            }
            else
            {
                loginUserReturnModel.Returnmessage = "Invalid 2FA code.";
                loginUserReturnModel.StatusCode = 401;
                return loginUserReturnModel;
            }
        }

        public async Task<LoginUserReturnModel> LoginWithRecoveryCode(LoginWithRecoveryCodeRequest request)
        {
            LoginUserReturnModel loginUserReturnModel = new();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {

                loginUserReturnModel.Returnmessage = "Invalid login attempt.";
                loginUserReturnModel.StatusCode = 401;
                return loginUserReturnModel;
            }
            var recoveryCode = request.RecoveryCode.Replace(" ", string.Empty);
            var res = await _userManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode);

            if (res.Succeeded)
            {
                TwoFactorAuthenticationVerified = true;
                return await Authenticate(new SignInCommand
                {
                    Email = request.Email,
                    Password = request.Password,
                    IpAddress = request.IpAddress,
                    Language = request.Language ?? Constants.Language.English,
                    RememberMe = false
                });
            }
            else if (user.LockoutEnabled && user.LockoutEnd > DateTime.Now)
            {

                loginUserReturnModel.Returnmessage = "User account locked out.";
                loginUserReturnModel.StatusCode = 401;
                return loginUserReturnModel;
            }
            else
            {

                loginUserReturnModel.Returnmessage = "Invalid recovery code.";
                loginUserReturnModel.StatusCode = 401;
                return loginUserReturnModel;
            }
        }

        public async Task<bool> Disable2Fa(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var disable2FaResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            return disable2FaResult.Succeeded;
        }
        #endregion

        #region Helper Methodes
        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.AsSpan(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                "otpauth://totp/{0}?secret={1}&issuer={2}&digits=6",
                email,
                unformattedKey,
                UrlEncoder.Default.Encode("Crystal_Clinic_Mgm"));
        }
        #endregion
    }
}
