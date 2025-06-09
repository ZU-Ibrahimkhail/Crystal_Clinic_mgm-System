using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ActiveUser
{
    public class ActiveUserCommandHandler : IRequestHandler<ActiveUserCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _genericRepositoryAsync;
        public ActiveUserCommandHandler(IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> genericRepositoryAsync)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _genericRepositoryAsync = genericRepositoryAsync;
        }
        public async Task<JsonResult> Handle(ActiveUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _genericRepositoryAsync
                .FindByCondition(c => c.Id == request.ID && c.IsDeleted == false && c.Id != _loggedInUser.Id && c.UserName != "SuperAdmin").SingleOrDefaultAsync(cancellationToken);
            if (entity == null)
                return _message.RecordNotFound();
            else
            {
                entity.IsActive = request.IsActive;
                entity.ModifiedBy = _loggedInUser.Id;
                entity.ModifiedOn = DateTime.Now;
                return await _genericRepositoryAsync.UpdateAsync(entity, cancellationToken);
            }
        }
    }
}
