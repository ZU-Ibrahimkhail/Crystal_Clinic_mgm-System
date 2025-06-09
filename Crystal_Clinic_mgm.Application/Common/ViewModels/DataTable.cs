using Microsoft.EntityFrameworkCore;
namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    public class DataTable<T> : DataTableResponse
    {
        public static async Task<DataTableResponse> Generate(IQueryable<T> source, int Limit, int Page)
        {
            int Offset = Limit * Page;
            DataTableResponse result = new();
            var data = await source.Skip(Offset).Take(Limit).ToListAsync();
            result.Data = data.Cast<object>().ToArray();
            result.Total = source.Count();
            result.Current_page = Page;
            return result;
        }
        public static async Task<DataTableResponse> Generate(List<T> source, int Limit, int Page)
        {
            return await Task.Run(() =>
            {
                int Offset = Limit * (Page - 1);
                DataTableResponse result = new();
                var data = source.Skip(Offset).Take(Limit);
                result.Data = data.Cast<object>().ToArray();
                result.Total = source.Count;
                result.Current_page = Page;
                return result;
            });
        }
    }
}
