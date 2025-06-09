using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDDL
{
    public class GetBranchDDLHandler : IRequestHandler<GetBranchDDLQuery, List<GetDropDownGeneralModel>>
    {

        private readonly IGenericRepositoryAsync<ERP_DbContext, Branch> _GenericRepositoryAsync;
        public GetBranchDDLHandler(IGenericRepositoryAsync<ERP_DbContext, Branch> genericRepositoryAsync)
        {
            _GenericRepositoryAsync = genericRepositoryAsync;
        }

        public async Task<List<GetDropDownGeneralModel>> Handle(GetBranchDDLQuery request, CancellationToken cancellationToken)
        {
            string language = GeneralHelper.SelectedLanauge(request.Language);
            List<GetDropDownGeneralModel> branchs = new();
            Localization localize = new();
            if (request.AllowedBranch == null)
            {
                branchs = await _GenericRepositoryAsync.FindByCondition(x => x.IsDeleted == false && x.IsActive/* && x.ID != _loggedInUser.BranchId*/).Select(x => new GetDropDownGeneralModel()
                {
                    Name = localize.GetName(language, x),
                    Code = x.Code,
                    Id = x.ID,
                }).ToListAsync(cancellationToken);
            }
            else
            {
                branchs = await _GenericRepositoryAsync.FindByCondition(x => x.IsDeleted == false && x.IsActive/*&& x.ID != _loggedInUser.BranchId */&& request.AllowedBranch.Contains("," + x.ID + ",")).Select(x => new GetDropDownGeneralModel()
                {
                    Name = localize.GetName(language, x),
                    Code = x.Code,
                    Id = x.ID,
                }).ToListAsync(cancellationToken);
            }
            return branchs;


        }
    }
}
