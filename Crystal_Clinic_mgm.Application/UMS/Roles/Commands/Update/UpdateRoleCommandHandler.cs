using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Update
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> _GRepoRole;
        private readonly IGenericRepositoryAsync<UMS_DbContext, RolePermission> _GRepoRolePermission;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IStringLocalizer<UMSValidationResource> _Umslocalizer;
        private readonly IGeneralHelperRepositoryAsync<UMS_DbContext> _helper;

        public UpdateRoleCommandHandler(
            IMessage message,
            ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<UMS_DbContext, ApplicationRole> gRepoRole,
            IGenericRepositoryAsync<UMS_DbContext, RolePermission> gRepoRolePermission,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IStringLocalizer<UMSValidationResource> umslocalizer,
            IGeneralHelperRepositoryAsync<UMS_DbContext> helper)
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

        public async Task<JsonResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateRoleCommandValidator(_helper, _localizer, _Umslocalizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            try
            {
                //if (request.Id != request.RequestId)
                //{
                //    return _message.InternalSystemError(UMSLocalizeMessage.InvalideRequestId + request.Id);
                //}
                var role = await _GRepoRole.GetDetailAsync(request.Id);
                if (role == null || role.IsDeleted == true)
                {
                    return _message.RecordNotFound();
                }
                role.Name = request.Name;
                role.RoleDescription = request.Description;
                role.ApplicationId = request.ApplicationId;
                role.ModifiedBy = _loggedInUser.Id;
                role.ModifiedOn = DateTime.Now;
                _GRepoRole.EditeAsync(role, cancellationToken);

                var OldRolePermisssion = await _GRepoRolePermission.FindByCondition(a => a.RoleId == role.Id).ToListAsync(cancellationToken);
                if (OldRolePermisssion != null)
                {
                    _GRepoRolePermission.MultiRecordRemoveRangeAsync(OldRolePermisssion, cancellationToken);
                }

                if (request.PermissionIds.Count > 0)
                {
                    foreach (var item in request.PermissionIds)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = item,
                            CreatedBy = _loggedInUser.Id,
                            ModifiedOn = DateTime.Now,
                            CreatedOn = DateTime.Now
                        };
                        _GRepoRolePermission.SaveAsync(rolePermission, cancellationToken);
                    }
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
