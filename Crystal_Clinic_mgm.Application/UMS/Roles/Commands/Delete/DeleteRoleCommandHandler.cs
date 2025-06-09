using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Delete
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserRole> _GRepoUserRole;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> _GRepoRole;
        private readonly IGenericRepositoryAsync<UMS_DbContext, RolePermission> _GRepoRolePermission;
        public DeleteRoleCommandHandler(IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, UserRole> gRepoUserRole, IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> gRepoRole, IGenericRepositoryAsync<UMS_DbContext, RolePermission> gRepoRolePermission)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _GRepoUserRole = gRepoUserRole;
            _GRepoRole = gRepoRole;
            _GRepoRolePermission = gRepoRolePermission;
        }
        public async Task<JsonResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            //this is for test
            try
            {
                var role = await _GRepoRole.GetDetailAsync(request.Id);
                if (role == null || role.IsDeleted == true)
                {
                    return _message.RecordNotFound();
                }
                role.IsDeleted = true;
                role.ModifiedBy = _loggedInUser.Id;
                role.ModifiedOn = DateTime.Now;
                role.Remarks = request.Remarks;
                _GRepoRole.EditeAsync(role, cancellationToken);

                // Remove related Records From RolePermission Table
                var rolePermissions = await _GRepoRolePermission.FindByCondition(a => a.RoleId == role.Id && a.IsDeleted == false).ToListAsync(cancellationToken);
                foreach (var rolepermission in rolePermissions)
                {
                    rolepermission.IsDeleted = true;
                    rolepermission.ModifiedBy = _loggedInUser.Id;
                    rolepermission.ModifiedOn = DateTime.Now;
                    rolepermission.Remarks = request.Remarks;
                    _GRepoRolePermission.EditeAsync(rolepermission, cancellationToken);
                }
                // Remove related Records From UserRole Table
                var UserRoles = await _GRepoUserRole.FindByCondition(a => a.RoleId == role.Id && a.IsDeleted == false).ToListAsync(cancellationToken);
                foreach (var UserRole in UserRoles)
                {
                    UserRole.IsDeleted = true;
                    UserRole.ModifiedBy = _loggedInUser.Id;
                    UserRole.ModifiedOn = DateTime.Now;
                    UserRole.Remarks = request.Remarks;
                    _GRepoUserRole.EditeAsync(UserRole, cancellationToken);
                }
                return _message.Delete();
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
    }
}
