using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Update
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator(IGeneralHelperRepositoryAsync<UMS_DbContext> helper,
                                          IStringLocalizer<CommonValidationResource> localizer,
                                          IStringLocalizer<UMSValidationResource> umslocalizer,
                                          IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            UMSLocalizeMessage umslocalizeMessage = new(umslocalizer);
            RuleFor(c => c.Id).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Id)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.ApplicationId).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.ApplicationId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.Name).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Name)]} {localizeMessage.RequiredField}")
                .Must((Data, Name) => helper.IsUnique<ApplicationRole>(Name, nameof(ApplicationRole.Name), Data.Id))
                .WithMessage($"{{PropertyValue}} {umslocalizeMessage.RoleExist}");
            RuleFor(c => c.Description).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Description)]} {localizeMessage.RequiredField}");
        }

        //public UpdateRoleCommandValidator(IGeneralHelperRepositoryAsync<UMS_DbContext> helper, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<UMSValidationResource> umslocalizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        //{
        //    Helper = helper;
        //    Localizer = localizer;
        //    Umslocalizer = umslocalizer;
        //    ColumnLocalizer = columnLocalizer;
        //}
    }
}
