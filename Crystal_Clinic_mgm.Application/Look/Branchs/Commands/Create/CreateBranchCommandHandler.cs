using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Create
{
    public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GenericRepositoryAsync;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, Branch> _helper;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        public CreateBranchCommandHandler(IMessage message,
                                              ILoggedInUser loggedInUser,
                                              IGenericRepositoryAsync<ERP_DbContext, Branch> genericRepositoryAsync,
                                              IGeneralHelperRepositoryAsync<ERP_DbContext, Branch> helper,
                                              IStringLocalizer<CommonValidationResource> localizer,
                                              IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _GenericRepositoryAsync = genericRepositoryAsync;
            _helper = helper;
            _localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
        }
        public async Task<JsonResult> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateBranchCommandValidator(_localizer, _ColumnLocalizer, _helper).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }
            var branch = new Branch()
            {
                EnglishName = request.EnglishName,
                PashtoName = request.PashtoName,
                DariName = request.DariName,
                Code = request.Code,
                ParentId = request.ParentId,
                Address = request.Address,
                IsActive = true,
                CreatedBy = _loggedInUser.Id,
                ModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            };
            return await _GenericRepositoryAsync.AddAsync(branch, cancellationToken);
        }
    }
}
