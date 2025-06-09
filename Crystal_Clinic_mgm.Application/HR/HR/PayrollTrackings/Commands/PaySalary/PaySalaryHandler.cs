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
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crystal_Clinic_Mgm.Application.HR.HR.PayrollTrackings.Commands.PaySalary
{
    public class PaySalaryHandler : IRequestHandler<PaySalaryCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, PayrollTracking> _genericRepositoryAsync;
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployeeProfile;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoApplicationUser;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;

        public PaySalaryHandler(
            IGenericRepositoryAsync<ERP_DbContext, PayrollTracking> genericRepositoryAsync,
            IStringLocalizer<CommonValidationResource> localizer,
            IStringLocalizer<CommonColumnNameResource> columnLocalizer,
            IMessage message,
            ILoggedInUser loggedInUser,
            IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployeeProfile,
            IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> gRepoApplicationUser,
            IGenericRepositoryAsync<ERP_DbContext, MainAccount> gRepoMainAccount,
            IGenericRepositoryAsync<ERP_DbContext, AccountTracking> gRepoAccountTracking)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _Localizer = localizer;
            _ColumnLocalizer = columnLocalizer;
            _message = message;
            _loggedInUser = loggedInUser;
            _GRepoEmployeeProfile = gRepoEmployeeProfile;
            _GRepoApplicationUser = gRepoApplicationUser;
            _GRepoMainAccount = gRepoMainAccount;
            _GRepoAccountTracking = gRepoAccountTracking;
        }

        public async Task<JsonResult> Handle(PaySalaryCommand request, CancellationToken cancellationToken)
        {
            #region Check Validation 

            var validator = new PaySalaryValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == request.MainAccountId).FirstOrDefault();
            var PayRollRecord = _genericRepositoryAsync.FindByCondition(x => !x.IsDeleted && x.ID == request.PayrollId).FirstOrDefault();

            if (mainAccount == null || PayRollRecord == null || PayRollRecord.IsPayed) return _message.RecordNotFound();
            if (mainAccount.CurrencyTypeId != PayRollRecord.CurrencyTypeId)
            {
                validator.Add(new FluentValidation.Results.ValidationFailure(
                                                           _ColumnLocalizer[nameof(mainAccount.CurrencyTypeId)],
                                                           $"{mainAccount.CurrencyTypeId} {_Localizer["DoesNotMatch"]} {PayRollRecord.CurrencyTypeId}"));
            }
            if (mainAccount.BalanceAmount < request.NetSalary)
            {
                validator.Add(new FluentValidation.Results.ValidationFailure(
                                                               _ColumnLocalizer[nameof(request.NetSalary)],
                                                               $"{_ColumnLocalizer[nameof(request.NetSalary)]} {_Localizer["IsGreaterThen"]} {_ColumnLocalizer[nameof(mainAccount.BalanceAmount)]} : {mainAccount.BalanceAmount}"));
            }

            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #endregion

            #region Update the payroll 
            PayRollRecord.EmployeeId = request.EmployeeId;
            PayRollRecord.ContractDetailsId = request.ContractDetailsId;
            PayRollRecord.PayTypeId = request.PayTypeId;
            PayRollRecord.BranchId = request.BranchId;
            PayRollRecord.BaseSalary = request.BaseSalary;
            PayRollRecord.AdvanceDeduction = request.AdvanceDeduction;
            PayRollRecord.NetSalary = request.NetSalary;
            PayRollRecord.PayedBy = _loggedInUser.Id;
            PayRollRecord.IsPayed = request.IsPayed;
            PayRollRecord.ModifiedOn = DateTime.Now;
            PayRollRecord.ModifiedBy = _loggedInUser.Id;


            _genericRepositoryAsync.EditeAsync(PayRollRecord, cancellationToken);
            #endregion

            if (PayRollRecord.IsPayed)
            {
                #region Update Main Asset Record
                mainAccount.BalanceAmount -= PayRollRecord.NetSalary;
                mainAccount.TotalCreditAmount += PayRollRecord.NetSalary;
                mainAccount.ModifiedOn = DateTime.Now;
                mainAccount.ModifiedBy = _loggedInUser.Id;
                _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
                #endregion

                #region Add Asset Tracking Record
                AccountTracking AccountTracking = new()
                {
                    CurrencyTypeId = mainAccount.CurrencyTypeId,
                    TransactionDate = DateTime.Now,
                    Description = $"{nameof(PayrollTracking)}:",
                    UserId = _loggedInUser.Id,
                    DebitAmount = 0,
                    CreditAmount = PayRollRecord.NetSalary,
                    BalanceAmount = mainAccount.BalanceAmount,
                    MainAccountId = mainAccount.ID,
                    trackType = TrackType.PAYROLL,
                    CreatedBy = _loggedInUser.Id,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                return await _GRepoAccountTracking.AddAsync(AccountTracking, cancellationToken);
                #endregion
            }

            return _message.Saved();
        }
    }
}
