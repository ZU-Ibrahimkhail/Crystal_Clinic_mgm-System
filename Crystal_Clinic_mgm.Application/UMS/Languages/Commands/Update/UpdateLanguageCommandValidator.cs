using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Commands.Update
{
    public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
    {
        public UpdateLanguageCommandValidator(
                 IGeneralHelperRepositoryAsync<UMS_DbContext, Crystal_Clinic_Mgm.Domain.Entities.UMS.Language> _Helper,
                 IStringLocalizer<CommonValidationResource> localizer,
                 IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            //---ID
            RuleFor(x => x.ID).Must(CheckId)
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.ID)]}{localizeMessage.NotValidId}");
            //-----EnglishName
            RuleFor(x => x.EnglishName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.EnglishNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.EnglishName), x.ID))
                .WithMessage(x => $"{localizeMessage.EnglishNameUniqueField}");
            //-----PashtoName
            RuleFor(x => x.PashtoName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.PashtoNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.PashtoName), x.ID))
                .WithMessage(x => $"{localizeMessage.PashtoNameUniqueField}");
            //-----DariName
            RuleFor(x => x.DariName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.DariNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.DariName), x.ID))
                .WithMessage(x => $"{localizeMessage.DariNameUniqueField}");
            //-----Code
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.CodeRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.DariName), x.ID))
                .WithMessage(x => $"{localizeMessage.CodeUniqueField}");

        }
        private bool CheckId(int id)
        {
            return !(id < 0);
        }
    }
}