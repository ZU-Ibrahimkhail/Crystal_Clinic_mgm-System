using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Create
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> _GRepoRole;
        private readonly IGenericRepositoryAsync<UMS_DbContext, RolePermission> _GRepoRolePermission;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IStringLocalizer<UMSValidationResource> _Umslocalizer;
        private readonly IGeneralHelperRepositoryAsync<UMS_DbContext> _helper;
        public CreateRoleCommandHandler(IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> gRepoRole, IGenericRepositoryAsync<UMS_DbContext, RolePermission> gRepoRolePermission, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer, IStringLocalizer<UMSValidationResource> umslocalizer, IGeneralHelperRepositoryAsync<UMS_DbContext> helper)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _GRepoRole = gRepoRole;
            _GRepoRolePermission = gRepoRolePermission;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _Umslocalizer = umslocalizer;
            _helper = helper;
        }
        public async Task<JsonResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var validator = new CreateRoleCommandValidator(_helper, _localizer, _Umslocalizer, _ColumnLocalizer).Validate(request).Errors;
                if (validator.Count > 0)
                {
                    return _message.CheckValidationError(validator);
                }
                var Role = new ApplicationRole
                {
                    Name = request.Name,
                    ApplicationId = request.ApplicationId,
                    RoleDescription = request.Description,
                    CreatedOn = DateTime.Now,
                    CreatedBy = _loggedInUser.Id,
                    IsDeleted = false,
                    ModifiedOn = DateTime.Now
                };
                _GRepoRole.SaveAsync(Role, cancellationToken);
                if (request.PermissionIds.Count > 0)
                {
                    foreach (var item in request.PermissionIds)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleId = Role.Id,
                            PermissionId = item,
                            CreatedBy = _loggedInUser.Id,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        };
                        _GRepoRolePermission.SaveAsync(rolePermission, cancellationToken);
                    }
                }
                return _message.Saved();
            });
        }
    }
}
