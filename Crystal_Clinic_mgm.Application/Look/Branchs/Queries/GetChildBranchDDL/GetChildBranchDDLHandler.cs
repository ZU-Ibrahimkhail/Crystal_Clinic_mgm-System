using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetChildDepartmentDDL
{
    public class GetChildBranchDDLHandler : IRequestHandler<GetChildBranchDDLQuery, List<GetDropDownGeneralModel>>
    {

        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GenericRepositoryAsync;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IGeneralHelperRepositoryAsync _helper;
        public GetChildBranchDDLHandler(IGenericRepositoryAsync<ERP_DbContext, Branch> genericRepositoryAsync, ILoggedInUser loggedInUser, IGeneralHelperRepositoryAsync helper)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
            _loggedInUser = loggedInUser;
            _helper = helper;
        }

        public async Task<List<GetDropDownGeneralModel>> Handle(GetChildBranchDDLQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            List<GetDropDownGeneralModel> branchs = new();

            var ChildBranchIds = await _helper.GetChildBranchs(_loggedInUser.BranchId);
            Localization localize = new();
            branchs = await _GenericRepositoryAsync.FindByCondition(x => x.ID == _loggedInUser.BranchId || ChildBranchIds.Contains(x.ID)).Select(x => new GetDropDownGeneralModel
            {
                Name = localize.GetName(language, x),
                Code = x.Code,
                Id = x.ID,

            }).OrderByDescending(x => x.Id).ToListAsync(cancellationToken);

            return branchs;


        }
    }
}
