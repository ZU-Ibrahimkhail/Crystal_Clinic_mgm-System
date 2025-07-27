using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Crystal_Clinic_Mgm.Application.AssetMS.Reports
{
    #region Request

    public class ExpenseReportQuery : IRequest<JsonResult>
    {
        public List<int> BranchIds { get; set; } = [];
        public List<int> ExpenseTypeIds { get; set; } = [];
        public DateTime FromDate { get; set; } = DateTime.Now.AddDays(-30);
        public DateTime ToDate { get; set; } = DateTime.Now;
    }
    #endregion

    #region Handler
    public class ExpenseReportHandler(IGenericRepositoryAsync<ERP_DbContext, ExpenseTracking> _GRepoExpenseTracking, IHttpContextAccessor httpContextAccessor, IGeneralHelperRepositoryAsync _helper) : IRequestHandler<ExpenseReportQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(ExpenseReportQuery request, CancellationToken cancellationToken)
        {
            Localization localize = new(httpContextAccessor);
            var language = localize.GetMyCookieValue(httpContextAccessor);
            var Data = _GRepoExpenseTracking.FindByCondition(x => !x.IsDeleted &&
                x.Date.Date >= request.FromDate.Date && x.Date.Date <= request.ToDate.Date
                &&
                (request.BranchIds.Count == 0 || request.BranchIds.Contains(x.BranchId))
                &&
                (request.ExpenseTypeIds.Count == 0 || request.ExpenseTypeIds.Contains(x.ExpenseTypeId))
            ).Include(x => x.Branch).Include(x => x.CurrencyType).Include(x => x.ExpenseType).Include(x => x.MainAccount);

            var report = await Data
                 .GroupBy(x => x.BranchId)
                 .Select(trk => ExpenseReportViewModel.Projection
                 .Compile()
                 .Invoke(trk.ToList(), localize)
                  ).ToListAsync(cancellationToken);

            var Transactions = await Data.Select(x => new ExpenseTransactionModel(x, localize, _helper, language)).ToListAsync(cancellationToken);
            return new JsonResult(new { report, Transactions });
        }
    }
    #endregion

    #region Models
    public class ExpenseReportViewModel
    {

        public string BranchName { get; set; } = string.Empty;
        public List<ExpenseTypeModel> ExpenseTypes { get; set; } = new();

        public static Expression<Func<List<ExpenseTracking>, Localization, ExpenseReportViewModel>> Projection
        {

            get
            {

                return (tr, Localize) => new ExpenseReportViewModel
                {

                    BranchName = Localize.GetName(tr.First().Branch),
                    ExpenseTypes = tr.GroupBy(g => g.ExpenseTypeId).Select(d => new ExpenseTypeModel()
                    {
                        ExpenseType = Localize.GetName(d.First().ExpenseType),
                        Dollor = d.Where(g => g.CurrencyTypeId == 1).Sum(x => x.Amount),
                        Afghani = d.Where(g => g.CurrencyTypeId == 2).Sum(x => x.Amount),
                        Lira = d.Where(g => g.CurrencyTypeId == 3).Sum(x => x.Amount)
                    }).ToList(),

                };
            }
        }


    }

    public class ExpenseTypeModel
    {
        public string ExpenseType { get; set; } = string.Empty;
        public double Dollor { get; set; }
        public double Afghani { get; set; }
        public double Lira { get; set; }
    }

    public class ExpenseTransactionModel(ExpenseTracking expenseTracking, Localization localize, IGeneralHelperRepositoryAsync _helper, string language)
    {
        public string? Branch { get; set; } = localize.GetName(expenseTracking.Branch);
        public string? MainAccountCode { get; set; } = expenseTracking.MainAccount?.Code;
        public string? CurrencyType { get; set; } = localize.GetName(expenseTracking.CurrencyType);
        public string? ExpenseType { get; set; } = localize.GetName(expenseTracking.ExpenseType);
        public double Amount { get; set; } = expenseTracking.Amount;
        public DateTime Date { get; set; } = expenseTracking.Date;
        public string Description { get; set; } = expenseTracking.Description;
        public string? UserName { get; set; } = _helper.GetUserName(language, expenseTracking.UserId);
    }

    #endregion
}
