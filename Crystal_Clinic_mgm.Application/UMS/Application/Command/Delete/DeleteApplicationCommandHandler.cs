using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Application.Command.Delete
{
    public class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Applications> _genericRepositoryAsync;
        public DeleteApplicationCommandHandler(IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, Applications> genericRepositoryAsync)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _genericRepositoryAsync = genericRepositoryAsync;
        }
        public async Task<JsonResult> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
        {
            if (request.ID > 0)
            {
                var entity = await _genericRepositoryAsync.GetDetailAsync(request.ID);
                if (entity == null || entity.IsDeleted == true)
                {
                    return _message.RecordNotFound();
                }
                else
                {
                    entity.IsDeleted = true;
                    entity.Remarks = request.Remarks;
                    entity.ModifiedBy = _loggedInUser.Id;
                    entity.ModifiedOn = DateTime.Now;
                    return await _genericRepositoryAsync.DeleteAsync(entity, cancellationToken);
                }
            }
            else
            {
                return _message.IdErrorMessage();
            }


        }
    }
}
