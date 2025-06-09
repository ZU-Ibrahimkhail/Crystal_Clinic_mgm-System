using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Notification.Commands.ShortUpdate
{
    public class ShortUpdateNotificationCommnadHandler : IRequestHandler<ShortUpdateNotificationCommnad, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Domain.Entities.UMS.Notification> _repositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;

        public ShortUpdateNotificationCommnadHandler(IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, Domain.Entities.UMS.Notification> repositoryAsync, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _repositoryAsync = repositoryAsync;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
        }

        public async Task<JsonResult> Handle(ShortUpdateNotificationCommnad request, CancellationToken cancellationToken)
        {
            var validator = new ShortUpdateNotificationCommnadValidator(_localizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            try
            {
                var NotificationRecord = await _repositoryAsync.GetDetailAsync(request.NotificationId);
                if (NotificationRecord == null || NotificationRecord.IsDeleted == true || NotificationRecord.IsRead == true)
                {
                    return _message.RecordNotFound();
                }
                else
                {
                    NotificationRecord.IsRead = true;
                    NotificationRecord.ModifiedOn = DateTime.Now;
                    NotificationRecord.ModifiedBy = _loggedInUser.Id;
                }


                return await _repositoryAsync.UpdateAsync(NotificationRecord, cancellationToken);
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
    }
}