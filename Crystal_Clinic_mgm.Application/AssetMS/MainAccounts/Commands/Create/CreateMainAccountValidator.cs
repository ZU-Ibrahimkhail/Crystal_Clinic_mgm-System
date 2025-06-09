using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Create
{
    public class CreateMainAccountValidator : AbstractValidator<CreateMainAccountCommand>
    {
        public CreateMainAccountValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);

            RuleFor(x => x.CurrencyTypeId).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.CurrencyTypeId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.DepositDate).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.DepositDate)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.OwnerUserId).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.OwnerUserId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.BranchId).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.BranchId)]} {localizeMessage.RequiredField}");
        }
    }
}
