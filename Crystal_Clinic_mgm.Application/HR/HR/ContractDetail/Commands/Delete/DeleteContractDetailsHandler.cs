using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Delete
{
    public class DeleteContractDetailsHandler : IRequestHandler<DeleteContractDetailsCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _genericRepositoryAsync;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;

        public DeleteContractDetailsHandler(
            IGenericRepositoryAsync<ERP_DbContext, ContractDetails> genericRepositoryAsync,
            IMessage message,
            ILoggedInUser loggedInUser)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _message = message;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(DeleteContractDetailsCommand request, CancellationToken cancellationToken)
        {


            var entity = await _genericRepositoryAsync.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }

            entity.IsDeleted = true;
            entity.Remarks = request.Remarks ?? entity.Remarks;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            return await _genericRepositoryAsync.DeleteAsync(entity, cancellationToken);

        }
    }
}
