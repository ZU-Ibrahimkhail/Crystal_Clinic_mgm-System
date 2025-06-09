using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.Update
{
    public class UpdateEmployeeProfileHandler : IRequestHandler<UpdateEmployeeProfileCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoUser;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, EmployeeProfile> _helper;

        public UpdateEmployeeProfileHandler(
            IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee,
            IStringLocalizer<CommonValidationResource> localizer,
            IMessage message,
            ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> gRepoUser,
            IGeneralHelperRepositoryAsync<ERP_DbContext, EmployeeProfile> helper)
        {
            _GRepoEmployee = gRepoEmployee;
            _localizer = localizer;
            _message = message;
            _loggedInUser = loggedInUser;
            _GRepoUser = gRepoUser;
            _helper = helper;
        }

        public async Task<JsonResult> Handle(UpdateEmployeeProfileCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateEmployeeProfileValidator(_localizer, _helper).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            var entity = await _GRepoEmployee.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound();
            }
            else
            {
                entity.EnglishFirstName = request.EnglishFirstName;
                entity.PashtoFirstName = request.PashtoFirstName;
                entity.EnglishSurName = request.EnglishSurName;
                entity.PashtoSurName = request.PashtoSurName;
                entity.EnglishFatherName = request.EnglishFatherName;
                entity.PashtoFatherName = request.PashtoFatherName;
                entity.EnglishGrandFatherName = request.EnglishGrandFatherName;
                entity.PashtoGrandFatherName = request.PashtoGrandFatherName;
                entity.TazkiraTypeId = request.TazkiraTypeId;
                entity.TazkiraNo = request.TazkiraNo;
                entity.JoldNo = request.JoldNo;
                entity.PageNo = request.PageNo;
                entity.RegNo = request.RegNo;
                entity.DateOfBirth = request.DateOfBirth;
                entity.TemporaryAddress = request.TemporaryAddress;
                entity.PermenantAddress = request.PermenantAddress;
                entity.Gender = request.Gender;
                entity.BloodGroup = request.BloodGroup;
                entity.JoinDate = request.JoinDate;
                entity.LeaveDate = request.LeaveDate;
                entity.LeaveRemark = request.LeaveRemark;
                entity.PersonalEmail = request.PersonalEmail ?? string.Empty;
                entity.PhoneNumber = request.PhoneNumber;
                entity.EmergencyPhoneNumber = request.EmergencyPhoneNumber;
                entity.IsActive = request.IsActive;
                entity.ModifiedBy = _loggedInUser.Id;
                entity.ModifiedOn = DateTime.Now;

                string FilePath = "";
                if (request.ProfilePhoto != null)
                {
                    var final = request.ProfilePhoto;
                    FileHandler _sotrage = new();
                    if (final.FileName.Length > 0)
                    {
                        string ext = Path.GetExtension(final.FileName);
                        string BasePath = AppConfig.UMS_UserProfilePhoto;
                        FilePath = await _sotrage.CreateAsync(final.OpenReadStream(), ext, "wwwroot", BasePath);
                    }
                    entity.PhotoPath = FilePath;
                }
                _GRepoEmployee.EditeAsync(entity, cancellationToken);
                var user = await _GRepoUser.FindByCondition(x => !x.IsDeleted && x.EmployeeId == entity.ID).FirstOrDefaultAsync(cancellationToken);
                if (user != null)
                {
                    user.PhoneNumber = entity.PhoneNumber;
                    user.Email = entity.PersonalEmail;
                    user.ModifiedBy = _loggedInUser.Id;
                    user.ModifiedOn = DateTime.Now;
                    _GRepoUser.EditeAsync(user, cancellationToken);
                }

                return _message.Update();
            }



        }
    }
}
