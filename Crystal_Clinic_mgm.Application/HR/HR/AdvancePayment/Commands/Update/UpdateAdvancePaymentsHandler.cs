using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Security.Cryptography.Xml;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Update
{
    public class UpdateAdvancePaymentHandler(IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> gRepoAdvancePayment, IGenericRepositoryAsync<ERP_DbContext, MainAccount> gRepoMainAccount, IGenericRepositoryAsync<ERP_DbContext, AccountTracking> gRepoAccountTracking, IStringLocalizer<CommonValidationResource> localizer, IStringLocalizer<CommonColumnNameResource> columnLocalizer, IMessage message, ILoggedInUser loggedInUser) : IRequestHandler<UpdateAdvancePaymentCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> _GRepoAdvancePayment = gRepoAdvancePayment;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = gRepoMainAccount;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = gRepoAccountTracking;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = columnLocalizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;

        public async Task<JsonResult> Handle(UpdateAdvancePaymentCommand request, CancellationToken cancellationToken)
        {

            #region Check Validations
            var entity = await _GRepoAdvancePayment.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted) { return _message.RecordNotFound(); }
            var oldAdvanceAmount = entity.AdvanceAmount;
            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == entity.MainAccountId).FirstOrDefault();
            if (mainAccount == null) { return _message.RecordNotFound(); }
            mainAccount.BalanceAmount += entity.AdvanceAmount;
            mainAccount.TotalCreditAmount -= entity.AdvanceAmount;

            var validator = new UpdateAdvancePaymentValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (request.AdvanceAmount > mainAccount.BalanceAmount)
            {
                validator.Add(new()
                {
                    PropertyName = nameof(request.AdvanceAmount),
                    ErrorMessage = $"{_ColumnLocalizer[nameof(request.AdvanceAmount)]} {_Localizer["IsGreaterThen"]} {_ColumnLocalizer[nameof(mainAccount.BalanceAmount)]} :  {mainAccount.BalanceAmount}"
                });
            }
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #endregion

            entity.EmployeeId = request.EmployeeId;
            entity.CurrencyTypeId = mainAccount.CurrencyTypeId;
            entity.PayTypeId = request.PayTypeId;
            entity.AdvanceAmount = request.AdvanceAmount;
            entity.EachInstallmentAmount = request.EachInstallmentAmount;
            entity.AdvanceDate = request.AdvanceDate;
            entity.Remarks = request.Remarks ?? entity.Remarks;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            _GRepoAdvancePayment.EditeAsync(entity, cancellationToken);


            #region Update Main Asset Record
            mainAccount.BalanceAmount -= entity.AdvanceAmount;
            mainAccount.TotalCreditAmount += entity.AdvanceAmount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(AdvancePayment)}_Update: {request.Remarks}",
                UserId = _loggedInUser.Id,
                DebitAmount = oldAdvanceAmount,
                CreditAmount = entity.AdvanceAmount,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.PAYROLL,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            _GRepoAccountTracking.SaveAsync(AccountTracking, cancellationToken);
            #endregion
            return _message.Update();

        }
    }
}
