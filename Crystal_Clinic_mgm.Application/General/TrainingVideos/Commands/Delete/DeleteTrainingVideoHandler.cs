using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Delete
{
    public class DeleteTrainingVideoHandler : IRequestHandler<DeleteTrainingVideoCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, TrainingVideo> _GRepoasync;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        public DeleteTrainingVideoHandler(IGenericRepositoryAsync<ERP_DbContext, TrainingVideo> gRepoasync, IMessage message, ILoggedInUser loggedInUser)
        {
            _GRepoasync = gRepoasync;
            _message = message;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(DeleteTrainingVideoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _GRepoasync.GetDetailAsync(request.Id);
            if (entity == null || entity.IsDeleted == true)
            {
                return _message.RecordNotFound();
            }
            else
            {
                entity.IsDeleted = true;
                entity.ModifiedBy = _loggedInUser.Id;
                entity.ModifiedOn = DateTime.Now;
                entity.Remarks = request.Remark;

                //var FileToDelete = "wwwroot" + entity.Attachment;
                //System.IO.File.Delete(FileToDelete);
                //return await _GRepoasync.RemoveAsync(entity, cancellationToken);

                await _GRepoasync.DeleteAsync(entity, cancellationToken);
                return _message.Remove();
            }
        }
    }
}