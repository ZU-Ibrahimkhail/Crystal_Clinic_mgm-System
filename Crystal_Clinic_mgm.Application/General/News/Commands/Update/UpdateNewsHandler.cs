using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.General.News.Commands.Update
{
    public class UpdateNewsHandler : IRequestHandler<UpdateNewsCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, Crystal_Clinic_Mgm.Domain.Entities.General.News> _genericRepositoryAsync;
        private readonly IGenericRepositoryAsync<ERP_DbContext, NewsNotification> _NNgenericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;

        public UpdateNewsHandler(IGenericRepositoryAsync<ERP_DbContext, Crystal_Clinic_Mgm.Domain.Entities.General.News> genericRepositoryAsync,
                                 IGenericRepositoryAsync<ERP_DbContext, NewsNotification> nNgenericRepositoryAsync,
                                 IStringLocalizer<CommonValidationResource> localizer,
                                 IStringLocalizer<CommonColumnNameResource> columnLocalizer,
                                 IMessage message,
                                 ILoggedInUser loggedInUser)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _NNgenericRepositoryAsync = nNgenericRepositoryAsync;
            _Localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _message = message;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateNewsValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #region update news
            var entity = await _genericRepositoryAsync.FindByCondition(x => x.ID == request.Id && x.CreatedBy == _loggedInUser.Id).SingleOrDefaultAsync(cancellationToken);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound();
            }
            if (entity.NewsDate.Date <= DateTime.Now.Date && (entity.EndTime == null || entity.EndTime.Value.TimeOfDay < DateTime.Now.TimeOfDay))
            {
                return new JsonResult(_Localizer["NewsExpired"].Value);
            }

            
            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.StartTime = request.StartTime;
            entity.EndTime = request.EndTime;
            entity.NewsDate = request.NewsDate;
            entity.Speaker = request.Speaker;
            entity.Location = request.Location;
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.AttachmentPath = request.Attachment;

            _genericRepositoryAsync.EditeAsync(entity, cancellationToken);
            #endregion



            return _message.Saved();
        }
    }
}
