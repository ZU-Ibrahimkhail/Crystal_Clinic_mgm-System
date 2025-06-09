using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.UserReport.UserRolePermission.Queries
{
    public class UserRolePermissionHandler : IRequestHandler<UserRolePermissionQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserRole> _GRepoUserRole;
        private readonly IMapper _mapper;
        private readonly UMS_DbContext _umsDbContext;
        private readonly IGeneralHelperRepositoryAsync _helper;
        public UserRolePermissionHandler(IGenericRepositoryAsync<UMS_DbContext, UserRole> GRepoUserRole, IMapper mapper, UMS_DbContext umsDbContext, IGeneralHelperRepositoryAsync helper)
        {
            _GRepoUserRole = GRepoUserRole;
            _mapper = mapper;
            _umsDbContext = umsDbContext;
            _helper = helper;
        }
        public async Task<JsonResult> Handle(UserRolePermissionQuery request, CancellationToken cancellationToken)
        {
            var language = GeneralHelper.SelectedLanauge(request.Language);
            List<UserRolePermissionModel> usersRolePermission = [];
            var userList = await _umsDbContext.Users
                .Where(i => !i.IsDeleted
                && (request.UserID == null || request.UserID == Guid.Empty || i.Id == request.UserID)
                 && (request.IsActive == false || i.IsActive == request.IsActive)
                 && (request.BranchId == 0 || i.BranchId == request.BranchId)
                )
                .Select(x => new
                {
                    x.UserName,
                    x.Id,
                    x.Email,
                    x.BranchId,
                    BranchName = _helper.GetUserCurrentBranchName(language, x.Id),
                    x.EmployeeId,
                    x.PhoneNumber,
                    x.IsActive,
                }).ToListAsync(cancellationToken);

            foreach (var user in userList)
            {
                var userRoles = await _GRepoUserRole.FindByCondition(ur => ur.UserId == user.Id)
                    .Select(ur => new ListRoleViewModel
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name ?? string.Empty,
                        Description = ur.Role.RoleDescription,
                        ApplicationId = ur.Role.ApplicationId,
                        Application = ur.Role.Application!.Abbrevation,
                        IsDeleted = ur.Role.IsDeleted,
                        // Checked = ur.Role.Checked,  // Adjust as necessary
                        TotalPermissions = ur.Role.TotalPermissions // Adjust as necessary
                    }).ToListAsync(cancellationToken);
                usersRolePermission.Add(new UserRolePermissionModel
                {
                    UserName = user.UserName ?? string.Empty,
                    UserID = user.Id,
                    Email = user.Email ?? string.Empty,
                    BranchId = user.BranchId,
                    BranchName = user.BranchName,
                    EmployeeId = user.EmployeeId,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    IsActive = user.IsActive,
                    UserRoles = userRoles
                });
            }
            return new JsonResult(usersRolePermission);
        }

    }

}
