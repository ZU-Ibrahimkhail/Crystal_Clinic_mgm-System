namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    public class ResponseDataTable<T>
    {
        public long TotalRecord { get; set; }
        public int CurrantPage { get; set; }
        public List<T> Data { get; set; } = new();
        public string Error { get; set; } = string.Empty;

    }
}
