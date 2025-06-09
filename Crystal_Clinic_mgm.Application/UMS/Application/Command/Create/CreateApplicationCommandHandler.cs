using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Application.Command.Create
{
    public class CreateApplicationCommondHandler : IRequestHandler<CreateApplicationCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Applications> _genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        public CreateApplicationCommondHandler(IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<UMS_DbContext, Applications> genericRepositoryAsync, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _genericRepositoryAsync = genericRepositoryAsync;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
        }
        public async Task<JsonResult> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateApplicationCommondValidator(_localizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            try
            {
                var app = new Applications
                {
                    Abbrevation = request.Abbrevation,
                    Description = request.Description,
                    DefaultRoute = request.DefaultRoute,
                    IconClass = request.IconClass,
                    Title = request.Title,
                    Area = request.Area,
                    CreatedBy = _loggedInUser.Id,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now

                };
                return await _genericRepositoryAsync.AddAsync(app, cancellationToken);
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
    }
}
