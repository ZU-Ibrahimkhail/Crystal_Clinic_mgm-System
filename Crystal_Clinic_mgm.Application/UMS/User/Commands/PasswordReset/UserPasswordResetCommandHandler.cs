using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Identity;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.PasswordReset
{
    public class UserPasswordResetCommandHandler : IRequestHandler<UserPasswordResetCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        UMSLocalizeMessage umslocalizeMessage;

        public UserPasswordResetCommandHandler(IMessage message,
                                               UserManager<ApplicationUser> userManager,
                                               IStringLocalizer<CommonValidationResource> localizer,
                                               IStringLocalizer<CommonColumnNameResource> columnLocalizer,
                                               IStringLocalizer<UMSValidationResource> umslocalizer)
        {
            _UserManager = userManager;
            _message = message;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            umslocalizeMessage = new(umslocalizer);
        }
        public async Task<JsonResult> Handle(UserPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var validator = new UserPasswordResetCommandValidator(_localizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }

            var User = await _UserManager.FindByIdAsync(request.ID);
            if (User == null)
            {
                return _message.RecordNotFound();
            }

            request.NewPassword ??= PasswordGenerator.GenerateRandomPassword();
            if (User.IsActive == true && User.IsDeleted == false)
            {
                var token = await _UserManager.GeneratePasswordResetTokenAsync(User);
                var Result = await _UserManager.ResetPasswordAsync(User, token, request.NewPassword);
                if (!Result.Succeeded)
                {
                    //return _message.InternalSystemError(Result.Errors);
                    return _message.ValidationErrorAuthentication(Result);
                }
                return _message.Saved();
            }
            else
            {
                return new JsonResult(new { Message = umslocalizeMessage.IsActiveAndIsDelete });
            }


        }
    }
}
