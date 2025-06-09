using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Commands.Create
{
    public class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommand>
    {

        public CreateLanguageCommandValidator(
            IGeneralHelperRepositoryAsync<UMS_DbContext, Language> _Helper,
            IStringLocalizer<CommonValidationResource> localizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            //-----EnglishName
            RuleFor(x => x.EnglishName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.EnglishNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.EnglishName)))
                .WithMessage(x => $"{localizeMessage.EnglishNameUniqueField}");
            //-----PashtoName
            RuleFor(x => x.PashtoName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.PashtoNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.PashtoName)))
                .WithMessage(x => $"{localizeMessage.PashtoNameUniqueField}");
            //-----DariName
            RuleFor(x => x.DariName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.DariNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.DariName)))
                .WithMessage(x => $"{localizeMessage.DariNameUniqueField}");
            //-----Code
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.CodeRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.DariName)))
                .WithMessage(x => $"{localizeMessage.CodeUniqueField}");

        }
    }
}
