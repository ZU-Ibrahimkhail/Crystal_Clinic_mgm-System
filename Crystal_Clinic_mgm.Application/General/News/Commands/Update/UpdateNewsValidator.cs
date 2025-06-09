using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.General.News.Commands.Update
{
    public class UpdateNewsValidator : AbstractValidator<UpdateNewsCommand>
    {
        public UpdateNewsValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(c => c.Title).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.Title)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.Description).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.Description)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.StartTime).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.StartTime)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.EndTime).Must((x, y) => y > x.StartTime).WithMessage(x => $"{columnLocalizer[nameof(x.EndTime)]} {localizer["BiggerEndDate"]}");
            RuleFor(c => c.NewsDate).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.NewsDate)]} {localizeMessage.RequiredField}");
        }

        //public UpdateNewsValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        //{
        //    Localizer = localizer;
        //    ColumnLocalizer = columnLocalizer;
        //}
    }
}
