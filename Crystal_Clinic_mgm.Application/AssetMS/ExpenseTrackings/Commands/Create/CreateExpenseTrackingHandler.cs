using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.ComponentModel;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Common.Storage;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Create
{
    public class CreateExpenseTrackingHandler(
        IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> GRepoExpenseTracking,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> GRepoAccountTracking,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message,
        ILoggedInUser loggedInUser,
        IStringLocalizer<CommonColumnNameResource> ColumnLocalizer,
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> GRepoMainAccount) : IRequestHandler<CreateExpenseTrackingCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> _GRepoExpenseTracking = GRepoExpenseTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = GRepoAccountTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = GRepoMainAccount;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = ColumnLocalizer;

        public async Task<JsonResult> Handle(CreateExpenseTrackingCommand request, CancellationToken cancellationToken)
        {
            #region Check Validations
            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == request.MainAccountId).FirstOrDefault();
            if (mainAccount == null) { return _message.RecordNotFound(); }

            var validator = new CreateExpenseTrackingValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (request.Amount > mainAccount.BalanceAmount)
            {
                validator.Add(new() { PropertyName = nameof(request.Amount), ErrorMessage = $"{_ColumnLocalizer[nameof(request.Amount)]} {_Localizer["IsGreaterThen"]} {_ColumnLocalizer[nameof(mainAccount.BalanceAmount)]} :  {mainAccount.BalanceAmount}" });
            }
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #endregion

            #region Add Expense Tracking Record
            
            string FilePath = "";
            if (request.Attachment != null)
            {
                var attachment = request.Attachment;
                FileHandler _sotrage = new();
                if (attachment.FileName.Length > 0)
                {
                    string ext = Path.GetExtension(attachment.FileName);
                    FilePath = await _sotrage.CreateAsync(attachment.OpenReadStream(), ext, "wwwroot", AppConfig.Archive_ArchivedDocuments);
                }
            }
            var entity = new ExpenseTracking
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                ExpenseTypeId = request.ExpenseTypeId,
                MainAccountId = request.MainAccountId,
                Amount = request.Amount,
                Date = request.Date,
                Description = request.Description,
                InvoiceNumber = request.InvoiceNumber,
                BranchId = _loggedInUser.BranchId,
                AttachmentPath = FilePath,
                UserId = _loggedInUser.Id,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
            };
            _GRepoExpenseTracking.SaveAsync(entity, cancellationToken);
            #endregion

            #region Update Main Asset Record
            mainAccount.TotalDebitAmount += request.Amount;
            mainAccount.BalanceAmount -= request.Amount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(ExpenseTracking)}: {request.Description}",
                UserId = _loggedInUser.Id,
                DebitAmount = request.Amount,
                CreditAmount = 0,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.EXPENSE,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            return await _GRepoAccountTracking.AddAsync(AccountTracking, cancellationToken);
            #endregion
        }
    }
}
