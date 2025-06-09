using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.AssetTypes.Commands.Update
{
    public class UpdateHandler(
        IGenericRepositoryAsync<ERP_DbContext, AssetType> genericRepositoryAsync,
        IStringLocalizer<CommonValidationResource> localizer,
        IGeneralHelperRepositoryAsync<ERP_DbContext, AssetType> _helper,
        IMessage message,
        ILoggedInUser loggedInUser) : IRequestHandler<UpdateAssetTypeCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, AssetType> _genericRepositoryAsync = genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, AssetType> _helper = _helper;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;

        public async Task<JsonResult> Handle(UpdateAssetTypeCommand request, CancellationToken cancellationToken)
        {

            var validator = new UpdateAssetTypeValidator(_Localizer, _helper).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            var entity = await _genericRepositoryAsync.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }

            entity.EnglishName = request.EnglishName;
            entity.DariName = request.DariName;
            entity.PashtoName = request.PashtoName;
            entity.Code = request.Code;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            return await _genericRepositoryAsync.UpdateAsync(entity, cancellationToken);

        }
    }
}
