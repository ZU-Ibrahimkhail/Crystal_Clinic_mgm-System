using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Create
{
    public class CreateWithdrawalTrackingValidator : AbstractValidator<CreateWithdrawalTrackingCommand>
    {
        public CreateWithdrawalTrackingValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);

            //RuleFor(x => x.CurrencyTypeId).NotEmpty()
            //    .WithMessage(x => $"{ColumnLocalizer[nameof(x.CurrencyTypeId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.Date).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Date)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.MainAccountId).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.MainAccountId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.WithdrawalAmount).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.WithdrawalAmount)]} {localizeMessage.RequiredField}");
        }
    }
}
