using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Commands.Create
{
    public class CreateHandler(
        IGenericRepositoryAsync<ERP_DbContext, Crystal_Clinic_Mgm.Domain.Entities.Look.CurrencyType> genericRepositoryAsync,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message,
        ILoggedInUser loggedInUser,
        IGeneralHelperRepositoryAsync<ERP_DbContext, Crystal_Clinic_Mgm.Domain.Entities.Look.CurrencyType> helper) : IRequestHandler<CreateCurrencyTypeCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, Crystal_Clinic_Mgm.Domain.Entities.Look.CurrencyType> _genericRepositoryAsync = genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, Crystal_Clinic_Mgm.Domain.Entities.Look.CurrencyType> _helper = helper;

        public async Task<JsonResult> Handle(CreateCurrencyTypeCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateValidator(_Localizer, _helper).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            var entity = new Crystal_Clinic_Mgm.Domain.Entities.Look.CurrencyType
            {
                EnglishName = request.EnglishName,
                DariName = request.DariName,
                PashtoName = request.PashtoName,
                Code = request.Code,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
            };

            return await _genericRepositoryAsync.AddAsync(entity, cancellationToken);

        }
    }
}
