using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Commands.PaySalary
{
    public class PaySalaryValidator : AbstractValidator<PaySalaryCommand>
    {
        public PaySalaryValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(c => c.EmployeeId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.EmployeeId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.ContractDetailsId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.ContractDetailsId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PayTypeId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.PayTypeId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.BranchId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.BranchId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.BaseSalary).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.BaseSalary)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.NetSalary).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.NetSalary)]} {localizeMessage.RequiredField}");
        }
    }
}
