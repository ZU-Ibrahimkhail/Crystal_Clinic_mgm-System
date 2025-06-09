using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Update
{
    public class UpdateExpenseTrackingValidator : AbstractValidator<UpdateExpenseTrackingCommand>
    {
        public UpdateExpenseTrackingValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);


            //RuleFor(x => x.CurrencyTypeId).NotEmpty()
            //    .WithMessage(x => $"{ColumnLocalizer[nameof(x.CurrencyTypeId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.Date).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Date)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.Amount).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Amount)]} {localizeMessage.RequiredField}");

        }
    }
}
