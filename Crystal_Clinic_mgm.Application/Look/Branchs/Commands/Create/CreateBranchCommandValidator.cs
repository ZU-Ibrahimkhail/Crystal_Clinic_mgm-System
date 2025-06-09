using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Create
{
    public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
    {
        public CreateBranchCommandValidator(IStringLocalizer<CommonValidationResource> localizer,
                                                IStringLocalizer<CommonColumnNameResource> ColumnLocalizer,
                                                IGeneralHelperRepositoryAsync<ERP_DbContext, Branch> helper)
        {

            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(x => x.EnglishName).NotEmpty()
                .WithMessage(x => $"{localizeMessage.EnglishNameRequiredField}")
                .Must((x, y) => helper.IsUnique(y, nameof(x.EnglishName)))
                .WithMessage(x => $"{localizeMessage.EnglishNameUniqueField}");
            RuleFor(x => x.PashtoName).NotEmpty()
                .WithMessage(x => $"{localizeMessage.PashtoNameRequiredField}")
                .Must((x, y) => helper.IsUnique(y, nameof(x.PashtoName)))
                .WithMessage(x => $"{localizeMessage.PashtoNameUniqueField}");
            RuleFor(x => x.DariName).NotEmpty()
                .WithMessage(x => $"{localizeMessage.DariNameRequiredField}")
                .Must((x, y) => helper.IsUnique(y, nameof(x.DariName)))
                .WithMessage(x => $"{localizeMessage.DariNameUniqueField}");
            RuleFor(x => x.Code).NotEmpty()
                .WithMessage(x => $"{localizeMessage.CodeRequiredField}")
                .Must((x, y) => helper.IsUnique(y, nameof(x.Code)))
                .WithMessage(x => $"{localizeMessage.CodeUniqueField}");
        }

    }
}
