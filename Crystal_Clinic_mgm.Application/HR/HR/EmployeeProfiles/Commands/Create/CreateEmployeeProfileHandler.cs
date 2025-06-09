using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.Create
{
    public class CreateEmployeeProfileHandler : IRequestHandler<CreateEmployeeProfileCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, EmployeeProfile> _helper;
        private readonly ERP_DbContext _DbContext;

        public CreateEmployeeProfileHandler(
            IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee,
            IStringLocalizer<CommonValidationResource> localizer,
            IMessage message,
            ILoggedInUser loggedInUser,
            IGeneralHelperRepositoryAsync<ERP_DbContext, EmployeeProfile> helper,
            ERP_DbContext DbContext
            )
        {
            _GRepoEmployee = gRepoEmployee;
            _localizer = localizer;
            _message = message;
            _loggedInUser = loggedInUser;
            _helper = helper;
            _DbContext = DbContext;
        }

        public async Task<JsonResult> Handle(CreateEmployeeProfileCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateEmployeeProfileValidator(_localizer, _helper, _DbContext).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }

            var attachment = request.ProfilePhoto;
            string FilePath = "";
            if (attachment != null)
            {
                FileHandler _sotrage = new();
                if (attachment.FileName.Length > 0)
                {
                    string ext = System.IO.Path.GetExtension(attachment.FileName);
                    FilePath = await _sotrage.CreateAsync(attachment.OpenReadStream(), ext, "wwwroot", AppConfig.UMS_UserProfilePhoto);
                }
            }



            var entity = new EmployeeProfile
            {
                EnglishFirstName = request.EnglishFirstName,
                PashtoFirstName = request.PashtoFirstName,
                EnglishSurName = request.EnglishSurName,
                PashtoSurName = request.PashtoSurName,
                EnglishFatherName = request.EnglishFatherName,
                PashtoFatherName = request.PashtoFatherName,
                EnglishGrandFatherName = request.EnglishGrandFatherName,
                PashtoGrandFatherName = request.PashtoGrandFatherName,
                TazkiraTypeId = request.TazkiraTypeId,
                TazkiraNo = request.TazkiraNo,
                JoldNo = request.JoldNo,
                PageNo = request.PageNo,
                RegNo = request.RegNo,
                DateOfBirth = request.DateOfBirth,
                TemporaryAddress = request.TemporaryAddress,
                PermenantAddress = request.PermenantAddress,
                BranchId = request.BranchId,
                BloodGroup = request.BloodGroup,
                Gender = request.Gender,
                JoinDate = request.JoinDate,
                LeaveDate = request.LeaveDate,
                LeaveRemark = request.LeaveRemark,
                PersonalEmail = request.PersonalEmail ?? string.Empty,
                PhoneNumber = request.PhoneNumber,
                EmergencyPhoneNumber = request.EmergencyPhoneNumber,
                IsActive = request.IsActive,
                PhotoPath = FilePath,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedBy = _loggedInUser.Id,
                ModifiedOn = DateTime.Now,
            };

            _GRepoEmployee.SaveAsync(entity, cancellationToken);
            return _message.Saved();
        }
    }
}
