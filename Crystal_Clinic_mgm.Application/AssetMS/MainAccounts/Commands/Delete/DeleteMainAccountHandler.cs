using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Delete
{
    public class DeleteMainAccountHandler(
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> genericRepositoryAsync,
        IMessage message,
        ILoggedInUser loggedInUser) : IRequestHandler<DeleteMainAccountCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _genericRepositoryAsync = genericRepositoryAsync;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;

        public async Task<JsonResult> Handle(DeleteMainAccountCommand request, CancellationToken cancellationToken)
        {


            var entity = await _genericRepositoryAsync.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }

            entity.IsDeleted = true;
            entity.Remarks = request.Remarks;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            return await _genericRepositoryAsync.DeleteAsync(entity, cancellationToken);

        }
    }
}
