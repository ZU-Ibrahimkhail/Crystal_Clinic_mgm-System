using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.Notification.Commands.ShortUpdate
{
    public class ShortUpdateNotificationCommnad : IRequest<JsonResult>
    {
        public int NotificationId { get; set; }
    }
}