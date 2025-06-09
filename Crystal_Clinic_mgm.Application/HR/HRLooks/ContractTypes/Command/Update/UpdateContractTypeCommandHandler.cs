using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Command.Update
{
    public class UpdateContractTypeCommandHandler : IRequestHandler<UpdateContractTypeCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractType> _genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _columnLocalizer;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, ContractType> _helper;

        public UpdateContractTypeCommandHandler(
            IMessage message,
            ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<ERP_DbContext, ContractType> genericRepositoryAsync,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IGeneralHelperRepositoryAsync<ERP_DbContext, ContractType> helper)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _genericRepositoryAsync = genericRepositoryAsync;
            _localizer = localizer;
            _columnLocalizer = columnLocalizer;
            _helper = helper;
        }

        public async Task<JsonResult> Handle(UpdateContractTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateContractTypeCommandValidator(_helper, _localizer, _columnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            var entity = await _genericRepositoryAsync.GetDetailAsync(request.Id);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound();
            }
            else
            {
                entity.EnglishName = request.EnglishName;
                entity.PashtoName = request.PashtoName;
                entity.DariName = request.DariName;
                entity.Code = request.Code;
                entity.ModifiedBy = _loggedInUser.Id;
                entity.ModifiedOn = DateTime.Now;
                //----Update
                return await _genericRepositoryAsync.UpdateAsync(entity, cancellationToken);
            }

        }
    }
}
