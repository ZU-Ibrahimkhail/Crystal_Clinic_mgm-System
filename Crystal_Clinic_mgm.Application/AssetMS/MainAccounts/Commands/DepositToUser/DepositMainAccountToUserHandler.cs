using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.DepositToUser;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.DepositMainAccountToUser
{
    public class DepositMainAccountToUserHandler(
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> GRepoMainAccount,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> GRepoAccountTracking,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message,
        ILoggedInUser loggedInUser,
        IStringLocalizer<CommonColumnNameResource> ColumnLocalizer) : IRequestHandler<DepositMainAccountToUserCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = GRepoMainAccount;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = GRepoAccountTracking;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = ColumnLocalizer;

        public async Task<JsonResult> Handle(DepositMainAccountToUserCommand request, CancellationToken cancellationToken)
        {
            #region Check Validations
            var Parent = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == request.ParentId).FirstOrDefault();
            var validator = new DepositMainAccountToUserValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            MainAccount? entity = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.BranchId == request.BranchId && (x.ID == request.MainAccountId || (x.OwnerUserId == request.ToUserId && x.ParentId == request.ParentId))).FirstOrDefault();
            if (Parent == null)
            {
                return _message.RecordNotFound(nameof(request.MainAccountId));
            }
            if (entity != null && Parent.CurrencyTypeId != entity.CurrencyTypeId)
            {
                validator.Add(new() { PropertyName = nameof(request.MainAccountId), ErrorMessage = $"{_ColumnLocalizer[nameof(MainAccount.CurrencyTypeId)]}  {_ColumnLocalizer[nameof(Parent.CurrencyTypeId)]} {_Localizer["DoesNotMatch"]}  {entity.CurrencyTypeId}" });
            }
            if (request.DepositAmmount > Parent.BalanceAmount)
            {
                validator.Add(new() { PropertyName = nameof(request.DepositAmmount), ErrorMessage = $"{_ColumnLocalizer[nameof(request.DepositAmmount)]} {_Localizer["IsGreaterThen"]} {_ColumnLocalizer[nameof(Parent.BalanceAmount)]} :  {Parent.BalanceAmount}" });
            }
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            #endregion

            #region Update Parent's Main Asset Record
            Parent.BalanceAmount -= request.DepositAmmount;
            Parent.TotalDebitAmount += request.DepositAmmount;
            Parent.ModifiedBy = _loggedInUser.Id;
            Parent.ModifiedOn = DateTime.Now;
            _GRepoMainAccount.EditeAsync(Parent, cancellationToken);
            #endregion

            #region Adding New Main Asset For Child

            if (entity != null)
            {
                entity.BalanceAmount += request.DepositAmmount;
                entity.TotalCreditAmount += request.DepositAmmount;
                entity.ModifiedOn = DateTime.Now;
                entity.ModifiedBy = _loggedInUser.Id;
                _GRepoMainAccount.EditeAsync(entity, cancellationToken);
            }
            else
            {
                entity = new MainAccount
                {
                    CurrencyTypeId = Parent.CurrencyTypeId,
                    ParentId = Parent.ID,
                    DepositDate = request.DepositDate,
                    Description = request.Description,
                    BranchId = request.BranchId,
                    OwnerUserId = request.ToUserId,
                    Code = $"{DateTime.Now.Date.ToShortDateString()}_{Random.Shared.Next(1111, 9999)}",
                    BalanceAmount = request.DepositAmmount,
                    TotalDebitAmount = request.DepositAmmount,
                    CreatedBy = _loggedInUser.Id,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                };
                _GRepoMainAccount.SaveAsync(entity, cancellationToken);
            }

            #endregion

            #region Adding Asset Tracking Record For Parent 
            AccountTracking ParentAccountTracking = new()
            {
                CurrencyTypeId = Parent.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(MainAccount)}: {entity.Description}",
                UserId = Parent.OwnerUserId,
                DebitAmount = request.DepositAmmount,
                CreditAmount = 0,
                BalanceAmount = Parent.BalanceAmount,
                MainAccountId = Parent.ID,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            _GRepoAccountTracking.SaveAsync(ParentAccountTracking, cancellationToken);
            #endregion

            #region Adding Asset Tracking Record For Child
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = entity.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(MainAccount)}: {entity.Description}",
                UserId = entity.OwnerUserId,
                DebitAmount = 0,
                CreditAmount = entity.BalanceAmount,
                BalanceAmount = entity.BalanceAmount,
                MainAccountId = entity.ID,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            return await _GRepoAccountTracking.AddAsync(AccountTracking, cancellationToken);
            #endregion

        }
    }
}
