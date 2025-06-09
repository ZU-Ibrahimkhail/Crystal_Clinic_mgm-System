using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Configuration;
using Crystal_Clinic_Mgm.Application.Common.Identity;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Data;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, JsonResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UMS_DbContext _DbContext;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserAllowedDocTypesSecurityLevels> _GRepoUserAllowedDocTypesSecurity;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoApplicationUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserRole> _GRepoUserRole;
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly IMailRepositoy _mailRepositoy;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IGeneralHelperRepositoryAsync<UMS_DbContext> _helper;
        readonly UMSLocalizeMessage _umslocalizeMessage;
        public CreateUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            IGenericRepositoryAsync<UMS_DbContext, UserAllowedDocTypesSecurityLevels> genericRepositoryAsync,
            IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> GRepoApplicationUser,
            IGenericRepositoryAsync<UMS_DbContext, UserRole> GRepoUserRole,
            IMessage message,
            UMS_DbContext context,
            ILoggedInUser loggedInUser,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IStringLocalizer<UMSValidationResource> umslocalizer,
            IGeneralHelperRepositoryAsync<UMS_DbContext> helper,
            IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee,
            IMailRepositoy mailRepositoy)
        {
            _userManager = userManager;
            _GRepoUserAllowedDocTypesSecurity = genericRepositoryAsync;
            _GRepoApplicationUser = GRepoApplicationUser;
            _GRepoUserRole = GRepoUserRole;
            _DbContext = context;
            _message = message;
            _loggedInUser = loggedInUser;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _umslocalizeMessage = new(umslocalizer);
            _helper = helper;
            _GRepoEmployee = gRepoEmployee;
            _mailRepositoy = mailRepositoy;
        }
        public async Task<JsonResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateUserCommandValidator(_helper, _umslocalizeMessage, _localizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }

            try
            {
                var _Emplpyee = _GRepoEmployee.FindByCondition(x => !x.IsDeleted && x.ID == request.EmployeeId).SingleOrDefault();
                if (_Emplpyee == null)
                {
                    return _message.ValidationError(8, _umslocalizeMessage.EmpOrRepresentatorNotFond);
                }
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = request.EmployeeId,
                    UserName = request.UserName,
                    Email = request.Email,
                    EmailConfirmed = true,
                    LastLoginDate = DateTime.Now,
                    IsActive = false,
                    SuccessLoginCount = 0,
                    PhoneNumber = _Emplpyee?.PhoneNumber ?? string.Empty,
                    BranchId = _Emplpyee?.BranchId,
                    PhoneNumberConfirmed = true,
                    IsBranchAdmin = request.IsBranchAdmin ?? false,
                    CreatedBy = _loggedInUser.Id,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                };
                var GeneratedPassword = PasswordGenerator.GenerateRandomPassword();
                var result = await _userManager.CreateAsync(user, GeneratedPassword);
                var allRoles = await _DbContext.ApplicationRoles.Select(r => r.Id).ToListAsync(cancellationToken);

                //---Send Password to Email
                var mailMde = new MailRequest
                {
                    ToEmail = user.Email,
                    Subject = "Create Account in " + Constants.ERPSystemName.Acronym,
                    Body = "Email from  " + Constants.ERPSystemName.FullName
                };
                //------Email Setting 
                //  await _mailRepositoy.SendEmailAsync(mailMde);
                //  var UserRoles = request.UserRoles != null;
                //  var AllowedBranchs = request.AllowedBranchs == null ? false : true;
                //  var AllowedDocumentType = request.AllowedDocumentType == null ? false : true;
                if (result.Succeeded)
                {
                    #region Update Employee or Representator Email
                    if (_Emplpyee != null)
                    {
                        _Emplpyee.PersonalEmail = request.Email;
                        _Emplpyee.HasAccount = true;
                        _Emplpyee.ModifiedBy = _loggedInUser.Id;
                        _Emplpyee.ModifiedOn = DateTime.Now;
                        _GRepoEmployee.EditeAsync(_Emplpyee, cancellationToken);
                    }

                    #endregion

                    if (request.UserRoles != null && request.UserRoles.Length > 0)
                    {
                        var ur = request.UserRoles.ToString();
                        var userrole = from val in ur.Split(',')
                                       select int.Parse(val);
                        foreach (var role in userrole)
                        {
                            if (allRoles.Contains(role))
                            {
                                var UserRole = new UserRole
                                {
                                    UserId = user.Id,
                                    RoleId = role,
                                    CreatedBy = _loggedInUser.Id,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = DateTime.Now,
                                };
                                _GRepoUserRole.SaveAsync(UserRole, cancellationToken);
                            }
                        }
                    }
                    var UserAllowedDocTypeDepSecurityLevel = new UserAllowedDocTypesSecurityLevels()
                    {
                        UserId = user.Id,
                        AllowedBranchId = "," + request.AllowedBranchs + ",",
                        CreatedBy = _loggedInUser.Id,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                    };
                    _GRepoUserAllowedDocTypesSecurity.SaveAsync(UserAllowedDocTypeDepSecurityLevel, cancellationToken);
                    var NewUser = new
                    {
                        user.UserName,
                        user.Email,
                        Password = GeneratedPassword,
                    };

                    return _message.Saved(NewUser);
                }
                else
                {
                    if (result.Errors.Any())
                        return _message.InternalSystemError(result.Errors.FirstOrDefault()?.Description ?? string.Empty);
                    else
                        return _message.InternalSystemError(_umslocalizeMessage.UserNotCreated);
                }
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
    }
}
