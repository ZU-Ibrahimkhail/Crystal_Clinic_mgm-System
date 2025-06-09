using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.UpdateProfileImage
{
    public class UpdateEmployeeProfileImageValidator : AbstractValidator<UpdateEmployeeProfileImageCommand>
    {
        public UpdateEmployeeProfileImageValidator(
            IStringLocalizer<CommonValidationResource> localizer, IGeneralHelperRepositoryAsync<ERP_DbContext, EmployeeProfile> helper)
        {
            LocalizeMessage localizeMessage = new(localizer);
            RuleFor(c => c.ProfilePhoto)
                .NotEmpty()
                .WithMessage(x => $"{localizer[nameof(x.ProfilePhoto)]} {localizeMessage.RequiredField}")
            .Must(CheckFile)
               .WithMessage(x => $"{localizer[nameof(x.ProfilePhoto)]} {localizeMessage.BigFileOrFileName}");
            //.When(x => x.ProfilePhoto != null);
        }
        private bool CheckFile(IFormFile? file)
        {
            //---Equal to 3MB or 3145728 is Byte,(3145728/1024)/3145728
            return file?.Length <= 3145728;
        }
    }
}
