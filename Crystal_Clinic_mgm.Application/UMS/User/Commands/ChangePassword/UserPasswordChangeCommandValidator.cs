using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ChangePassword
{
    public class UserPasswordChangeCommandValidator : AbstractValidator<UserChangePasswordCommand>
    {
        public UserPasswordChangeCommandValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.NewPassword)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.CurrentPassword)]} {localizeMessage.RequiredField}");

        }
    }
}
