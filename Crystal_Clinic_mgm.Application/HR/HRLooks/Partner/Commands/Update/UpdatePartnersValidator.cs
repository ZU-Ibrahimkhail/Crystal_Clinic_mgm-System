using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.Helper;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Command.Update
{
    public class UpdatePartnersValidator : AbstractValidator<UpdatePartnersCommand>
    {
        public UpdatePartnersValidator(IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer, IGeneralHelperRepositoryAsync<ERP_DbContext, Partners> helper)
        {
            LocalizeMessage localizeMessage = new(localizer);

            RuleFor(x => x.Email)
                .Must((x, y) => helper.IsUnique(y, nameof(x.Email), x.ID))
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Email)]} {localizeMessage.UniqueField}").When(x => x.Email != null);
           
            RuleFor(x => x.Phone).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Phone)]} {localizeMessage.PashtoNameRequiredField}")
                .Must((x, y) => helper.IsUnique(y, nameof(x.Phone), x.ID))
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Phone)]} {localizeMessage.UniqueField}")
                .Must(GeneralHelper.IsPhoneNbr)
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Phone)]} {localizeMessage.NotValidPhoneNumber}");

        }
    }
}
