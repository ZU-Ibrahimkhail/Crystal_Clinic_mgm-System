namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    public class DataTableResponse
    {
        public long Total { get; set; }
        public int Current_page { get; set; }
        public object[] Data { get; set; } = Array.Empty<object>();
        public string? Error { get; set; }
    }
}
