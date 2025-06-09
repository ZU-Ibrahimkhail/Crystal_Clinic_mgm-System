using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.UpdateDeposit
{
    public class UpdateDepositValidator : AbstractValidator<UpdateDepositCommand>
    {
        public UpdateDepositValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);


            //RuleFor(x => x.CurrencyTypeId).NotEmpty()
            //    .WithMessage(x => $"{ColumnLocalizer[nameof(x.CurrencyTypeId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.Date).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Date)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.DepositAmount).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.DepositAmount)]} {localizeMessage.RequiredField}");

        }
    }
}
