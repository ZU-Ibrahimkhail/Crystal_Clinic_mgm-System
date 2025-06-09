using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Exceptions;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, JsonResult>
    {
        private readonly UMS_DbContext _context;
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IMessage _message;
        readonly UMSLocalizeMessage _umslocalizeMessage;
        public DeleteUserCommandHandler(UMS_DbContext context, ILoggedInUser loggedInUser, IMessage message, IStringLocalizer<UMSValidationResource> umsLocalizer, IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee)
        {
            _context = context;
            _loggedInUser = loggedInUser;
            _message = message;
            _umslocalizeMessage = new(umsLocalizer);
            _GRepoEmployee = gRepoEmployee;
        }
        public async Task<JsonResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
            if (entity == null || entity.IsDeleted == true)
            {
                throw new NotFoundException(nameof(ApplicationUser), request.Id);
            }
            if (entity.IsSuperAdmin == true && entity.IsDeleted == false)
            {
                return new JsonResult(new { message = _umslocalizeMessage.SuperadminNotDelete });
            }
            if (entity.IsDeleted == false && entity.Id != _loggedInUser.Id && entity.UserName != "SuperAdmin")
            {
                entity.IsDeleted = true;
                entity.Email += "_";
                entity.NormalizedEmail += "_";
                entity.ModifiedBy = _loggedInUser.Id;
                entity.ModifiedOn = DateTime.Now;
                entity.Remarks = request.Remarks;
                var hasChiled = _context.Users.Any(o => o.IsDeleted == false && o.IsActive == true);
                if (hasChiled)
                {
                    // TODO: Add functional test for this behaviour.
                    throw new DeleteFailureException(nameof(ApplicationUser), request.Id, _umslocalizeMessage.ParentChildRecord);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return _message.Delete();
            }
            else
                return new JsonResult(new { message = _umslocalizeMessage.IsActiveAndIsDelete });
            // _context.Users.Remove(entity);
        }
    }
}
