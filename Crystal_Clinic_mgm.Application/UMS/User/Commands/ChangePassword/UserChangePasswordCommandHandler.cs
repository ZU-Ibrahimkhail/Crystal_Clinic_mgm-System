using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Identity;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ChangePassword
{
    public class UserChangePasswordCommandHandler : IRequestHandler<UserChangePasswordCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;


        public UserChangePasswordCommandHandler(IMessage message, UserManager<ApplicationUser> userManager, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            _UserManager = userManager;
            _message = message;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
        }
        public async Task<JsonResult> Handle(UserChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var validator = new UserPasswordChangeCommandValidator(_localizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            try
            {
                ApplicationUser? Users = await _UserManager.FindByIdAsync(request.ID);
                if (Users == null)
                {
                    return _message.RecordNotFound();
                }
                request.NewPassword ??= PasswordGenerator.GenerateRandomPassword();
                var result = await _UserManager.ChangePasswordAsync(Users, request.CurrentPassword, request.NewPassword);
                //  var Isuserexist = await _UserManager.CheckPasswordAsync(Users, request.CurrentPassword); 
                if (!result.Succeeded)
                {
                    return _message.ValidationErrorAuthentication(result);
                }
                return _message.Update();
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.Message.ToString());
            }
        }
    }
}