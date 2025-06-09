namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    #region Bar Chart
    public class BarChartDashboard
    {
        public List<string> StructureName { get; set; } = new();
        public string GroupBy { get; set; } = string.Empty;
        public short Value { get; set; }
        public List<BarChartData?> Data { get; set; } = new();
    }
    public class BarChartData
    {
        public string Name { get; set; } = string.Empty;
        public List<int> Data { get; set; } = new();
    }
    #endregion
    #region Spider Dashboard
    public class SpiderDashboard
    {
        public int TotalCategoryCount { get; set; }
        public int TotalValueSum { get; set; }
        public List<SpiderData> Data { get; set; } = new();
    }
    public class SpiderData
    {
        public string Label { get; set; } = string.Empty;
        public int Value { get; set; } = 0;
    }
    #endregion
}
