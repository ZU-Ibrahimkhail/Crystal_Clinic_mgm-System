using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserList
{
    public class GetUserListQuery : DataTableOption, IRequest<DataTableResponse>
    {
        [JsonInclude]
        public string Language { get; set; } = string.Empty;
        public string? SearchBy { get; set; }
    }
}
