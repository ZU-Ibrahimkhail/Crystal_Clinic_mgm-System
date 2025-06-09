using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;


namespace Crystal_Clinic_Mgm.Application.General.News.Queries.GetList
{
    public class GetNewsListQuery : DataTableOption, IRequest<ResponseDataTable<GetNewsListModel>>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;
        public string? SearchBy { get; set; }

    }
}
