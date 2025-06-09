using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Update
{
    public class UpdateTrainingVideoHandler : IRequestHandler<UpdateTrainingVideoCommand, JsonResult>
    {
        public readonly IGenericRepositoryAsync<ERP_DbContext, TrainingVideo> _genericRepositoryAsync;
        public readonly IStringLocalizer<CommonValidationResource> _localizer;
        public readonly IStringLocalizer<CommonColumnNameResource> _columnlocalizer;
        public readonly IMessage _message;
        public readonly ILoggedInUser _loggedInUser;

        public UpdateTrainingVideoHandler(
            IGenericRepositoryAsync<ERP_DbContext, TrainingVideo> genericRepositoryAsync,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnlocalizer,
            IMessage message,
            ILoggedInUser loggedInUser)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _localizer = localizer;
            _columnlocalizer = columnlocalizer;
            _message = message;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(UpdateTrainingVideoCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateTrainingVideoValidator(_localizer, _columnlocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }


            var entity = await _genericRepositoryAsync.GetDetailAsync(request.id);
            if (entity == null || entity.IsDeleted == true)
            {
                return _message.RecordNotFound();
            }

            if (request.Poster != null && request.Poster.FileName.Length > 0)
            {
                FileHandler fileHandler = new();
                await fileHandler.RemoveFile("wwwroot", entity.Poster);
                string PosterPath = await fileHandler.CreateAsync(request.Poster!.OpenReadStream(),
                                  Path.GetExtension(request.Poster.FileName), "wwwroot", AppConfig.TrainingAttachment);
                entity.Poster = PosterPath;
            }
            if (request.DariVideoPath != null && request.DariVideoPath.FileName.Length > 0)
            {
                FileHandler fileHandler = new();
                await fileHandler.RemoveFile("wwwroot", entity.DariVideoPath);
                string DariVideoPath = await fileHandler.CreateAsync(request.DariVideoPath!.OpenReadStream(),
                                  Path.GetExtension(request.DariVideoPath.FileName), "wwwroot", AppConfig.TrainingAttachment);
                entity.DariVideoPath = DariVideoPath;
            }
            if (request.PashtoVideoPath != null && request.PashtoVideoPath.FileName.Length > 0)
            {
                FileHandler fileHandler = new();
                await fileHandler.RemoveFile("wwwroot", entity.PashtoVideoPath);
                string PashtoVideoPath = await fileHandler.CreateAsync(request.PashtoVideoPath!.OpenReadStream(),
                                  Path.GetExtension(request.PashtoVideoPath.FileName), "wwwroot", AppConfig.TrainingAttachment);
                entity.DariVideoPath = PashtoVideoPath;
            }

            entity.DariTitle = request.DariTitle;
            entity.PashtoTitle = request.PashtoTitle;
            entity.Application = request.Application;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            _genericRepositoryAsync.EditeAsync(entity, cancellationToken);
            return _message.Saved();
        }
    }
}