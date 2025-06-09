using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Create
{
    public class CreateContractDetailsValidator : AbstractValidator<CreateContractDetailsCommand>
    {
        public CreateContractDetailsValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(c => c.EmployeeProfileId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.EmployeeProfileId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.ContractTypeId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.ContractTypeId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PositionTitleId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.PositionTitleId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.BranchId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.BranchId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.StartDate).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.StartDate)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.EndDate)
                .Must((x, y) => x.StartDate < y)
                .WithMessage(x => $"{columnLocalizer[nameof(x.EndDate)]} {localizeMessage.ReturnDateCondition}")
                .When(x => x.EndDate != null);
        }
    }
}
