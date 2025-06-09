using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Common.Services.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly UMS_DbContext _UmsDbContext;
        private readonly ILoggedInUser _loggedInUser;

        public NotificationRepository(UMS_DbContext umsDbContext, ILoggedInUser loggedInUser)
        {
            _UmsDbContext = umsDbContext;
            _loggedInUser = loggedInUser;
        }



        public void AddNotification(Guid? ToUserId, int NotificationMessageId, int Application, int Branch, int TrackingId, string? Remarks = null, string TrackingNumber = "")
        {
            if (ToUserId != null)
            {
                var notification = new Notification
                {
                    NotificationMessageId = NotificationMessageId,
                    TrackingId = TrackingId,
                    FromUserId = _loggedInUser.Id,
                    ToUserId = ToUserId.Value,
                    IsRead = false,
                    CreatedBy = _loggedInUser.Id, /*(int)_LogedInUser.Id;*/
                    BranchId = Branch, /*(int)_LogedInUser.BranchId*/
                    ApplicationId = Application, /*DMTS, Archive etc it take manually because one user have multiple module*/
                    Remarks = Remarks,
                    TrackingNumber = TrackingNumber,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };
                _UmsDbContext.Add(notification);
                _UmsDbContext.SaveChanges();
            }
        }

        public void RemoveNotification(Guid UserId, int TrackingId, int NotificationMessageId)
        {
            var notifications = _UmsDbContext.Notifications.Where(x => x.ToUserId == UserId && x.TrackingId == TrackingId && x.NotificationMessageId == NotificationMessageId);
            _UmsDbContext.RemoveRange(notifications);
            _UmsDbContext.SaveChanges();

        }
    }

}
