using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.UMS.Notification.Queries.GetNotificationList
{
    public class GetNotificationViewModel
    {
        public int ID { get; set; }
        public int TrackingId { get; set; }
        public int NotificationMessageId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public Guid FromUserId { get; set; }
        public string FromUserName { get; set; } = string.Empty;
        public string? FromUserPhotoPath { get; set; } = string.Empty;
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
        public string? ControllerLink { get; set; } = string.Empty;
        public string? ActionLink { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string? Remarks { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;

        public static Expression<Func<Domain.Entities.UMS.Notification, IGeneralHelperRepositoryAsync, string, GetNotificationViewModel>> Projection
        {
            get
            {
                GetNotificationViewModel This = new();
                return (Notifications, _helper, language) => new GetNotificationViewModel
                {
                    ID = Notifications.ID,
                    TrackingId = Notifications.TrackingId ?? 0,
                    IsRead = Notifications.IsRead,
                    NotificationMessageId = Notifications.NotificationMessageId,
                    Title = This.GetNotificationTitle(language, Notifications.NotificationMessage),
                    Message = This.GetNotificationMessage(language, Notifications.NotificationMessage),
                    Description = This.GetNotificationDescriptoin(language, Notifications.NotificationMessage),
                    FromUserId = Notifications.FromUserId,
                    FromUserName = _helper.GetUserName(language, Notifications.FromUserId),
                    FromUserPhotoPath = _helper.GetUserPhotoPath(Notifications.FromUserId),
                    ApplicationId = Notifications.ApplicationId,
                    ApplicationName = Notifications.Application != null ? Notifications.Application.Title : string.Empty,
                    CreatedOn = Notifications.CreatedOn,
                    Remarks = Notifications.Remarks,
                    TrackingNumber = Notifications.TrackingNumber,
                    ControllerLink = Notifications.NotificationMessage != null ? Notifications.NotificationMessage.ControllerLink : string.Empty,
                    ActionLink = Notifications.NotificationMessage != null ? Notifications.NotificationMessage.ActionLink : string.Empty
                };
            }
        }
        #region Notification Message Localization
        public string GetNotificationTitle(string language, NotificationMessage? entity) => entity != null ? language switch
        {
            Constants.Language.English => entity.EnglishTitle,
            Constants.Language.Dari => entity.DariTitle,
            _ => entity.PashtoTitle,
        } : string.Empty;
        public string GetNotificationMessage(string language, NotificationMessage? entity) => entity != null ? language switch
        {
            Constants.Language.English => entity.EnglishMessage,
            Constants.Language.Dari => entity.DariMessage,
            _ => entity.PashtoMessage,
        } : string.Empty;
        public string GetNotificationDescriptoin(string language, NotificationMessage? entity) => entity != null ? language switch
        {
            Constants.Language.English => entity.EnglishDescription,
            Constants.Language.Dari => entity.DariDescription,
            _ => entity.PashtoDescription,
        } : string.Empty;

        #endregion
    }
}
