using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Linq.Expressions;
using static Crystal_Clinic_Mgm.Application.AssetMS.Reports.Report;
using static Crystal_Clinic_Mgm.Application.AssetMS.Reports.Report.CurrencyBaseReport;

namespace Crystal_Clinic_Mgm.Application.AssetMS.Reports
{
    #region Request

    public class TransactionReportQuery : IRequest<JsonResult>
    {
        public List<Guid> MainAccountIds { get; set; } = [];
        public DateTime FromDate { get; set; } = DateTime.Now.AddDays(-30);
        public DateTime ToDate { get; set; } = DateTime.Now;
    }
    #endregion

    #region Handler
    public class TransactionReportHandler(IGenericRepositoryAsync<ERP_DbContext, AccountTracking> _GRepoTransactionTracking, IHttpContextAccessor httpContextAccessor, IGeneralHelperRepositoryAsync _helper) : IRequestHandler<TransactionReportQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(TransactionReportQuery request, CancellationToken cancellationToken)
        {
            Localization localize = new(httpContextAccessor);
            var language = localize.GetMyCookieValue(httpContextAccessor);
            var Data = _GRepoTransactionTracking.FindByCondition(x => !x.IsDeleted &&
                x.TransactionDate.Date >= request.FromDate.Date && x.TransactionDate.Date <= request.ToDate.Date
                &&
                (request.MainAccountIds.Count == 0 || request.MainAccountIds.Contains(x.MainAccountId)))
            .Include(x => x.MainAccount)
            .Include(x => x.MainAccount!.Branch)
            .Include(x => x.CurrencyType);

            var report = Data.GroupBy(c => c.CurrencyTypeId)
                                                          .Select(c => new CurrencyBaseReport
                                                          {
                                                              CurrencyType = localize.GetName(c.First().CurrencyType),
                                                              TotalReports = c.GroupBy(g => g.MainAccountId)
                                                                                .Select(x => new AssetTotal
                                                                                {
                                                                                    MainAccountCode = x.First().MainAccount!.Code,
                                                                                    StartAmount = x.First().BalanceAmount,
                                                                                    CurrentAmount = x.OrderByDescending(x=>x.ID).First().BalanceAmount,
                                                                                    Debit =Convert.ToDouble(x.Sum(s => s.DebitAmount)),
                                                                                    Credit = Convert.ToDouble(x.Sum(s => s.CreditAmount)),

                                                                                }).ToList()
                                                          }).ToList();



            var TransactionReport = await Data
                 .GroupBy(x => x.MainAccountId)
                 .Select(trk => new TransactionReportViewModel()
                 {
                     MainAccountCode = trk.First().MainAccount!.Code,
                     TransactionModel = trk.Select(x => new TransactionModel(x, localize, _helper, language)).ToList(),
                 }).ToListAsync(cancellationToken);

            return new JsonResult(new { TransactionReport, report });
        }
    }
    #endregion

    #region Models
    public class Report
    {
        public List<CurrencyBaseReport> CurrencyBaseReports { get; set; } = [];
        public class CurrencyBaseReport
        {
            public string? CurrencyType { get; set; }
            public List<AssetTotal> TotalReports { get; set; } = [];
            public class AssetTotal
            {
                public string? MainAccountCode { get; set; }
                public double StartAmount { get; set; }
                public double CurrentAmount { get; set; }
                public double Debit { get; set; }
                public double Credit { get; set; }
            }
        }


    }


    public class CurrencyTotals
    {
        public decimal TotalStartAmount { get; set; }
        public decimal TotalCurrentAmount { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
    }


    public class TransactionReportViewModel
    {

        public string? MainAccountCode { get; set; } = string.Empty;
        public List<TransactionModel> TransactionModel { get; set; } = new();

    }

    public class TransactionModel(AccountTracking AccountTracking, Localization localize, IGeneralHelperRepositoryAsync _helper, string language)
    {
        public string? CurrencyType { get; set; } = localize.GetName(AccountTracking.CurrencyType);
        public double DebitAmount { get; set; } = AccountTracking.DebitAmount;
        public double CreditAmount { get; set; } = AccountTracking.CreditAmount;
        public double BalanceAmount { get; set; } = AccountTracking.BalanceAmount;
        public DateTime TransactionDate { get; set; } = AccountTracking.TransactionDate;
        public string Description { get; set; } = AccountTracking.Description ?? string.Empty;
        public string? UserName { get; set; } = _helper.GetUserName(language, AccountTracking.UserId);
        public TrackType trackType { get; set; } = AccountTracking.trackType;
    }

    #endregion
}

