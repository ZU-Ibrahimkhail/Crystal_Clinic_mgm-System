using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.AssetMS.ExpenseTrackings.Commands.Delete
{
    public class DeleteExpenseTrackingHandler(
        IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> GRepoExpenseTracking,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> GRepoAccountTracking,
        IMessage message,
        ILoggedInUser loggedInUser,
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> GRepoMainAccount) : IRequestHandler<DeleteExpenseTrackingCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> _GRepoExpenseTracking = GRepoExpenseTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = GRepoAccountTracking;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = GRepoMainAccount;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        public async Task<JsonResult> Handle(DeleteExpenseTrackingCommand request, CancellationToken cancellationToken)
        {


            var entity = await _GRepoExpenseTracking.GetDetailAsync(request.ID);
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
            mainAccount.TotalDebitAmount -= entity.Amount;
            mainAccount.BalanceAmount += entity.Amount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(ExpenseTracking)}_Delete: {request.Remarks}",
                UserId = entity.UserId,
                DebitAmount = 0,
                CreditAmount = entity.Amount,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.EXPENSE,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

             _GRepoAccountTracking.SaveAsync(AccountTracking, cancellationToken);
            #endregion

            return await _GRepoExpenseTracking.DeleteAsync(entity, cancellationToken);

        }
    }
}
