using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Microsoft.AspNetCore.Http;

namespace Crystal_Clinic_Mgm.Common.Localizations
{
    public class Localization
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string language { get; set; }
        public Localization(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            string language = GetMyCookieValue(_httpContextAccessor);

        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Localization()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }


        public string GetName(string language, LookAndAuditableEntity? entity)
        {
            return entity != null ? language switch
            {
                Constants.Constants.Language.English => entity.EnglishName,
                Constants.Constants.Language.Dari => entity.DariName,
                _ => entity.PashtoName,
            } : string.Empty;
        }

        public string GetName(LookAndAuditableEntity? entity)
        {
            return entity != null ? language switch
            {
                Constants.Constants.Language.English => entity.EnglishName,
                Constants.Constants.Language.Dari => entity.DariName,
                _ => entity.PashtoName,
            } : string.Empty;
        }

        public string GetEmployeeLocalizedName(EmployeeProfile? entity, bool IsFirstName)
        {
            if (IsFirstName)
            {

                return entity != null ? language switch
                {
                    Constants.Constants.Language.English => entity.EnglishFirstName,
                    _ => entity.PashtoFirstName,
                } : string.Empty;
            }

            return entity != null ? language switch
            {
                Constants.Constants.Language.English => entity.EnglishSurName,
                _ => entity.PashtoSurName,
            } : string.Empty;

        }
        public string GetEmployeeLocalizeFatherName(EmployeeProfile? entity)
        {
            return entity != null ? language switch
            {
                Constants.Constants.Language.English => entity.EnglishFatherName,
                _ => entity.PashtoFatherName,

            } : string.Empty;
        }
        public string GetEmployeeLocalizeGrandFatherName(EmployeeProfile? entity)
        {
            return entity != null ? language switch
            {
                Constants.Constants.Language.English => entity.EnglishGrandFatherName,
                _ => entity.PashtoGrandFatherName,

            } : string.Empty;
        }

        public string GetMyCookieValue(IHttpContextAccessor httpContextAccessor)
        {
            // Retrieve the cookie value from the Cookies collection
            string cookieValue = httpContextAccessor!.HttpContext!.Request!.Cookies[Constants.Constants.CultureCookies.CookiesName] ?? string.Empty;
            string SelectedLanguage = GeneralHelper.SelectedLanauge(cookieValue ?? string.Empty);

            return SelectedLanguage ?? string.Empty;
        }
    }
}
