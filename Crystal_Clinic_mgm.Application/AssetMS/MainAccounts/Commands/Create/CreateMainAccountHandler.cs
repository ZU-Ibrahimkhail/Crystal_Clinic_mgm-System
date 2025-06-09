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

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Create
{
    public class CreateMainAccountHandler(
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> GRepoMainAccount,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> GRepoAccountTracking,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message,
        ILoggedInUser loggedInUser,
        IStringLocalizer<CommonColumnNameResource> ColumnLocalizer) : IRequestHandler<CreateMainAccountCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = GRepoMainAccount;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = GRepoAccountTracking;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = ColumnLocalizer;

        public async Task<JsonResult> Handle(CreateMainAccountCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateMainAccountValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            var entity = new MainAccount
            {
                CurrencyTypeId = request.CurrencyTypeId,
                DepositDate = request.DepositDate,
                Description = request.Description,
                BranchId = request.BranchId,
                OwnerUserId = request.OwnerUserId,
                Code = $"{DateTime.Now.Date.ToShortDateString()}_{Random.Shared.Next(1111, 9999)}",
                TotalCreditAmount = request.BalanceAmount,
                BalanceAmount = request.BalanceAmount,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
            };

            _GRepoMainAccount.SaveAsync(entity, cancellationToken);

            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = request.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(MainAccount)}: {request.Description}",
                UserId = request.OwnerUserId,
                DebitAmount = 0,
                CreditAmount = request.BalanceAmount,
                BalanceAmount = entity.BalanceAmount,
                MainAccountId = entity.ID,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            return await _GRepoAccountTracking.AddAsync(AccountTracking, cancellationToken);

        }
    }
}
