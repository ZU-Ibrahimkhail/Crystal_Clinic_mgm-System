using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetActiveEmplyeeDDLByDepartment;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetAuthoritiesDDL
{
    public class GetAuthoritiesDDLHandler : IRequestHandler<GetAuthoritiesDDLQuery, List<GetActiveEmplyeeModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _GRepoContractDetails;
        private readonly IGeneralHelperRepositoryAsync _helper;

        public GetAuthoritiesDDLHandler(IGenericRepositoryAsync<ERP_DbContext, ContractDetails> gRepoContractDetails, IGeneralHelperRepositoryAsync helper)
        {
            _GRepoContractDetails = gRepoContractDetails;
            _helper = helper;
        }

        public async Task<List<GetActiveEmplyeeModel>> Handle(GetAuthoritiesDDLQuery request, CancellationToken cancellationToken)
        {


            request.Language = GeneralHelper.SelectedLanauge(request.Language);
            Localization localize = new();

            var records = await _GRepoContractDetails.FindByCondition(x => x.IsActive
                                                                 && !x.IsDeleted
                                                                 && !x.EmployeeProfile!.IsDeleted
                                                                 && !x.PositionTitle!.IsDeleted
                                                                 && x.PositionTitle!.IsActive)
                                                                     .Include(x => x.EmployeeProfile)
                                                                     .Include(x => x.PositionTitle)
                                                                     .Include(x => x.Branch)

                                                                .Select(x => new GetActiveEmplyeeModel
                                                                {
                                                                    EmployeeProfileId = x.EmployeeProfileId,
                                                                    ContractId = x.ID,
                                                                    EmployeeFullName = _helper.GetEmployeeLocaalizeFullName(x.EmployeeProfileId, request.Language),
                                                                    EmployeePosition = localize.GetName(request.Language, x.PositionTitle),
                                                                    BranchId = x.BranchId,
                                                                    BranchName = localize.GetName(request.Language, x.Branch)
                                                                }).ToListAsync(cancellationToken);

            return records;



        }
    }
}
