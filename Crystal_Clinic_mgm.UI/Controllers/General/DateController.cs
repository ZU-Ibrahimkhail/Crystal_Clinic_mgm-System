using Microsoft.AspNetCore.Mvc;
using System.Globalization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Crystal_Clinic_Mgm.UI.Controllers.General
{
    //[Authorize]
    [Route("api/[controller]")]
    //[Tags("General")]
    public class DateController : ControllerBase
    {
        public DateController()
        {
        }
        [HttpGet("GetCurrentDate")]
        public IActionResult Get()
        {
            try
            {
                #region Today Date       
                // Creates and initializes a HijriCalendar.
                HijriCalendar hijDate = new HijriCalendar();
                PersianCalendar prsDate = new PersianCalendar();
                string dtformat = "dd/MM/yyyy";
                // int year = 0, month = 0, day = 0;
                string hijriDate = "", gregorianDate = "", shamshiDate = "";
                DateTime gdate = DateTime.Now;
                hijriDate =
                     hijDate.GetDayOfMonth(gdate).ToString() + "/" +
                     hijDate.GetMonth(gdate).ToString() + "/" +
                     hijDate.GetYear(gdate).ToString();
                //----------
                shamshiDate =
                        prsDate.GetDayOfMonth(gdate).ToString() + "/" +
                        prsDate.GetMonth(gdate).ToString() + "/" +
                       prsDate.GetYear(gdate).ToString();
                //------
                gregorianDate = gdate.ToString(dtformat);

                return Ok(new
                {
                    date = new[] { new { gregorianDate = gregorianDate, shamshiDate = shamshiDate, hijriDate = hijriDate, dtformat = dtformat } }
                }
                     );
                #endregion
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("GetDateConverter/{day:int}/{month:int}/{year:int}/type:string")]
        public IActionResult Get(int day, int month, int year, string type)
        {
            try
            {
                #region DateConvertion
                string datetype = type.ToLower();
                // Creates and initializes a HijriCalendar.
                HijriCalendar hijDate = new HijriCalendar();
                PersianCalendar prsDate = new PersianCalendar();
                string dtformat = "dd/MM/yyyy";
                // int year = 0, month = 0, day = 0;
                string hijriDate = "", gregorianDate = "", shamshiDate = "";
                if (datetype == "gregorian")
                {
                    DateTime gdate = new DateTime(year, month, day);
                    hijriDate =
                         hijDate.GetDayOfMonth(gdate).ToString() + "/" +
                         hijDate.GetMonth(gdate).ToString() + "/" +
                         hijDate.GetYear(gdate).ToString();
                    //----------
                    shamshiDate =
                            prsDate.GetDayOfMonth(gdate).ToString() + "/" +
                            prsDate.GetMonth(gdate).ToString() + "/" +
                           prsDate.GetYear(gdate).ToString();
                    //------
                    gregorianDate = gdate.ToString(dtformat);
                }
                else if (datetype == "hijri")
                {
                    // year = 1444;month = 3;day = 15;
                    //--1:First Convert to Gregorian
                    DateTime gdate = new DateTime(year, month, day, hijDate);
                    hijriDate =
                         hijDate.GetDayOfMonth(gdate).ToString() + "/" +
                         hijDate.GetMonth(gdate).ToString() + "/" +
                        hijDate.GetYear(gdate).ToString();
                    //----------
                    shamshiDate =
                            prsDate.GetDayOfMonth(gdate).ToString() + "/" +
                            prsDate.GetMonth(gdate).ToString() + "/" +
                           prsDate.GetYear(gdate).ToString();
                    //------
                    gregorianDate = gdate.ToString(dtformat);
                }
                else if (datetype == "shamshi")
                {
                    //year = 1401; month = 03; day = 18;
                    //--1:First Convert to Gregorian
                    DateTime gdate = new DateTime(year, month, day, prsDate);
                    hijriDate =
                         hijDate.GetDayOfMonth(gdate).ToString() + "/" +
                         hijDate.GetMonth(gdate).ToString() + "/" +
                        hijDate.GetYear(gdate).ToString();
                    //-------------------------
                    shamshiDate =
                            prsDate.GetDayOfMonth(gdate).ToString() + "/" +
                            prsDate.GetMonth(gdate).ToString() + "/" +
                           prsDate.GetYear(gdate).ToString();
                    //--------------------------
                    gregorianDate = gdate.ToString(dtformat);
                }
                else
                {
                    return BadRequest(ModelState);
                    // gregorianDate = ""; shamshiDate=""; hijriDate=""; dtformat= "";
                }
                return Ok(new
                {
                    date = new[] { new { gregorianDate = gregorianDate, shamshiDate = shamshiDate, hijriDate = hijriDate, dtformat = dtformat } }
                });
                #endregion
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }
        }
        public static string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            // We can't have the hijri date writen in English. We will get a runtime error
            //if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            //{
            //    DateLangCulture = "ar-sa";
            //}
            // Set the date time format to the given culture
            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;
            // Set the calendar property of the date time format to the given calendar
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;
                case "Gregorian":
                    DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;
                case "Persian":
                    DTFormat.Calendar = new System.Globalization.PersianCalendar();
                    break;
                default:
                    return "";
            }
            // We format the date structure to whatever we want
            var dtf = DTFormat.ShortDatePattern = "dd/MM/yyyy";
            var df = DateConv.Date.ToString("f", DTFormat);
            return df.ToString();
        }
    }
}
