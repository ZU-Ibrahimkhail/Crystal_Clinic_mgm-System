using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDetail
{
    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, JsonResult>
    {
        private readonly IMessage _message;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserAllowedDocTypesSecurityLevels> _GRepoUserAllowedDocTypesSecurity;
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserRole> _GRepoUserRole;
        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GRepoBranch;
        private readonly IGeneralHelperRepositoryAsync _helper;
        readonly UMSLocalizeMessage _umslocalizeMessage;



        public GetUserDetailQueryHandler(
            IMessage message,
            UserManager<ApplicationUser> userManager,
            IGenericRepositoryAsync<UMS_DbContext, UserAllowedDocTypesSecurityLevels> gRepoUserAllowedDocTypesSecurity,
            IGenericRepositoryAsync<UMS_DbContext, UserRole> gRepoUserRole,
            IGenericRepositoryAsync<ERP_DbContext, Branch> gRepoBranch,
            IStringLocalizer<UMSValidationResource> umsLocalizer,
            IGeneralHelperRepositoryAsync helper)
        {
            _message = message;
            _userManager = userManager;
            _GRepoUserAllowedDocTypesSecurity = gRepoUserAllowedDocTypesSecurity;
            _GRepoUserRole = gRepoUserRole;
            _GRepoBranch = gRepoBranch;
            _umslocalizeMessage = new(umsLocalizer);
            _helper = helper;
        }

        public async Task<JsonResult> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = null;
            var Language = GeneralHelper.SelectedLanauge(request.Language);
            if (request.Id != null)
            {
                user = await _userManager.FindByIdAsync(request.Id.ToString()!);
            }
            else if (request.UserName != null)
            {
                user = await _userManager.FindByNameAsync(request.UserName);
            }
            else if (request.Email != null)
            {
                user = await _userManager.FindByEmailAsync(request.Email);
            }
            if (user == null)
            {
                return _message.RecordNotFound();
            }
            string allowedbranchlevel = _GRepoUserAllowedDocTypesSecurity.FindByCondition(x => x.IsDeleted == false && x.UserId == user.Id).FirstOrDefault()?.AllowedBranchId ?? string.Empty;

            if (user.IsDeleted == false)
            {
                var UserRole = await _GRepoUserRole.FindByCondition(ur => ur.UserId == user.Id)
                                           .Select(ur => new ListRoleViewModel()
                                           {
                                               Id = ur.Role.Id,
                                               Name = ur.Role.Name ?? string.Empty,
                                               Description = ur.Role.RoleDescription,
                                               ApplicationId = ur.Role.ApplicationId,
                                               Application = ur.Role.Application!.Abbrevation
                                           }).ToListAsync(cancellationToken);

                var branchs = await _GRepoBranch.FindByCondition(x => x.IsDeleted == false && allowedbranchlevel.Contains(x.ID.ToString())).Select(x => new GetDropDownGeneralModels()
                {
                    Id = x.ID,
                    EnglishName = x.EnglishName,
                    PashtoName = x.PashtoName,
                    DariName = x.DariName,

                }).ToListAsync(cancellationToken);



                var Branch = _GRepoBranch.FindByCondition(x => x.IsDeleted == false && x.ID == user.BranchId).FirstOrDefault();
                Localization Localize = new();
                var model = new UserDetailModel
                {
                    ID = user.Id.ToString(),
                    EmployeeId = user.EmployeeId,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    IsActive = user.IsActive,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    BranchId = user.BranchId,
                    BranchName = Localize.GetName(Language, Branch),
                    PositionName = _helper.GetUserPosition(Language, user.Id),
                    OwnerID = user.EmployeeId ?? 0,
                    OwnerName = _helper.GetUserName(Language, user.Id),
                    PhotoPath = _helper.GetUserPhotoPath(user.Id),
                    UserRoles = UserRole,
                    AllowedbranchlevelModels = branchs,
                };

                return new JsonResult(model);
            }
            else
            {
                return new JsonResult(new { message = _umslocalizeMessage.IsActiveAndIsDelete });

            }
        }
    }
}
