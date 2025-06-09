using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.Update
{
    public class UpdateEmployeeProfileValidator : AbstractValidator<UpdateEmployeeProfileCommand>
    {
        public UpdateEmployeeProfileValidator(IStringLocalizer<CommonValidationResource> localizer, IGeneralHelperRepositoryAsync<ERP_DbContext, EmployeeProfile> helper)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(c => c.EnglishFirstName)
                .NotEmpty()
                .WithMessage(localizeMessage.EnglishNameRequiredField);
            RuleFor(c => c.PashtoFirstName)
                .NotEmpty()
                .WithMessage(localizeMessage.DariNameRequiredField);
            RuleFor(c => c.EnglishSurName)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.EnglishSurName)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PashtoSurName)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.PashtoSurName)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.EnglishFatherName)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.EnglishFatherName)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PashtoFatherName)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.PashtoFatherName)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.EnglishGrandFatherName)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.EnglishGrandFatherName)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PashtoGrandFatherName)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.PashtoGrandFatherName)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.TazkiraTypeId)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.TazkiraTypeId)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.TazkiraNo)
              .NotEmpty()
              .WithMessage(x => $"{localizer[nameof(x.TazkiraNo)]} {localizeMessage.RequiredField}");

            RuleFor(c => c.DateOfBirth)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.DateOfBirth)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.TemporaryAddress)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.TemporaryAddress)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PermenantAddress)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.PermenantAddress)]} {localizeMessage.RequiredField}");

            RuleFor(c => c.JoinDate)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.JoinDate)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PersonalEmail)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.PersonalEmail)]} {localizeMessage.RequiredField}")
                .Must((x, y) => helper.IsUnique(y!, nameof(EmployeeProfile.PersonalEmail), x.ID))
                .WithMessage(x => $"{localizer[nameof(x.PersonalEmail)]} {localizeMessage.UniqueField}");
            RuleFor(c => c.PhoneNumber)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.PhoneNumber)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.PhoneNumber)
                .Must((x, y) => helper.IsUnique(y, nameof(EmployeeProfile.PhoneNumber), x.ID))
                .WithMessage(x => $"{localizer[nameof(x.PhoneNumber)]} {localizeMessage.UniqueField}");
            RuleFor(c => c.PhoneNumber)
                .Must(x => GeneralHelper.IsPhoneNbr(x))
                .WithMessage(x => $"{localizer[nameof(x.PhoneNumber)]} {localizeMessage.NotValidPhoneNumber}");
            RuleFor(c => c.EmergencyPhoneNumber)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.EmergencyPhoneNumber)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.EmergencyPhoneNumber)
                .Must(x => GeneralHelper.IsPhoneNbr(x))
                .WithMessage(x => $"{localizer[nameof(x.EmergencyPhoneNumber)]} {localizeMessage.NotValidPhoneNumber}");
            RuleFor(c => c.IsActive)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.IsActive)]} {localizeMessage.RequiredField}");
            RuleFor(c => c.ProfilePhoto)
                .Must(CheckFile)
                .WithMessage(x => $"{localizer[nameof(x.ProfilePhoto)]} {localizeMessage.BigFileOrFileName}")
                .When(x => x.ProfilePhoto != null);
        }

        private bool CheckFile(IFormFile? file)
        {
            //---Equal to 3MB or 3145728 is Byte,(3145728/1024)/3145728
            return file?.Length <= 3145728;
        }
    }
}

