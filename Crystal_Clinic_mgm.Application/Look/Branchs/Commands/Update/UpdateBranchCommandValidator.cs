using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Update
{
    public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
    {
        public UpdateBranchCommandValidator(IStringLocalizer<CommonValidationResource> localizer,
                                                IStringLocalizer<CommonColumnNameResource> Columnocalizer,
                                                IGeneralHelperRepositoryAsync<ERP_DbContext, Branch> helper)
        {
            LocalizeMessage localizeMessage = new(localizer);

            RuleFor(c => c.Id).Must(CheckId)
                .WithMessage(x => $"{Columnocalizer[nameof(x.Id)]} {localizer[nameof(localizeMessage.NotValidId)]}");
            RuleFor(x => x.EnglishName).NotEmpty()
                .WithMessage(x => localizer[nameof(localizeMessage.EnglishNameRequiredField)])
                .Must((x, y) => helper.IsUnique(y, nameof(x.EnglishName), x.Id))
                .WithMessage(x => localizeMessage.EnglishNameUniqueField);
            RuleFor(x => x.DariName).NotEmpty()
                .WithMessage(x => localizer[nameof(localizeMessage.DariNameRequiredField)])
                .Must((x, y) => helper.IsUnique(y, nameof(x.DariName), x.Id))
                .WithMessage(x => localizeMessage.DariNameUniqueField);
            RuleFor(x => x.PashtoName).NotEmpty()
                .WithMessage(x => localizer[nameof(localizeMessage.PashtoNameRequiredField)])
                .Must((x, y) => helper.IsUnique(y, nameof(x.PashtoName), x.Id))
                .WithMessage(x => localizeMessage.PashtoNameUniqueField);
            RuleFor(x => x.Code).NotEmpty()
                .WithMessage(x => localizer[nameof(localizeMessage.CodeRequiredField)])
                .Must((x, y) => helper.IsUnique(y, nameof(x.Code), x.Id))
                .WithMessage(x => localizeMessage.CodeUniqueField);


        }


        private bool CheckId(int Id)
        {
            return !(Id < 0);
        }
    }
}
