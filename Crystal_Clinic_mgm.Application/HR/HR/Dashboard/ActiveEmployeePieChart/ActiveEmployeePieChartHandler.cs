using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.Dashboard.ActiveEmployeePieChart
{
    public class ActiveEmployeePieChartHandler : IRequestHandler<ActiveEmployeePieChartQuery, List<SpiderData>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _repository;
        private readonly IGeneralHelperRepositoryAsync _helper;
        private readonly ILoggedInUser _loggedInUser;

        public ActiveEmployeePieChartHandler(
            IGenericRepositoryAsync<ERP_DbContext, ContractDetails> repository,
            IGeneralHelperRepositoryAsync helper,
            ILoggedInUser loggedInUser)
        {
            _repository = repository;
            _helper = helper;
            _loggedInUser = loggedInUser;
        }

        public async Task<List<SpiderData>> Handle(ActiveEmployeePieChartQuery request, CancellationToken cancellationToken)
        {
            var childeDepartmenIds = await _helper.GetChildBranchs(_loggedInUser.BranchId).ConfigureAwait(false);

            string language = GeneralHelper.SelectedLanauge(request.Language);

            Localization localize = new();
            var totaleActiveEmployee = await _repository.FindByCondition(x => x.IsActive == true
            && (childeDepartmenIds.Contains(x.BranchId) || x.BranchId == _loggedInUser.BranchId))
                                                  .Include(x => x.Branch)
                                                  .GroupBy(x => x.BranchId)
                                                  .Select(x => new SpiderData()
                                                  {
                                                      Label = localize.GetName(language, x.First().Branch),
                                                      Value = x.Count()
                                                  }).ToListAsync(cancellationToken).ConfigureAwait(false);
            return totaleActiveEmployee;

        }
    }
}
