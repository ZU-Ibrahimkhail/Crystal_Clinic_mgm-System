using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, JsonResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserAllowedDocTypesSecurityLevels> _GRepoUserAllowedDocTypesSecurity;
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserRole> _GRepoUserRole;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IGeneralHelperRepositoryAsync<UMS_DbContext> _helper;
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        readonly UMSLocalizeMessage _umslocalizeMessage;
        private readonly IGenericRepositoryAsync<UMS_DbContext, RolePermission> _GRepoRolePermission;
        public UpdateUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            IMessage message,
            ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<UMS_DbContext, UserAllowedDocTypesSecurityLevels> gRepoUserAllowedDocTypesSecurity,
            IGenericRepositoryAsync<UMS_DbContext, UserRole> gRepoUserRole,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IStringLocalizer<UMSValidationResource> umslocalizer,
            IGeneralHelperRepositoryAsync<UMS_DbContext> helper,
            IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee,
            IGenericRepositoryAsync<UMS_DbContext, RolePermission> gRepoRolePermission)
        {
            _userManager = userManager;
            _message = message;
            _loggedInUser = loggedInUser;
            _GRepoUserAllowedDocTypesSecurity = gRepoUserAllowedDocTypesSecurity;
            _GRepoUserRole = gRepoUserRole;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _umslocalizeMessage = new(umslocalizer);
            _helper = helper;
            _GRepoEmployee = gRepoEmployee;
            _GRepoRolePermission = gRepoRolePermission;
        }

        public async Task<JsonResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            #region checking the validations 
            var validator = new UpdateUserCommandValidator(_helper, _localizer, _umslocalizeMessage, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            #endregion

            try
            {
                var _Emplpyee = _GRepoEmployee.FindByCondition(x => !x.IsDeleted && x.ID == request.EmployeeId).SingleOrDefault();
                if (_Emplpyee == null)
                {
                    return _message.ValidationError(8, _umslocalizeMessage.EmpOrRepresentatorNotFond);
                }
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user != null && user.IsDeleted == false)
                {
                    #region modifying the records of the user 
                    user.EmployeeId = request.EmployeeId;
                    user.BranchId = _Emplpyee?.BranchId;
                    user.UserName = request.UserName;
                    user.Email = request.Email;
                    user.PhoneNumber = _Emplpyee?.PhoneNumber ?? string.Empty;
                    user.ModifiedBy = _loggedInUser.Id;// _user.Id,
                    user.ModifiedOn = DateTime.Now;
                    user.IsBranchAdmin = request.IsBranchAdmin ?? user.IsBranchAdmin;
                    #endregion

                    #region updating the database and
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        #region Update Employee or Representator Email
                        if (_Emplpyee != null)
                        {
                            _Emplpyee.PersonalEmail = request.Email;
                            _Emplpyee.ModifiedBy = _loggedInUser.Id;
                            _Emplpyee.ModifiedOn = DateTime.Now;
                            _GRepoEmployee.EditeAsync(_Emplpyee, cancellationToken);
                        }
                        #endregion

                        #region removing old roles of the user
                        var OldRoles = _GRepoUserRole.FindByConditionWithTracking(a => a.UserId == user.Id).ToList();
                        if (OldRoles != null && OldRoles.Count > 0)
                        {
                            _GRepoUserRole.MultiRecordRemoveRangeAsync(OldRoles, cancellationToken);
                        }
                        #endregion

                        #region adding user role 
                        var ur = request.UserRoles?.ToString();
                        var userrole = from val in ur?.Split(',')
                                       select int.Parse(val);
                        if (request.UserRoles != null && request.UserRoles.Length > 0)
                        {
                            foreach (var roleid in userrole)
                            {
                                var userRole = new UserRole
                                {
                                    RoleId = roleid,
                                    UserId = user.Id,
                                    CreatedBy = _loggedInUser.Id,
                                    CreatedOn = DateTime.Now
                                };
                                _GRepoUserRole.SaveAsync(userRole, cancellationToken);
                            }
                        }
                        #endregion

                        #region removing old allowed document type security levels
                        var OldUserAllowedDocTypeDepSecurityLevel = _GRepoUserAllowedDocTypesSecurity.FindByConditionWithTracking(x => x.UserId == user.Id).ToList();
                        if (OldUserAllowedDocTypeDepSecurityLevel != null)
                        {
                            _GRepoUserAllowedDocTypesSecurity.MultiRecordRemoveRangeAsync(OldUserAllowedDocTypeDepSecurityLevel, cancellationToken);
                        }
                        #endregion

                        #region adding user allowedDocumentType and  security level
                        var UserAllowedDocTypeDepSecurityLevel = new UserAllowedDocTypesSecurityLevels()
                        {
                            UserId = user.Id,
                            AllowedBranchId = "," + request.AllowedbranchlevelModels + ",",
                            CreatedBy = _loggedInUser.Id,
                            CreatedOn = DateTime.Now,
                        };
                        _GRepoUserAllowedDocTypesSecurity.SaveAsync(UserAllowedDocTypeDepSecurityLevel, cancellationToken);
                        #endregion

                        return _message.Update();
                    }
                    else
                    {
                        return _message.InternalSystemError(_umslocalizeMessage.UserNotUpdated);
                    }
                    #endregion
                }
                else
                {
                    return _message.RecordNotFound();
                }
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
    }
}
