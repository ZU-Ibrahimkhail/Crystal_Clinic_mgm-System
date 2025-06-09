using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentAndChildDepartments
{
    public class GetBranchAndChildBranchHandler : IRequestHandler<GetBranchAndChildBranchQuery, JsonResult>
    {

        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GenericRepositoryAsync;
        private readonly IGeneralHelperRepositoryAsync _helper;

        public GetBranchAndChildBranchHandler(IGenericRepositoryAsync<ERP_DbContext, Branch> genericRepositoryAsync, IGeneralHelperRepositoryAsync helper)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _helper = helper;
        }

        public async Task<JsonResult> Handle(GetBranchAndChildBranchQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            Localization localize = new();
            List<GetBranchAndChildBranchsModel> branchInfo = new();

            #region return Child Branch 
            var ChildBranchIds = await _helper.GetChildBranchs(request.BranchId);

            branchInfo.AddRange(await _GenericRepositoryAsync.FindByCondition(x => !x.IsDeleted && (ChildBranchIds.Contains(x.ID) || x.ID == request.BranchId)).
                Select(branch => new GetBranchAndChildBranchsModel
                {
                    BranchId = branch.ID,
                    BranchName = localize.GetName(language, branch),
                }).ToListAsync());
            return new JsonResult(branchInfo);

            #endregion

        }
    }
}
