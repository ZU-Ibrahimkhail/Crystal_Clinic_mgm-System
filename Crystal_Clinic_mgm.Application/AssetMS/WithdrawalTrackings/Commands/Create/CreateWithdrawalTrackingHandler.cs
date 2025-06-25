using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.ComponentModel;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Create
{
    public class CreateWithdrawalTrackingHandler(
        IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> GRepoWithdrawalTracking,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> GRepoAccountTracking,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message,
        ILoggedInUser loggedInUser,
        IStringLocalizer<CommonColumnNameResource> ColumnLocalizer,
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> GRepoMainAccount) : IRequestHandler<CreateWithdrawalTrackingCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> _GRepoWithdrawalTracking = GRepoWithdrawalTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = GRepoAccountTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = GRepoMainAccount;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = ColumnLocalizer;

        public async Task<JsonResult> Handle(CreateWithdrawalTrackingCommand request, CancellationToken cancellationToken)
        {
            #region Check Validations
            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == request.MainAccountId).FirstOrDefault();
            if (mainAccount == null) { return _message.RecordNotFound(); }

            var validator = new CreateWithdrawalTrackingValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (request.WithdrawalAmount > mainAccount.BalanceAmount)
            {
                validator.Add(new() { PropertyName = nameof(request.WithdrawalAmount), ErrorMessage = $"{_ColumnLocalizer[nameof(request.WithdrawalAmount)]} {_Localizer["IsGreaterThen"]} {_ColumnLocalizer[nameof(mainAccount.BalanceAmount)]} :  {mainAccount.BalanceAmount}" });
            }
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #endregion

            #region Add Withdrawal Tracking Record
            var entity = new WithdrawalTracking
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                MainAccountId = request.MainAccountId,
                WithdrawalAmount = request.WithdrawalAmount,
                DepositAmount = 0,
                Date = request.Date,
                Description = request.Description,
                BranchId = _loggedInUser.BranchId,
                UserId = _loggedInUser.Id,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
            };
            _GRepoWithdrawalTracking.SaveAsync(entity, cancellationToken);
            #endregion

            #region Update Main Asset Record
            mainAccount.TotalDebitAmount += request.WithdrawalAmount;
            mainAccount.BalanceAmount -= request.WithdrawalAmount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(WithdrawalTracking)}: {request.Description}",
                UserId = _loggedInUser.Id,
                DebitAmount = request.WithdrawalAmount,
                CreditAmount = 0,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.WITHDRAW,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            return await _GRepoAccountTracking.AddAsync(AccountTracking, cancellationToken);
            #endregion
        }
    }
}
