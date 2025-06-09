using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Command.Create
{
    public class CreateHandler(
        IGenericRepositoryAsync<ERP_DbContext, Partners> genericRepositoryAsync,
        IStringLocalizer<CommonValidationResource> localizer,
        IStringLocalizer<CommonColumnNameResource> ColumnLocalizer,
        IMessage message,
        ILoggedInUser loggedInUser,
        IGeneralHelperRepositoryAsync<ERP_DbContext, Partners> helper) : IRequestHandler<CreatePartnersCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, Partners> _genericRepositoryAsync = genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = ColumnLocalizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, Partners> _helper = helper;

        public async Task<JsonResult> Handle(CreatePartnersCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreatePartnersValidator(_Localizer, _ColumnLocalizer, _helper).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            var entity = new Partners
            {
                NameInEnglish = request.NameInEnglish,
                NameInPashto = request.NameInPashto,
                Email = request.Email,
                Phone = request.Phone,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
            };

            return await _genericRepositoryAsync.AddAsync(entity, cancellationToken);

        }
    }
}
