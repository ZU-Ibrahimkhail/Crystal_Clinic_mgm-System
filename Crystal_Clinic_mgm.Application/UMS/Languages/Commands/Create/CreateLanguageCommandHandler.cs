using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;



namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Commands.Create
{
    public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, JsonResult>
    {
        private readonly ILoggedInUser _loggedInUser;
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Language> _genericRepository;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IGeneralHelperRepositoryAsync<UMS_DbContext, Language> _helper;

        public CreateLanguageCommandHandler(
             ILoggedInUser loggedInUser,
            IMessage message,
            IGenericRepositoryAsync<UMS_DbContext, Language> genericRepository,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IGeneralHelperRepositoryAsync<UMS_DbContext, Language> helper
            )
        {
            _message = message;
            _genericRepository = genericRepository;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _helper = helper;
            _loggedInUser = loggedInUser;
        }
        public async Task<JsonResult> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLanguageCommandValidator(_helper, _localizer).Validate(request).Errors;

            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            var gid = _loggedInUser.Id;
            var entity = new Language
            {
                EnglishName = request.EnglishName,
                PashtoName = request.PashtoName,
                DariName = request.DariName,
                Code = request.Code,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                IsDeleted = false
            };
            return await _genericRepository.AddAsync(entity, cancellationToken);
        }
    }
}
