using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Commands.GeneratePayrollList
{
    public class GeneratePayrollListHandler(IGenericRepositoryAsync<ERP_DbContext, PayrollTracking> gRepoPayrollTracking, IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> gRepoAdvancePayment, IGenericRepositoryAsync<ERP_DbContext, ContractDetails> gRepoContractDetails, IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployeeProfile, IGenericRepositoryAsync<ERP_DbContext, MainAccount> gRepoMainAccount, IGenericRepositoryAsync<ERP_DbContext, AccountTracking> gRepoAccountTracking, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer, IMessage message, ILoggedInUser loggedInUser) : IRequestHandler<GeneratePayrollListCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, PayrollTracking> _GRepoPayrollTracking = gRepoPayrollTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> _GRepoAdvancePayment = gRepoAdvancePayment;
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _GRepoContractDetails = gRepoContractDetails;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = columnLocalizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;

        public async Task<JsonResult> Handle(GeneratePayrollListCommand request, CancellationToken cancellationToken)
        {


            var IsDuplicatePayment = _GRepoPayrollTracking.Any(x => !x.IsDeleted && x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year);
            if (IsDuplicatePayment)
            {

                return new JsonResult(new Dictionary<string, string>
                {
                    { nameof(PayrollTracking), $"{_Localizer["AlreadyExist"]}" }
                });
            }

            var ActiveContract = await _GRepoContractDetails.FindByCondition(x => !x.IsDeleted && x.IsActive).ToListAsync(cancellationToken);
            if (ActiveContract == null)
            {
                return _message.RecordNotFound();
            }

            foreach (var contract in ActiveContract)
            {
                var baseSalary = contract.SalaryAmount;
                var AdvancePayment = _GRepoAdvancePayment
                      .FindByCondition(ap => ap.EmployeeId == contract.EmployeeProfileId && ap.RemainingBalance > 0).FirstOrDefault();
                var netSalary = baseSalary - (AdvancePayment?.EachInstallmentAmount ?? 0);

                var entity = new PayrollTracking
                {
                    EmployeeId = contract.EmployeeProfileId,
                    ContractDetailsId = contract.ID,
                    CurrencyTypeId = contract.CurrencyTypeId,
                    Date = DateTime.Now,
                    BranchId = contract.BranchId,
                    BaseSalary = baseSalary,
                    AdvanceDeduction = Math.Min(AdvancePayment?.EachInstallmentAmount ?? 0, AdvancePayment?.RemainingBalance ?? 0),
                    NetSalary = netSalary,
                    CreatedOn = DateTime.Now,
                    CreatedBy = _loggedInUser.Id,
                };
                // Create a new payroll record
                _GRepoPayrollTracking.SaveAsync(entity, cancellationToken);

            }

            return _message.Saved();
        }
    }
}
