namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Dashboards
{
    public class ChartModel
    {
        public string[] Day { get; set; } = [];
        public string[] Week { get; set; } = [];
        public string[] Month { get; set; } = [];
        public string[] Year { get; set; } = [];
    }

    public class ChartData
    {
        public string GroupName { get; set; } = string.Empty;
        public List<Data> Data { get; set; } = [];
    }

    public class Data
    {
        public string Name { get; set; } = string.Empty;
        public decimal[] data { get; set; } = [];
    }
}