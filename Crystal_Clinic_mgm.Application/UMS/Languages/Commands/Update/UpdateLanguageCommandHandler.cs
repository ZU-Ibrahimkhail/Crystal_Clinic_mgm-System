using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Exceptions;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Commands.Update
{
    public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Crystal_Clinic_Mgm.Domain.Entities.UMS.Language> _genericRepository;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IGeneralHelperRepositoryAsync<UMS_DbContext, Crystal_Clinic_Mgm.Domain.Entities.UMS.Language> _helper;
        private readonly ILoggedInUser _loggedInUser;

        public UpdateLanguageCommandHandler(
            IMessage message,
            IGenericRepositoryAsync<UMS_DbContext, Crystal_Clinic_Mgm.Domain.Entities.UMS.Language> genericRepository,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IGeneralHelperRepositoryAsync<UMS_DbContext, Crystal_Clinic_Mgm.Domain.Entities.UMS.Language> helper,
            ILoggedInUser loggedInUser)
        {
            _message = message;
            _genericRepository = genericRepository;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _helper = helper;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLanguageCommandValidator(_helper, _localizer, _ColumnLocalizer).Validate(request).Errors;

            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            var entity = await _genericRepository.GetDetailAsync(request.ID);
            if (entity != null)
            {
                entity.PashtoName = request.PashtoName;
                entity.DariName = request.DariName;
                entity.Code = request.Code;
                entity.ModifiedBy = _loggedInUser.Id;
                entity.ModifiedOn = DateTime.Now;
                return await _genericRepository.UpdateAsync(entity, cancellationToken);
            }
            else
            {
                throw new NotFoundException(nameof(UpdateLanguageCommand), request.ID);
            }
        }
    }
}
