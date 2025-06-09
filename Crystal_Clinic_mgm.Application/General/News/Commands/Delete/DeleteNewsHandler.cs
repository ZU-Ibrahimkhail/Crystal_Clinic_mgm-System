using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.General.News.Commands.Delete
{
    public class DeleteNewsHandler : IRequestHandler<DeleteNewsCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext,Crystal_Clinic_Mgm.Domain.Entities.General.News> _genericRepositoryAsync;
        private readonly IGenericRepositoryAsync<ERP_DbContext, NewsNotification> _NNgenericRepositoryAsync;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;

        public DeleteNewsHandler(IGenericRepositoryAsync<ERP_DbContext, Crystal_Clinic_Mgm.Domain.Entities.General.News> genericRepositoryAsync,
            IGenericRepositoryAsync<ERP_DbContext, NewsNotification> nNgenericRepositoryAsync,
            IMessage message,
            ILoggedInUser loggedInUser)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _NNgenericRepositoryAsync = nNgenericRepositoryAsync;
            _message = message;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(DeleteNewsCommand request, CancellationToken cancelationToken)
        {
            var entity = await _genericRepositoryAsync.FindByCondition(x => x.ID == request.ID && x.CreatedBy == _loggedInUser.Id).SingleOrDefaultAsync(cancelationToken);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }
            entity.IsDeleted = true;
            entity.Remarks = request.Remark;
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedBy = _loggedInUser.Id;

            await _genericRepositoryAsync.DeleteAsync(entity, cancelationToken);

            var getNewsNotification = _NNgenericRepositoryAsync.GetByCondition(x => x.NewsId == request.ID);

            _NNgenericRepositoryAsync.MultiRecordRemoveRangeAsync(getNewsNotification, cancelationToken);

            return _message.Delete();


        }
    }
}
