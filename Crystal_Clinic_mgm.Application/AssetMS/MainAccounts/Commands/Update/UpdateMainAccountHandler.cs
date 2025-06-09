using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;

namespace Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Commands.Update
{
    public class UpdateMainAccountHandler(
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> genericRepositoryAsync,
        IStringLocalizer<CommonValidationResource> localizer,
        IStringLocalizer<CommonColumnNameResource> ColumnLocalizer,
        IMessage message,
        ILoggedInUser loggedInUser) : IRequestHandler<UpdateMainAccountCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _genericRepositoryAsync = genericRepositoryAsync;
        private readonly IStringLocalizer<CommonValidationResource> _Localizer = localizer;
        private readonly IStringLocalizer<CommonColumnNameResource> _ColumnLocalizer = ColumnLocalizer;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;

        public async Task<JsonResult> Handle(UpdateMainAccountCommand request, CancellationToken cancellationToken)
        {

            var validator = new UpdateMainAccountValidator(_Localizer, _ColumnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return _message.CheckCCValidationError(validator);
            }
            var entity = await _genericRepositoryAsync.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }

            entity.CurrencyTypeId = request.CurrencyTypeId;
            entity.DepositDate = request.DepositDate;
            entity.Description = request.Description;
            entity.BranchId = request.BranchId;
            entity.OwnerUserId = request.OwnerUserId;
            entity.TotalCreditAmount -= request.BalanceAmount == null? entity.TotalCreditAmount : entity.BalanceAmount;
            entity.TotalCreditAmount += request.BalanceAmount ?? entity.TotalCreditAmount;
            entity.BalanceAmount = request.BalanceAmount ?? entity.BalanceAmount;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            return await _genericRepositoryAsync.UpdateAsync(entity, cancellationToken);

        }
    }
}
