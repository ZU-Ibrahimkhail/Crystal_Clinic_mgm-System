using FluentValidation;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(
            IGeneralHelperRepositoryAsync<UMS_DbContext> helper,
            UMSLocalizeMessage umslocalizeMessage,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> ColumnLocalizer)
        {
            LocalizeMessage localizeMessage = new(localizer);

            RuleFor(r => r.EmployeeId).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.EmployeeId)]} {localizeMessage.RequiredField}");

            RuleFor(O => O.EmployeeId)
                .Must((Data, value) => helper.IsUnique<ApplicationUser>(value ?? 0, nameof(ApplicationUser.EmployeeId)))
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.EmployeeId)]} {{PropertyValue}} {umslocalizeMessage.UserExist}")
                .When(x => x.EmployeeId != null);
            RuleFor(O => O.UserName).NotEmpty()
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.UserName)]} {localizeMessage.RequiredField}")
                .Must((Data, value) => helper.IsUnique<ApplicationUser>(value, nameof(ApplicationUser.UserName)))
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.UserName)]} {{PropertyValue}} {umslocalizeMessage.UserExist}");
            RuleFor(O => O.Email).NotEmpty().WithMessage(x => $"{ColumnLocalizer[nameof(x.Email)]} {localizeMessage.RequiredField}")
                .Must((Data, value) => helper.IsUnique<ApplicationUser>(value, nameof(ApplicationUser.Email)))
                .WithMessage(x => $"{ColumnLocalizer[nameof(x.Email)]} {{PropertyValue}} {umslocalizeMessage.UserExist}");

        }
    }
}
