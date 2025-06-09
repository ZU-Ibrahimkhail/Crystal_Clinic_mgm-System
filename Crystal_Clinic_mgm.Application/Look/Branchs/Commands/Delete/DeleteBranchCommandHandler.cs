using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Delete
{
    public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GenericRepositoryAsync;
        public DeleteBranchCommandHandler(IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<ERP_DbContext, Branch> genericRepositoryAsync)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _GenericRepositoryAsync = genericRepositoryAsync;
        }
        public async Task<JsonResult> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var entity = await _GenericRepositoryAsync.GetDetailAsync(request.Id);
                if (entity == null || entity.IsDeleted == true)
                {
                    return _message.RecordNotFound();
                }
                else
                {
                    entity.IsDeleted = true;
                    entity.ModifiedOn = DateTime.Now;
                    entity.ModifiedBy = _loggedInUser.Id;
                    entity.Remarks = request.Remarks;
                    return await _GenericRepositoryAsync.DeleteAsync(entity, cancellationToken);
                }
            }
            else
            {
                return _message.IdErrorMessage();
            }
        }
    }
}
