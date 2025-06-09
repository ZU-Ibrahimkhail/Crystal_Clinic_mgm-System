using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.PasswordReset
{
    public class PasswordResetValidator : AbstractValidator<PasswordResetCommand>
    {
        public PasswordResetValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(x => x.ID).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.ID)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.Email).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Email)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.VerificationCode).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.VerificationCode)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.NewPassword)]} {localizeMessage.RequiredField}");
        }
    }
}
