using System.Globalization;

namespace Crystal_Clinic_Mgm.Common.Helper
{
    public static class DataConversion
    {
        public static string dateToShami(DateTime dtime)
        {
            PersianCalendar prsDate = new PersianCalendar();
            string shamshiDate =
                            prsDate.GetDayOfMonth(dtime).ToString() + "/" +
                            prsDate.GetMonth(dtime).ToString() + "/" +
                            prsDate.GetYear(dtime).ToString();
            return shamshiDate;
        }
    }
}
