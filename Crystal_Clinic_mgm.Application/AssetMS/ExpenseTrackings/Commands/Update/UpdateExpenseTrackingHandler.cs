using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Update
{
    public class UpdateExpenseTrackingHandler(
        IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> gRepoExpenseTracking,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> gRepoAccountTracking,
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> gRepoMainAccount,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message,
        ILoggedInUser loggedInUser,
        IStringLocalizer<CommonColumnNameResource> columnLocalizer) : IRequestHandler<UpdateExpenseTrackingCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> _GRepoExpenseTracking = gRepoExpenseTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = gRepoAccountTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = gRepoMainAccount;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = columnLocalizer;

        public async Task<JsonResult> Handle(UpdateExpenseTrackingCommand request, CancellationToken cancellationToken)
        {
            #region Check Validations
            var entity = await _GRepoExpenseTracking.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted) { return _message.RecordNotFound(); }
            var oldAmount = entity.Amount;
            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == entity.MainAccountId).FirstOrDefault();

            if (mainAccount == null) { return _message.RecordNotFound(); }

            mainAccount.TotalDebitAmount -= entity.Amount;
            mainAccount.BalanceAmount += entity.Amount;

            var validator = new UpdateExpenseTrackingValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (request.Amount > mainAccount.BalanceAmount)
            {
                validator.Add(new()
                {
                    PropertyName = nameof(request.Amount),
                    ErrorMessage = $"{_ColumnLocalizer[nameof(request.Amount)]} {_Localizer["IsGreaterThen"]} {_ColumnLocalizer[nameof(mainAccount.BalanceAmount)]} :  {mainAccount.BalanceAmount}"
                });
            }
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #endregion

            #region Update Expense Tracking Record
            entity.CurrencyTypeId = mainAccount.CurrencyTypeId;
            entity.ExpenseTypeId = request.ExpenseTypeId;
            entity.Date = request.Date;
            entity.Description = request.Description;
            entity.BranchId = _loggedInUser.BranchId;
            entity.UserId = _loggedInUser.Id;
            entity.Amount = request.Amount;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.InvoiceNumber = request.InvoiceNumber;
            entity.AttachmentPath = request.Attachment;
            entity.ModifiedOn = DateTime.Now;
            _GRepoExpenseTracking.EditeAsync(entity, cancellationToken);
            #endregion

            #region Update Main Asset Record
            mainAccount.TotalDebitAmount += entity.Amount;
            mainAccount.BalanceAmount -= entity.Amount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(ExpenseTracking)}_Update: {request.Description}",
                UserId = entity.UserId,
                DebitAmount = entity.Amount,
                CreditAmount = oldAmount,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.EXPENSE,
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
