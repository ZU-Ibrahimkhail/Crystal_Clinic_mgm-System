using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
namespace Crystal_Clinic_Mgm.Common.LocalizeMessage
{
    public class UMSLocalizeMessage
    {
       
        public IStringLocalizer<UMSValidationResource> _localizer;
        public string Superadmin = string.Empty;
        public string NotAuthenticated = string.Empty;
        public string NoPermission = string.Empty;
        public string InvalidPolicy = string.Empty;
        public string AccountLocked = string.Empty;
        public string LoggedIn = string.Empty;
        public string LoginCredentials = string.Empty;
        public string UserNotFound = string.Empty;
        public string PasswordIncorrect = string.Empty;
        public string SignOut = string.Empty;
        public string UserExist = string.Empty;
        public string RoleExist = string.Empty;
        public string PermissionExist = string.Empty;
        public string InvalidRequest = string.Empty;
        public string ParentChildRecord = string.Empty;
        public string UserNotCreated = string.Empty;
        public string EmailExist = string.Empty;
        public string ChangePasswordCredentials = string.Empty;
        public string UserAndEmailExist = string.Empty;
        public string ValidPhoneNO = string.Empty;
        public string UserNotUpdated = string.Empty;
        public string InvalideRequestId = string.Empty;
        public string IsActiveAndIsDelete = string.Empty;
        public string SuperadminNotDelete = string.Empty;
        public string CCRoleMessage = string.Empty;
        public string EmpOrRepresentatorNotFond = string.Empty;
        public UMSLocalizeMessage(IStringLocalizer<UMSValidationResource> localizer)
        {
            _localizer = localizer;
            Superadmin = _localizer[nameof(Superadmin)];
            NotAuthenticated = _localizer[nameof(NotAuthenticated)];
            NoPermission = _localizer[nameof(NoPermission)];
            InvalidPolicy = _localizer[nameof(InvalidPolicy)];
            AccountLocked = _localizer[nameof(AccountLocked)];
            LoggedIn = _localizer[nameof(LoggedIn)];
            LoginCredentials = _localizer[nameof(LoginCredentials)];
            UserNotFound = _localizer[nameof(UserNotFound)];
            PasswordIncorrect = _localizer[nameof(PasswordIncorrect)];
            SignOut = _localizer[nameof(SignOut)];
            UserExist = _localizer[nameof(UserExist)];
            RoleExist = _localizer[nameof(RoleExist)];
            PermissionExist = _localizer[nameof(PermissionExist)];
            InvalidRequest = _localizer[nameof(InvalidRequest)];
            ParentChildRecord = _localizer[nameof(ParentChildRecord)];
            UserNotCreated = _localizer[nameof(UserNotCreated)];
            EmailExist = _localizer[nameof(EmailExist)];
            ChangePasswordCredentials = _localizer[nameof(ChangePasswordCredentials)];
            UserAndEmailExist = _localizer[nameof(UserAndEmailExist)];
            ValidPhoneNO = _localizer[nameof(ValidPhoneNO)];
            UserNotUpdated = _localizer[nameof(UserNotUpdated)];
            InvalideRequestId = _localizer[nameof(InvalideRequestId)];
            IsActiveAndIsDelete = _localizer[nameof(IsActiveAndIsDelete)];
            SuperadminNotDelete = _localizer[nameof(SuperadminNotDelete)];
            CCRoleMessage = _localizer[nameof(CCRoleMessage)];
            EmpOrRepresentatorNotFond = _localizer[nameof(EmpOrRepresentatorNotFond)];
        }
    }
}
