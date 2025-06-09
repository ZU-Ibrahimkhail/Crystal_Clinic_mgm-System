using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Create
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator(IGeneralHelperRepositoryAsync<UMS_DbContext> helper, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<UMSValidationResource> umslocalizer, IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);
            UMSLocalizeMessage umslocalizeMessage = new(umslocalizer);
            RuleFor(x => x.ApplicationId).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.ApplicationId)]} {localizeMessage.RequiredField}");
            RuleFor(x => x.Name).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Name)]} {localizeMessage.RequiredField}")
                                .Must((x, y) => helper.IsUnique<ApplicationRole>(y, nameof(ApplicationRole.Name))).WithMessage($"{{PropertyValue}} {umslocalizeMessage.RoleExist}");
            RuleFor(x => x.Description).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Description)]} {localizeMessage.RequiredField}");
        }
    }
}
