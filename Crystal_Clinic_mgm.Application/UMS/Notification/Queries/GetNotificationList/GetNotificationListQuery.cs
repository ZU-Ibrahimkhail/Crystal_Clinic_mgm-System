using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.Notification.Queries.GetNotificationList
{
    public class GetNotificationListQuery : DataTableOption, IRequest<ResponseDataTable<GetNotificationViewModel>>
    {
        [JsonIgnore]
        public string Language { get; set; } = string.Empty;
    }
}