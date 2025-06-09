namespace Crystal_Clinic_Mgm.Application.Common.Services.IRepositories
{
    public interface INotificationRepository
    {
        void AddNotification(Guid? ToUserId, int NotificationMessageId, int Application, int Branch, int TrackingId, string? Remarks = null, string TrackingNumber = "");
        void RemoveNotification(Guid UserId, int TrackingId, int NotificationMessageId);
    }
}
