using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Crystal_Clinic_Mgm.Common.Helper
{

    public static class GeneralHelper
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //public GeneralHelper(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}
        // Regular expression used to validate a phone number with length and formate like this +93799388428.
        public const string motif = @"[+][0-9]{7}[0-9]{1}[0-9]*?";
        // Regular expression used to validate a phone number with length and formate like this 799388428
        //public const string motif = @"[6-7]{1}[0-9]{8}$"; 
        public static bool IsPhoneNbr(string number)
        {
            if (number != null)
                return Regex.IsMatch(number, motif);
            else return false;
        }
        public static DateTime WeekStartDate()
        {
            return DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Saturday => DateTime.Now.Date,
                DayOfWeek.Sunday => DateTime.Now.Date.AddDays(-1),
                DayOfWeek.Monday => DateTime.Now.Date.AddDays(-2),
                DayOfWeek.Tuesday => DateTime.Now.Date.AddDays(-3),
                DayOfWeek.Wednesday => DateTime.Now.Date.AddDays(-4),
                DayOfWeek.Thursday => DateTime.Now.Date.AddDays(-5),
                DayOfWeek.Friday => DateTime.Now.Date.AddDays(-6),
                _ => DateTime.Now.Date
            };
        }


        public static string SelectedLanauge(string? language)
        {
            if (language == null)
                return Constants.Constants.Language.English;
            string english = Constants.Constants.Language.English;
            string dari = Constants.Constants.Language.Dari;
            string pashto = Constants.Constants.Language.Pashto;

            return language switch
            {
                var lang when lang.EndsWith(english) => english,
                var lang when lang.EndsWith(dari) => dari,
                _ => pashto,
            };
        }

    }

}
