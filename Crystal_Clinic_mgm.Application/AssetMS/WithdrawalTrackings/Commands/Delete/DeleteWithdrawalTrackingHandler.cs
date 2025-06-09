using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.WithdrawalTrackings.Commands.Delete
{
    public class DeleteWithdrawalTrackingHandler(
        IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> GRepoWithdrawalTracking,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> GRepoAccountTracking,
        IMessage message,
        ILoggedInUser loggedInUser,
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> GRepoMainAccount) : IRequestHandler<DeleteWithdrawalTrackingCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, WithdrawalTracking> _GRepoWithdrawalTracking = GRepoWithdrawalTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = GRepoAccountTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = GRepoMainAccount;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        public async Task<JsonResult> Handle(DeleteWithdrawalTrackingCommand request, CancellationToken cancellationToken)
        {


            var entity = await _GRepoWithdrawalTracking.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }

            entity.IsDeleted = true;
            entity.Remarks = request.Remarks;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == entity.MainAccountId).FirstOrDefault();
            if (mainAccount == null) { return _message.RecordNotFound(); }

            #region Update Main Asset Record
            mainAccount.TotalDebitAmount -= entity.WithdrawalAmount;
            mainAccount.TotalCreditAmount -= entity.DepositAmount;
            mainAccount.BalanceAmount += entity.WithdrawalAmount;
            mainAccount.BalanceAmount -= entity.DepositAmount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(WithdrawalTracking)}_Delete: {request.Remarks}",
                UserId = entity.UserId,
                CreditAmount = entity.WithdrawalAmount,
                DebitAmount = entity.DepositAmount,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.WITHDRAW,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            _GRepoAccountTracking.SaveAsync(AccountTracking, cancellationToken);
            #endregion

            return await _GRepoWithdrawalTracking.DeleteAsync(entity, cancellationToken);

        }
    }
}
