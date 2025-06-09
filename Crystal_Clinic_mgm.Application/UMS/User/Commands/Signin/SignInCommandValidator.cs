using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.Signin
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator(IStringLocalizer<CommonValidationResource> localizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} " + localizeMessage.RequiredField);
            RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} " + localizeMessage.RequiredField);
        }
    }
}
