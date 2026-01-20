using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;
using System.Reflection.PortableExecutable;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Update
{
    public class UpdateContractDetailsHandler : IRequestHandler<UpdateContractDetailsCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _genericRepositoryAsync;
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployeeProfile;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoApplicationUser;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;

        public UpdateContractDetailsHandler(
            IGenericRepositoryAsync<ERP_DbContext, ContractDetails> genericRepositoryAsync,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IMessage message,
            ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployeeProfile,
            IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> gRepoApplicationUser)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _Localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _message = message;
            _loggedInUser = loggedInUser;
            _GRepoEmployeeProfile = gRepoEmployeeProfile;
            _GRepoApplicationUser = gRepoApplicationUser;
        }

        public async Task<JsonResult> Handle(UpdateContractDetailsCommand request, CancellationToken cancellationToken)
        {

            var validator = new UpdateContractDetailsValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            var ActiveRecords = _genericRepositoryAsync.FindByCondition(x => !x.IsDeleted
            && x.IsActive
            && x.EmployeeProfileId == request.EmployeeProfileId
            && x.ID != request.ID).Any();

            if (request.IsActive && ActiveRecords)
            {
                validator.Add(new FluentValidation.Results.ValidationFailure(
                                                           _ColumnLocalizer[nameof(request.EmployeeProfileId)],
                                                           $"{_Localizer["ActiveContractAlreadyExists"]}"));
            }
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            var entity = await _genericRepositoryAsync.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }

            entity.EmployeeProfileId = request.EmployeeProfileId;
            entity.AttachmentPath = request.Attachment;
            entity.ContractTypeId = request.ContractTypeId;
            entity.PositionTitleId = request.PositionTitleId;
            entity.CurrencyTypeId = request.CurrencyTypeId;
            entity.BranchId = request.BranchId;
            entity.SalaryAmount = request.SalaryAmount;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.IsActive = request.IsActive;
            entity.Remarks = request.Remarks ?? entity.Remarks;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            _genericRepositoryAsync.EditeAsync(entity, cancellationToken);

            var employee = await _GRepoEmployeeProfile.GetDetailAsync(request.EmployeeProfileId);
            if (employee != null)
            {
                employee.BranchId = request.BranchId;
                employee.ModifiedOn = DateTime.Now;
                employee.ModifiedBy = _loggedInUser.Id;
                _GRepoEmployeeProfile.EditeAsync(employee, cancellationToken);
            }

            var user = _GRepoApplicationUser.FindByCondition(x => !x.IsDeleted && x.EmployeeId == request.EmployeeProfileId).FirstOrDefault();
            if (user != null)
            {
                user.BranchId = request.BranchId;
                user.ModifiedOn = DateTime.Now;
                user.ModifiedBy = _loggedInUser.Id;
                _GRepoApplicationUser.EditeAsync(user, cancellationToken);

            }
            return _message.Update();

        }
    }
}
