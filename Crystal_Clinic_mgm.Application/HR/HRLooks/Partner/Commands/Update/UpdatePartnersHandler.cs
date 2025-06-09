using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Command.Update
{
    public class UpdateHandler(
        IGenericRepositoryAsync<ERP_DbContext, Partners> genericRepositoryAsync,
        IStringLocalizer<CommonValidationResource> localizer,
        IGeneralHelperRepositoryAsync<ERP_DbContext, Partners> _helper,
        IMessage message,
        ILoggedInUser loggedInUser,
        IStringLocalizer<CommonColumnNameResource> columnLocalizer) : IRequestHandler<UpdatePartnersCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, Partners> _genericRepositoryAsync = genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = columnLocalizer;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, Partners> _helper = _helper;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;

        public async Task<JsonResult> Handle(UpdatePartnersCommand request, CancellationToken cancellationToken)
        {

            var validator = new UpdatePartnersValidator(_Localizer,_ColumnLocalizer, _helper).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            var entity = await _genericRepositoryAsync.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }

            entity.ID = request.ID;
            entity.NameInEnglish = request.NameInEnglish;
            entity.NameInPashto = request.NameInPashto;
            entity.Phone = request.Phone;
            entity.Email = request.Email;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            return await _genericRepositoryAsync.UpdateAsync(entity, cancellationToken);

        }
    }
}
