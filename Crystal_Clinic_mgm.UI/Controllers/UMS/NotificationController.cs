using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.Notification.Commands.ShortUpdate;
using Crystal_Clinic_Mgm.Application.UMS.Notification.Queries.GetNotificationList;
using Crystal_Clinic_Mgm.Application.UMS.Notification.Queries.GetUnreadNotifactionCount;
using Crystal_Clinic_Mgm.Common.Constants;

namespace Crystal_Clinic_Mgm.UI.Controllers.UMS
{
    [Authorize]
    public class NotificationController : BaseController
    {
        /// <summary>
        /// Short Update
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPut("ShortUpdate/{Id:int}")]
        public async Task<IActionResult> ShortUpdate(ShortUpdateNotificationCommnad command, int Id)
        {
            if (ModelState.IsValid)
            {
                command.NotificationId = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Get List of Notifications
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetNotificationViewModel>> GetList(GetNotificationListQuery query)
        {
            query.Language = Request.Cookies[Constants.CultureCookies.CookiesName] ?? Constants.Language.English;
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Get Unread Notofications
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetUnreadCount")]
        public async Task<IActionResult> GetUnreadNotificationCount(GetUnreadNotifactionCountQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}