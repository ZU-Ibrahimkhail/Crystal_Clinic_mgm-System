using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Create
{
    public class CreatePositionTitleCommandValidator : AbstractValidator<CreatePositionTitleCommand>
    {
        public CreatePositionTitleCommandValidator(
            IGeneralHelperRepositoryAsync<ERP_DbContext, PositionTitle> _Helper,
            IStringLocalizer<CommonColumnNameResource> ColumnLocalizer,
            IStringLocalizer<CommonValidationResource> localizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            //-----EnglishName
            RuleFor(x => x.EnglishName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.EnglishNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.EnglishName), x.BranchId, nameof(PositionTitle.BranchId)))
                .WithMessage(x => $"{localizeMessage.EnglishNameUniqueField}");
            //-----PashtoName
            RuleFor(x => x.PashtoName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.PashtoNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.PashtoName), x.BranchId, nameof(PositionTitle.BranchId)))
                .WithMessage(x => $"{localizeMessage.PashtoNameUniqueField}");
            //-----DariName
            RuleFor(x => x.DariName)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.DariNameRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.DariName), x.BranchId, nameof(PositionTitle.BranchId)))
                .WithMessage(x => $"{localizeMessage.DariNameUniqueField}");
            //-----Code
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(x => $"{localizeMessage.CodeRequiredField}")
                .Must((x, y) => _Helper.IsUnique(y, nameof(x.DariName), x.BranchId, nameof(PositionTitle.BranchId)))
                .WithMessage(x => $"{localizeMessage.CodeUniqueField}");
            //----Branch Id
            RuleFor(x => x.BranchId)
                .NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.BranchId)]} {localizeMessage.RequiredField}");

        }
    }
}
