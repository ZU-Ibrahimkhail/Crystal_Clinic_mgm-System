using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.ContractDetail.Commands.Create
{
    public class CreateContractDetailsHandler : IRequestHandler<CreateContractDetailsCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _genericRepositoryAsync;
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployeeProfile;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoApplicationUser;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;

        public CreateContractDetailsHandler(
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

        public async Task<JsonResult> Handle(CreateContractDetailsCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateContractDetailsValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;

            if (request.IsActive == true)
            {
                var ActiveRecords = _genericRepositoryAsync.FindByCondition(x => !x.IsDeleted && x.IsActive && x.EmployeeProfileId == request.EmployeeProfileId).Any();
                if (ActiveRecords)
                {
                    validator.Add(new FluentValidation.Results.ValidationFailure(
                                                                             _ColumnLocalizer[nameof(request.EmployeeProfileId)],
                                                                             $"{_Localizer["ActiveContractAlreadyExists"]}"));
                }
            }

            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }


            var entity = new ContractDetails
            {
                EmployeeProfileId = request.EmployeeProfileId,
                ContractTypeId = request.ContractTypeId,
                PositionTitleId = request.PositionTitleId,
                CurrencyTypeId = request.CurrencyTypeId,
                SalaryAmount = request.SalaryAmount,
                BranchId = request.BranchId,
                StartDate = request.StartDate,
                EndDate = request.EndDate ?? null,
                IsActive = request.IsActive,
                Remarks = request.Remarks,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
            };
            _genericRepositoryAsync.SaveAsync(entity, cancellationToken);

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

            return _message.Saved();

        }
    }
}
