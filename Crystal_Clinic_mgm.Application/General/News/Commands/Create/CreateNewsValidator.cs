using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.General.News.Commands.Create
{
    public class CreateNewsValidator : AbstractValidator<CreateNewsCommand>
    {
        public CreateNewsValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(c => c.Title).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.Title)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.Description).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.Description)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.EndTime).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.EndTime)]} {localizeMessage.RequiredField}").When(x => x.StartTime != null);
            RuleFor(c => c.EndTime).Must((x, y) => y > x.StartTime).WithMessage(x => $"{columnLocalizer[nameof(x.EndTime)]} {localizeMessage.ReturnDateCondition}").When(x => x.StartTime != null);
            RuleFor(c => c.NewsDate).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.NewsDate)]} {localizeMessage.RequiredField}");

        }
    }
}
