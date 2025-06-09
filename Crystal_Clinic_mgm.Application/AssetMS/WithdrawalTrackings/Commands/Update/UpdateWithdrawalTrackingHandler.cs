using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Update
{
    public class UpdateWithdrawalTrackingHandler(
        IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> gRepoWithdrawalTracking,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> gRepoAccountTracking,
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> gRepoMainAccount,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message,
        ILoggedInUser loggedInUser,
        IStringLocalizer<CommonColumnNameResource> columnLocalizer) : IRequestHandler<UpdateWithdrawalTrackingCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> _GRepoWithdrawalTracking = gRepoWithdrawalTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = gRepoAccountTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = gRepoMainAccount;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = columnLocalizer;

        public async Task<JsonResult> Handle(UpdateWithdrawalTrackingCommand request, CancellationToken cancellationToken)
        {
            #region Check Validations
            var entity = await _GRepoWithdrawalTracking.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted) { return _message.RecordNotFound(); }

            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == entity.MainAccountId).FirstOrDefault();
            if (mainAccount == null) { return _message.RecordNotFound(); }
            mainAccount.BalanceAmount += entity.WithdrawalAmount;
            mainAccount.TotalCreditAmount -= entity.WithdrawalAmount;


            var validator = new UpdateWithdrawalTrackingValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (request.WithdrawalAmount > mainAccount.BalanceAmount)
            {
                validator.Add(new()
                {
                    PropertyName = nameof(request.WithdrawalAmount),
                    ErrorMessage = $"{_ColumnLocalizer[nameof(request.WithdrawalAmount)]} {_Localizer["IsGreaterThen"]} {_ColumnLocalizer[nameof(mainAccount.BalanceAmount)]} :  {mainAccount.BalanceAmount}"
                });
            }
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #endregion
            var lastWithdraw = entity.WithdrawalAmount;
            #region Update Withdrawal Tracking Record
            entity.CurrencyTypeId = mainAccount.CurrencyTypeId;
            entity.Date = request.Date;
            entity.Description = request.Description;
            entity.WithdrawalAmount = request.WithdrawalAmount;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;
            _GRepoWithdrawalTracking.EditeAsync(entity, cancellationToken);
            #endregion

            #region Update Main Asset Record
            mainAccount.TotalCreditAmount += lastWithdraw - entity.WithdrawalAmount;
            mainAccount.BalanceAmount += lastWithdraw - entity.WithdrawalAmount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(WithdrawalTracking)}_Update: {request.Description}",
                UserId = entity.UserId,
                DebitAmount = lastWithdraw,
                CreditAmount = entity.WithdrawalAmount,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.WITHDRAW,
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
