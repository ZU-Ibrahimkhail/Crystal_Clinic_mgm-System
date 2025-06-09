using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.AdvancePayments.Commands.Delete
{
    public class DeleteAdvancePaymentHandler(
        IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> genericRepositoryAsync,
        IMessage message,
        ILoggedInUser loggedInUser,
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> gRepoMainAccount,
        IGenericRepositoryAsync<ERP_DbContext, AccountTracking> gRepoAccountTracking) : IRequestHandler<DeleteAdvancePaymentCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, AdvancePayment> _genericRepositoryAsync = genericRepositoryAsync;
        private readonly IMessage _message = message;
        private readonly ILoggedInUser _loggedInUser = loggedInUser;
        private readonly IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccount = gRepoMainAccount;
        private readonly IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoAccountTracking = gRepoAccountTracking;

        public async Task<JsonResult> Handle(DeleteAdvancePaymentCommand request, CancellationToken cancellationToken)
        {


            var entity = await _genericRepositoryAsync.GetDetailAsync(request.ID);
            if (entity == null || entity.IsDeleted)
            {
                return _message.RecordNotFound(request.ID);
            }
            var mainAccount = _GRepoMainAccount.FindByCondition(x => !x.IsDeleted && x.ID == entity.MainAccountId).FirstOrDefault();
            if (mainAccount == null) { return _message.RecordNotFound(); }


            entity.IsDeleted = true;
            entity.Remarks = request.Remarks ?? entity.Remarks;
            entity.ModifiedBy = _loggedInUser.Id;
            entity.ModifiedOn = DateTime.Now;

            #region Update Main Asset Record
            mainAccount.BalanceAmount += entity.AdvanceAmount;
            mainAccount.TotalCreditAmount -= entity.AdvanceAmount;
            mainAccount.ModifiedOn = DateTime.Now;
            mainAccount.ModifiedBy = _loggedInUser.Id;
            _GRepoMainAccount.EditeAsync(mainAccount, cancellationToken);
            #endregion

            #region Add Asset Tracking Record
            AccountTracking AccountTracking = new()
            {
                CurrencyTypeId = mainAccount.CurrencyTypeId,
                TransactionDate = DateTime.Now,
                Description = $"{nameof(AdvancePayment)}_Delete: {request.Remarks}",
                UserId = _loggedInUser.Id,
                DebitAmount = entity.AdvanceAmount,
                CreditAmount = 0,
                BalanceAmount = mainAccount.BalanceAmount,
                MainAccountId = mainAccount.ID,
                trackType = TrackType.PAYROLL,
                CreatedBy = _loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            _GRepoAccountTracking.SaveAsync(AccountTracking, cancellationToken);
            #endregion
            return await _genericRepositoryAsync.DeleteAsync(entity, cancellationToken);

        }
    }
}
