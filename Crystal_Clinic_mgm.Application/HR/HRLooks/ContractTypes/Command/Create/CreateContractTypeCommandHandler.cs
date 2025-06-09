using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.ContractTypes.Command.Create
{
    public class CreateContractTypeCommandHandler : IRequestHandler<CreateContractTypeCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractType> _genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _columnLocalizer;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, ContractType> _helper;

        public CreateContractTypeCommandHandler(
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

        public async Task<JsonResult> Handle(CreateContractTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateContractTypeCommandValidator(_helper, _localizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            var entity = new ContractType()
            {
                EnglishName = request.EnglishName,
                PashtoName = request.PashtoName,
                DariName = request.DariName,
                Code = request.Code,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            return await _genericRepositoryAsync.AddAsync(entity, cancellationToken);

        }
    }
}
