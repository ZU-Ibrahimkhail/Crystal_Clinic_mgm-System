using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Delete
{
    public class DeletDeletePositionTitleCommande : IRequestHandler<DeletePositionTitleCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<ERP_DbContext, PositionTitle> _genericRepositoryAsync;

        public DeletDeletePositionTitleCommande(
            IMessage message,
            ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<ERP_DbContext, PositionTitle> genericRepositoryAsync)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _genericRepositoryAsync = genericRepositoryAsync;
        }

        public async Task<JsonResult> Handle(DeletePositionTitleCommand request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var entity = await _genericRepositoryAsync.GetDetailAsync(request.Id);
                if (entity == null || entity.IsDeleted)
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
