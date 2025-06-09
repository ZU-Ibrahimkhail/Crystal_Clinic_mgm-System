using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Update
{
    public class UpdateTrainingVideoValidator : AbstractValidator<UpdateTrainingVideoCommand>
    {
        public UpdateTrainingVideoValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(c => c.DariTitle).NotEmpty().WithMessage(localizeMessage.RequiredField);
            RuleFor(c => c.PashtoTitle).NotEmpty().WithMessage(localizeMessage.RequiredField);
            RuleFor(c => c.Application).NotEmpty().WithMessage(localizeMessage.RequiredField);
            RuleFor(c => c.Poster).NotEmpty().WithMessage(localizeMessage.RequiredField);

            RuleFor(c => c.DariVideoPath).NotEmpty().WithMessage(localizeMessage.RequiredField).When(x => x.PashtoVideoPath == null);
            RuleFor(c => c.DariVideoPath).Must(HaveCorrectSize)
                .WithMessage(x => $"{columnLocalizer[nameof(x.DariVideoPath)]} {localizer["LargFile"]}");
            RuleFor(c => c.PashtoVideoPath).NotEmpty().WithMessage(localizeMessage.RequiredField).When(x => x.DariVideoPath == null);
            RuleFor(c => c.PashtoVideoPath).Must(HaveCorrectSize)
                .WithMessage(x => $"{columnLocalizer[nameof(x.PashtoVideoPath)]} {localizer["LargFile"]}");
        }
        private bool HaveCorrectSize(IFormFile? file)
        {
            return file == null || file!.Length < 513 * 1024 * 1024;
        }
    }
}
