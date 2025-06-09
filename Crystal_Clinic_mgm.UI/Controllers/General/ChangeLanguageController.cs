using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    //[Tags("General")]
    public class ChangeLanguageController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangeLanguageController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// fa-IR|en|ps-AF
        /// </summary>
        /// <param name="SetCulture"></param>
        /// <returns></returns>
        [HttpPost("{SetCulture:regex(^(fa-IR|en|ps-AF))}")]
        public async Task<IActionResult> SetCulture(string SetCulture)
        {
            if (SetCulture.Trim() == null)
            {
                return NotFound();
            }
            return await Task.Run(() =>
            {
                Response.Cookies.Append(/*"LanguageCookies",*/
                     CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(SetCulture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) });

                return Ok();

            });

        }

    }
}
