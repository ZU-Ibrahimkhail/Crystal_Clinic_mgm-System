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

namespace Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Create
{
    public class CreateTrainingVideoCommandHandler : IRequestHandler<CreateTrainingVideoCommand, JsonResult>
    {
        public readonly IGenericRepositoryAsync<ERP_DbContext, TrainingVideo> _genericRepositoryAsync;
        public readonly IStringLocalizer<CommonValidationResource> _localizer;
        public readonly IStringLocalizer<CommonColumnNameResource> _columnlocalizer;
        public readonly IMessage _message;
        public readonly ILoggedInUser _loggedInUser;

        public CreateTrainingVideoCommandHandler(
            IGenericRepositoryAsync<ERP_DbContext, TrainingVideo> genericRepositoryAsync,
            IMessage message,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnlocalizer,
            ILoggedInUser loggedInUser)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _message = message;
            _localizer = localizer;
            _columnlocalizer = columnlocalizer;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(CreateTrainingVideoCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateTrainingVideoValidator(_localizer, _columnlocalizer).Validate(request).Errors;

            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }

            try
            {
                string PosterPath = "";
                if (request.Poster != null)
                {
                    PosterPath = await new FileHandler().CreateAsync(request.Poster!.OpenReadStream(),
                                      Path.GetExtension(request.Poster.FileName), "wwwroot", AppConfig.TrainingAttachment);
                }
                string DariVideoPath = "";
                if (request.DariVideoPath != null)
                {
                    DariVideoPath = await new FileHandler().CreateAsync(request.DariVideoPath!.OpenReadStream(),
                                      Path.GetExtension(request.DariVideoPath.FileName), "wwwroot", AppConfig.TrainingAttachment);
                }
                string PashtoVideoPath = "";
                if (request.PashtoVideoPath != null)
                {
                    PashtoVideoPath = await new FileHandler().CreateAsync(request.PashtoVideoPath!.OpenReadStream(),
                                      Path.GetExtension(request.PashtoVideoPath.FileName), "wwwroot", AppConfig.TrainingAttachment);
                }
                var entity = new TrainingVideo()
                {
                    DariTitle = request.DariTitle,
                    PashtoTitle = request.PashtoTitle,
                    Application = request.Application,
                    Poster = PosterPath,
                    DariVideoPath = DariVideoPath,
                    PashtoVideoPath = PashtoVideoPath,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    CreatedBy = _loggedInUser.Id,
                };
                _genericRepositoryAsync.SaveAsync(entity, cancellationToken);
                return _message.Saved();
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
    }
}