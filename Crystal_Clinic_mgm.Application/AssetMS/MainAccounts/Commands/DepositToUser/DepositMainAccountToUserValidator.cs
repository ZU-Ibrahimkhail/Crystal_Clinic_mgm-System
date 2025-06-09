using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.DepositToUser;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.DepositMainAccountToUser
{
    public class DepositMainAccountToUserValidator : AbstractValidator<DepositMainAccountToUserCommand>
    {
        public DepositMainAccountToUserValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);

            RuleFor(x => x.DepositDate).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.DepositDate)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.ToUserId).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.ToUserId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.DepositAmmount).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.DepositAmmount)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.BranchId).NotEmpty()
              .WithMessage(x => $"{ColumnLocalizer[nameof(x.BranchId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.AssetTypeId).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.AssetTypeId)]} {localizeMessage.RequiredField}");
        }
    }
}
