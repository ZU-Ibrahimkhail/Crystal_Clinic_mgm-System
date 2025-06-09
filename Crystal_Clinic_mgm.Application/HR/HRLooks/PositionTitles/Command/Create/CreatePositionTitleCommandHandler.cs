using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Create
{
    public class CreatePositionTitleCommandHandler : IRequestHandler<CreatePositionTitleCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<ERP_DbContext, PositionTitle> _genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _columnLocalizer;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, PositionTitle> _helper;

        public CreatePositionTitleCommandHandler(
            IMessage message,
            ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<ERP_DbContext, PositionTitle> genericRepositoryAsync,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IGeneralHelperRepositoryAsync<ERP_DbContext, PositionTitle> helper)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _genericRepositoryAsync = genericRepositoryAsync;
            _localizer = localizer;
            _columnLocalizer = columnLocalizer;
            _helper = helper;
        }

        public async Task<JsonResult> Handle(CreatePositionTitleCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreatePositionTitleCommandValidator(_helper, _columnLocalizer, _localizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            var entity = new PositionTitle()
            {
                EnglishName = request.EnglishName,
                PashtoName = request.PashtoName,
                DariName = request.DariName,
                Code = request.Code,
                BranchId = request.BranchId,
                IsActive = request.IsActive,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            return await _genericRepositoryAsync.AddAsync(entity, cancellationToken);

        }
    }
}
