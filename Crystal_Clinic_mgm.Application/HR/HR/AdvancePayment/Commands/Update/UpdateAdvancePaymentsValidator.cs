using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Update
{
    public class UpdateAdvancePaymentValidator : AbstractValidator<UpdateAdvancePaymentCommand>
    {
        public UpdateAdvancePaymentValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(c => c.EmployeeId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.EmployeeId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PayTypeId).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.PayTypeId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.AdvanceDate).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.AdvanceDate)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.AdvanceAmount).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.AdvanceAmount)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.EachInstallmentAmount).NotEmpty().WithMessage(x => $"{columnLocalizer[nameof(x.EachInstallmentAmount)]} {localizeMessage.RequiredField}");
        }
    }
}
