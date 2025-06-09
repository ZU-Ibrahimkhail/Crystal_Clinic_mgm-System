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
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Create;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Create
{
    public class CreateAdvancePaymentHandler(IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> gRepoAdvancePayment, IGenericRepositoryAsync<ERP_DbContext, MainAccount> gRepoMainAccount, IGenericRepositoryAsync<ERP_DbContext, AccountTracking> gRepoAccountTracking, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer, IMessage message, ILoggedInUser loggedInUser) : IRequestHandler<CreateAdvancePaymentCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> _GRepoAdvancePayment = gRepoAdvancePayment;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = gRepoMainAccount;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = gRepoAccountTracking;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = columnLocalizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;

        public async Task<JsonResult> Handle(CreateAdvancePaymentCommand request, CancellationToken cancellationToken)
        {


            #region Check Validations
            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == request.MainAccountId).FirstOrDefault();
            if (mainAccount == null) { return _message.RecordNotFound(); }

            var validator = new CreateAdvancePaymentValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (request.AdvanceAmount > mainAccount.BalanceAmount)
            {
                validator.Add(new() { PropertyName = nameof(request.AdvanceAmount), ErrorMessage = $"{_ColumnLocalizer[nameof(request.AdvanceAmount)]} {_Localizer["IsGreaterThen"]} {_ColumnLocalizer[nameof(mainAccount.BalanceAmount)]} :  {mainAccount.BalanceAmount}" });
            }
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #endregion



            var entity = new AdvancePayment
            {
                MainAccountId = request.MainAccountId,
                EmployeeId = request.EmployeeId,
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                PayTypeId = request.PayTypeId,
                AdvanceAmount = request.AdvanceAmount,
                EachInstallmentAmount = request.EachInstallmentAmount,
                AdvanceDate = request.AdvanceDate,
                Remarks = request.Remarks,
                PayedBy = _loggedInUser.Id,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
            };
            _GRepoAdvancePayment.SaveAsync(entity, cancellationToken);

            #region Update Main Asset Record
            mainAccount.BalanceAmount -= request.AdvanceAmount;
            mainAccount.TotalCreditAmount += request.AdvanceAmount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(AdvancePayment)}: {request.Remarks}",
                UserId = _loggedInUser.Id,
                DebitAmount = 0,
                CreditAmount = request.AdvanceAmount,
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
    }
}
