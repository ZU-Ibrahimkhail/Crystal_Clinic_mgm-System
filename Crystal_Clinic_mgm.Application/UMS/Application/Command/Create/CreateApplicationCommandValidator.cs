using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
namespace Crystal_Clinic_Mgm.Application.UMS.Application.Command.Create
{
    public class CreateApplicationCommondValidator : AbstractValidator<CreateApplicationCommand>
    {
        public CreateApplicationCommondValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(x => x.Abbrevation).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Abbrevation)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.Description).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Description)]} {localizeMessage.RequiredField}");
        }
    }
}
