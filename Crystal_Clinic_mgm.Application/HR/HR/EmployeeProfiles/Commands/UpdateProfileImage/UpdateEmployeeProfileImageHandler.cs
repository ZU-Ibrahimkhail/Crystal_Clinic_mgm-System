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

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.UpdateProfileImage
{

    public class UpdateEmployeeProfileImageHandler : IRequestHandler<UpdateEmployeeProfileImageCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IMessage _message;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, EmployeeProfile> _helper;

        public UpdateEmployeeProfileImageHandler(
              IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee,
            IStringLocalizer<CommonValidationResource> localizer,
            IMessage message,
            ILoggedInUser loggedInUser,
            IGeneralHelperRepositoryAsync<ERP_DbContext, EmployeeProfile> helper)
        {
            _GRepoEmployee = gRepoEmployee;
            _loggedInUser = loggedInUser;
            _localizer = localizer;
            _message = message;
            _helper = helper;
        }

        public async Task<JsonResult> Handle(UpdateEmployeeProfileImageCommand request, CancellationToken cancellationToken)
        {// Check for validation errors
            var validator = new UpdateEmployeeProfileImageValidator(_localizer, _helper).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            // Retrieve employee details
            var entity = await _GRepoEmployee.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound();
            }
            else
            {    // Update modification details
                entity.ModifiedBy = _loggedInUser.Id;
                entity.ModifiedOn = DateTime.Now;
                string FilePath = "";
                // Check if profile photo is provided in the request
                if (request.ProfilePhoto != null)
                {
                    var final = request.ProfilePhoto;
                    FileHandler _sotrage = new();
                    if (final.FileName.Length > 0)
                    {
                        string ext = Path.GetExtension(final.FileName);
                        string BasePath = AppConfig.UMS_UserProfilePhoto;
                        // Save the file and get the file path
                        FilePath = await _sotrage.CreateAsync(final.OpenReadStream(), ext, "wwwroot", BasePath);
                    }
                    entity.PhotoPath = FilePath;
                }
                // Update employee record
                _GRepoEmployee.EditeAsync(entity, cancellationToken);
                return _message.Update();
            }
        }
    }
}
