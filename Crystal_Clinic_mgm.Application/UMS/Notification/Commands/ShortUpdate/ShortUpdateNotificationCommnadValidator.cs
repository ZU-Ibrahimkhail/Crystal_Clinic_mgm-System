using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;

namespace Crystal_Clinic_Mgm.Application.UMS.Notification.Commands.ShortUpdate
{
    public class ShortUpdateNotificationCommnadValidator : AbstractValidator<ShortUpdateNotificationCommnad>
    {
        public ShortUpdateNotificationCommnadValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(e => e.NotificationId).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.NotificationId)]} {localizeMessage.RequiredField}");
        }
    }
}
