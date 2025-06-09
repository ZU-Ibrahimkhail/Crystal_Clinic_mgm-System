namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    public class DateDifferenceModel
    {
        public int Years { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public static DateDifferenceModel GetDateDifference(DateTime? FirstDate, DateTime SecondDate)
        {


            TimeSpan TS = FirstDate == null ? DateTime.Now.Subtract(SecondDate) : FirstDate.Value.Subtract(SecondDate);
            return new DateDifferenceModel
            {
                Years = TS.Days / 365,
                Months = TS.Days % 365 / 31,
                Days = TS.Days % 365 % 31,
                Hours = TS.Hours,
                Minutes = TS.Minutes,
                Seconds = TS.Seconds
            };
        }
    }

}

