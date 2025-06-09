using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.DashBoards.UserLogInByDepartmentPieChart
{
    public class UserLogInByBranchPieChartDashBoardHandler : IRequestHandler<UserLogInByBranchPieChartQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserAudit> _GrepoUserAudit;
        private readonly IGeneralHelperRepositoryAsync _helper;

        public UserLogInByBranchPieChartDashBoardHandler(
            IGenericRepositoryAsync<UMS_DbContext, UserAudit> grepoUserAudit,
            IGeneralHelperRepositoryAsync helper)
        {
            _GrepoUserAudit = grepoUserAudit;
            _helper = helper;
        }

        public async Task<JsonResult> Handle(UserLogInByBranchPieChartQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            Localization localize = new();

            return await Task.Run(() =>
            {

                var branchlist = _GrepoUserAudit.FindByCondition(x => x.ActionOn >= request.StartDate && x.ActionOn <= request.EndDate)
                    .Include(x => x.User)
                    .GroupBy(x => x.User!.BranchId)
                    .Select(x => new SpiderData()
                    {
                        Label = _helper.GetUserCurrentBranchName(language, x.First().UserId),
                        Value = x.Count()
                    })
                    .ToList();

                return new JsonResult(branchlist);
            });
        }
    }
}
