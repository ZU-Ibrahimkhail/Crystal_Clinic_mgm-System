using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.AssetMS.MainAccounts.Queries.GetChildDDl;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.AssetMS.Dashboards
{
    #region Request

    public class RealTimeDashboardQuery : IRequest<JsonResult>
    {
    }
    #endregion

    #region Handler
    public class RealTimeDashboardHandler(
        IGenericRepositoryAsync<ERP_DbContext, MainAccount> _GRepoMainAccountTracking,
        IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> _GRepoExpenseTracking,
        IGenericRepositoryAsync<ERP_DbContext, TradeTracking> _GRepoTradeTracking,
        ILoggedInUser _loggedInUser,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<RealTimeDashboardQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(RealTimeDashboardQuery request, CancellationToken cancellationToken)
        {

            Localization localize = new(httpContextAccessor);

            var today = DateTime.Today.Date;

            var TotalMainAccountsCurrentBalance = await _GRepoMainAccountTracking.FindByCondition(x => !x.IsDeleted && x.OwnerUserId == _loggedInUser.Id)
                .Include(x => x.CurrencyType)
                .GroupBy(x => x.CurrencyTypeId)
                .Select(x => new { CurrencyType = localize.GetName(x.First().CurrencyType), Value = Convert.ToDouble(x.Sum(g => g.BalanceAmount)) }).ToListAsync(cancellationToken);

            var TodayTotalExpense = await _GRepoExpenseTracking.FindByCondition(x => !x.IsDeleted && x.Date.Date == today && x.UserId == _loggedInUser.Id)
                .Include(x => x.CurrencyType)
                .GroupBy(x => x.CurrencyTypeId)
                .Select(x => new { CurrencyType = localize.GetName(x.First().CurrencyType), Value = x.Sum(g => g.Amount) }).ToListAsync();

            var TotalTodaysTrade = await _GRepoTradeTracking.FindByCondition(x => !x.IsDeleted && x.Date.Date == today && x.UserId == _loggedInUser.Id)
                 .Include(x => x.CurrencyType)
                 .GroupBy(x => x.CurrencyTypeId)
                 .Select(x => new
                 {
                     CurrencyType = localize.GetName(x.First().CurrencyType),
                     TradeAmount = x.Sum(g => g.TradeAmount),
                     ProfitAmount = x.Sum(g => g.ProfitAmount),
                     LossAmount = x.Sum(g => g.LossAmount)
                 }).ToListAsync(cancellationToken);
            return new JsonResult(new { TotalMainAccountsCurrentBalance, TodayTotalExpense, TotalTodaysTrade });
        }
    }
    #endregion
}
