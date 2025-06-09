using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using Crystal_Clinic_Mgm.Application.Common.Identity;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands._2FA;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.PasswordReset;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.RecieveVerificationCode;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.SendVerificationCode;
using Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin;
using Crystal_Clinic_Mgm.Application.UMS.UsersAudit.Update;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Domain.Entities.UMS.AuthanticationModels;
using Shyjus.BrowserDetection;
using System.Text.Json;

namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    public class Response
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
    //[Tags("UMS")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        readonly UMSLocalizeMessage umslocalizeMessage;
        private readonly IBrowserDetector _browserDetector;
        private readonly ILoggedInUser _loggedInUser;
        public AuthController(IUserService userService, SignInManager<ApplicationUser> signInManager, IStringLocalizer<UMSValidationResource> umsLocalizer, IBrowserDetector browserDetector, ILoggedInUser loggedInUser)
        {
            _signInManager = signInManager;
            _userService = userService;
            umslocalizeMessage = new(umsLocalizer);
            _browserDetector = browserDetector;
            _loggedInUser = loggedInUser;
        }

        /// <summary>
        /// sign in
        /// </summary>
        /// <param name="signInCommand"></param>
        /// <returns></returns>
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInCommand signInCommand)
        {
            signInCommand.IpAddress = IpAddress().ToString();
            signInCommand.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            var ReturnResult = await Mediator.Send(signInCommand);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string JsonData = JsonSerializer.Serialize(ReturnResult, options);
            var data = JObject.Parse(JsonData);
            //string returnmessage =(string)data["Value"].ToString();
            //if (!string.IsNullOrEmpty(returnmessage))
            //{
            //    return returnmessage;
            //}
            //if (data.GetValue("Value").HasValues)
            //{
            int statusCode = (int)(data["Value"]?["StatusCode"] ?? "401");
            if (statusCode == 200)
            {
                string Token = data["Value"]?["Token"]?.ToString() ?? string.Empty;
                SetTokenCookie(Token);
                //-----------------------
                string returnmessage = data["Value"]?.ToString() ?? string.Empty;
                return new ObjectResult(returnmessage);
            }
            else
            {

                return StatusCode(statusCode, data["Value"]?["Returnmessage"] ?? "You are not authunticated !");
            }
        }



        /// <summary>
        /// sign in
        /// </summary>
        /// <param name="loginWith2FaRequest"></param>
        /// <returns></returns>
        [HttpPost("SignInWith2Fa")]
        public async Task<IActionResult> SignInWith2Fa(LoginWith2FaRequest loginWith2FaRequest)
        {
            loginWith2FaRequest.IpAddress = IpAddress().ToString();
            loginWith2FaRequest.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            var ReturnResult = await _userService.LoginWith2Fa(loginWith2FaRequest);
            int statusCode = ReturnResult.StatusCode;

            JsonSerializerOptions options = new() { WriteIndented = true };
            string JsonData = JsonSerializer.Serialize(ReturnResult, options);
            if (statusCode != 401)
            {
                string Token = ReturnResult.Token ?? string.Empty;
                SetTokenCookie(Token);
                //-----------------------
                // string returnmessage = data["Value"]?.ToString() ?? string.Empty/*;*/
                return new ObjectResult(JsonData);
            }
            else
            {

                return StatusCode(statusCode, ReturnResult.Returnmessage);
            }
        }


        /// <summary>
        /// sign in
        /// </summary>
        /// <param name="loginWith2FaRequest"></param>
        /// <returns></returns>
        [HttpPost("SignInWithRecoveryCode")]
        public async Task<IActionResult> SignInWithRecoveryCode(LoginWithRecoveryCodeRequest loginWith2FaRequest)
        {
            loginWith2FaRequest.IpAddress = IpAddress().ToString();
            loginWith2FaRequest.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? string.Empty;
            var ReturnResult = await _userService.LoginWithRecoveryCode(loginWith2FaRequest);
            int statusCode = ReturnResult.StatusCode;

            JsonSerializerOptions options = new() { WriteIndented = true };
            string JsonData = JsonSerializer.Serialize(ReturnResult, options);
            if (statusCode != 401)
            {
                string Token = ReturnResult.Token ?? string.Empty;
                SetTokenCookie(Token);
                //-----------------------
                return new ObjectResult(JsonData);
            }
            else
            {

                return StatusCode(statusCode, ReturnResult.Returnmessage);
            }
        }



        /// <summary>
        /// Sign Out
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("SignOut")]
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public async Task<IActionResult> SignOut()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            UserAuditUpdateCommand command = new()
            {
                IpAddress = IpAddress().ToString(),
                BrowserName = _browserDetector.Browser?.Name ?? string.Empty,
                BrowserVersion = _browserDetector.Browser?.Version ?? string.Empty,
                UserId = _loggedInUser.Id,
                ActionEnd = DateTime.Now
            };
            var UpdateUserAudit = await Mediator.Send(command);
            if (UpdateUserAudit)
            {
                await RevokeToken(new RevokeTokenRequest { Token = token });
                await _signInManager.SignOutAsync();
                return new JsonResult(new { code = 200, message = umslocalizeMessage.SignOut });
            }
            return new JsonResult(new { code = 404, message = "Token not found" })
            {
                StatusCode = 404
            };
        }
        /// <summary>
        /// Invalid Access
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        [Route("InvalidAccess")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult InvalidAccess(string reason)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "UnAuthorized", Message = reason });
        }

        /// <summary>
        /// Not Authenticated
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("NotAuthenticated")]
        public IActionResult NotAuthenticated()
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "UnAuthenticated", Message = umslocalizeMessage.NotAuthenticated });
        }

        /// <summary>
        /// Refresh token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {

            #region Geting old refresh token and generating new one
            var refreshToken = Request.Cookies["refreshToken"] ?? string.Empty /*HttpContext.Request.Query["refreshToken"]*/;

            var language = GeneralHelper.SelectedLanauge(Request.Cookies[Constants.CultureCookies.CookiesName]);
            var response = await _userService.RefreshToken(language, refreshToken, IpAddress());

            if (response == null)
                return Unauthorized(new { message = "Your token is not found due to expire or DeActivation" });
            #endregion

            #region Serializing the response
            string JsonData = JsonSerializer.Serialize(response, new JsonSerializerOptions() { WriteIndented = true });
            #endregion

            #region setting New Coockie
            SetTokenCookie(response.RefreshToken);
            #endregion

            return new ObjectResult(JsonData);

        }
        /// <summary>
        /// Revoke Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Revoke-Token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token;
            if (!string.IsNullOrEmpty(token))
                token = Request.Cookies["refreshToken"];
            var response = await _userService.RevokeToken(token ?? string.Empty, IpAddress());
            if (!response)
                return NotFound(new { message = "Token not found" });
            return Ok(new { message = "Token revoked" });
        }

        /// <summary>
        /// Generate Qr Code Data
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GenerateQrCodeData")]
        public async Task<Generate2FaQrCodeAndUriResponse> GenerateQrCodeData()
        {
            return await _userService.Generate2FaQrCodeAndUri(_loggedInUser.Id);
        }
        /// <summary>
        /// Enable Two Factor Authentication
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("EnableTwoFactorAuthentication")]
        public async Task<VerifyTheCode2FaResponse> EnableTwoFactorAuthentication(string Code)
        {
            return await _userService.VeriyCodeAndEnable2Fa(_loggedInUser.Id, Code);
        }

        /// <summary>
        /// Generate Recovery Codes
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GenerateRecoveryCodes")]
        public async Task<VerifyTheCode2FaResponse> GenerateRecoveryCodes()
        {
            return await _userService.GenerateRecoveryCodes(_loggedInUser.Id);
        }

        /// <summary>
        /// Disable Two Factor Authentication
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("DisableTwoFactorAuthentication")]
        public async Task<bool> DisableTwoFactorAuthentication()
        {
            return await _userService.Disable2Fa(_loggedInUser.Id);

        }


        /// <summary>
        /// Get All Active User 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetAllActiveUser")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            if (!users.Any())
                return Ok(new { message = "There are no Active user in the List" });
            else
                return Ok(users);
        }

        /// <summary>
        /// Get User By id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}/GetUserById")]
        public IActionResult GetById(Guid id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user.Result);
        }

        /// <summary>
        /// Get Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetToken")]
        public async Task<IActionResult> GetToken(string? token)
        {
            var Token = token ?? Request.Cookies["refreshToken"];
            var user = await _userService.GetTokenBy(Token ?? string.Empty, IpAddress().ToString());
            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return NotFound();
            }
            return Ok(user);
        }
        // helper methods
        private void SetTokenCookie(string token)
        {
            //DeleteCookies();
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddHours(2)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private void DeleteCookies()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }
        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"]!;
            else
            {
                System.Net.IPAddress ipaddres = System.Net.Dns.GetHostEntry("").AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
                return ipaddres.ToString();
                //return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }


        [HttpPost("ForgetPassword-SendVerificationCode")]
        [DisableRBAC]
        public async Task<JsonResult> SendVerificationCode(SendVerificationCodeCommand data)
        {
            return await Mediator.Send(data);

        }
        [HttpPost("ForgetPassword-RecieveVerificationCode")]
        [DisableRBAC]
        public async Task<JsonResult> RecieveVerificationCode(RecieveVerificationCodeCommand data)
        {
            return await Mediator.Send(data);

        }
        [HttpPost("ForgetPassword-PasswordReset")]
        [DisableRBAC]
        public async Task<JsonResult> PasswordReset(PasswordResetCommand data)
        {
            return await Mediator.Send(data);

        }
    }
}
