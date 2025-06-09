using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    public class MyDataTable<T> : ResponseDataTable<T>
    {
        public static async Task<ResponseDataTable<T>> Generate(IQueryable<T> source, int Limit, int Page)
        {
            int Offset = Limit * Page;
            ResponseDataTable<T> result = new();
            var data = await source.Skip(Offset).Take(Limit).ToListAsync();
            result.Data = data;
            result.TotalRecord = source.Count();
            result.CurrantPage = Page;
            return result;
        }

        public static ResponseDataTable<T> Generate(IEnumerable<T> source, int Limit, int Page)
        {
            int Offset = Limit * Page;
            ResponseDataTable<T> result = new();
            var data = source.Skip(Offset).Take(Limit).ToList();
            result.Data = data;
            result.TotalRecord = source.Count();
            result.CurrantPage = Page;
            return result;
        }
        public static ResponseDataTable<T> Generate(List<T> source, int Limit, int Page)
        {
            int Offset = Limit * Page;
            ResponseDataTable<T> result = new();
            var data = source.Skip(Offset).Take(Limit).ToList();
            result.Data = data;
            result.TotalRecord = source.Count();
            result.CurrantPage = Page;
            return result;
        }

    }
}


