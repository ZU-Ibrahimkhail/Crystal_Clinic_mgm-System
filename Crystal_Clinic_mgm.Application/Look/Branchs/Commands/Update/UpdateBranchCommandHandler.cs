using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Commands.Update
{
    public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GenericRepositoryAsync;
        private readonly IGeneralHelperRepositoryAsync<ERP_DbContext, Branch> _helper;
        private readonly IStringLocalizer<CommonValidationResource> _localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;

        public UpdateBranchCommandHandler(
            IMessage message, ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<ERP_DbContext, Branch> genericRepositoryAsync,
            IStringLocalizer<CommonValidationResource> localizer, IGeneralHelperRepositoryAsync<ERP_DbContext, Branch> helper, IStringLocalizer<CommonColumnNameResource> columnLocalizer)
        {
            _message = message;
            _loggedInUser = loggedInUser;
            _GenericRepositoryAsync = genericRepositoryAsync;
            _localizer = localizer;
            _helper = helper;
            _ColumnLocalizer = columnLocalizer;
        }

        public async Task<JsonResult> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _GenericRepositoryAsync.GetDetailAsync(request.Id);
            if (entity == null || entity.IsDeleted == true || !entity.IsActive)
            {
                return _message.RecordNotFound();
            }
            var validator = new UpdateBranchCommandValidator(_localizer, _ColumnLocalizer, _helper).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckValidationError(validator);
            }

            entity.EnglishName = request.EnglishName;
            entity.PashtoName = request.PashtoName;
            entity.DariName = request.DariName;
            entity.Code = request.Code;
            entity.ParentId = request.ParentId;
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedBy = _loggedInUser.Id;
            return await _GenericRepositoryAsync.UpdateAsync(entity, cancellationToken);


        }
    }
}
