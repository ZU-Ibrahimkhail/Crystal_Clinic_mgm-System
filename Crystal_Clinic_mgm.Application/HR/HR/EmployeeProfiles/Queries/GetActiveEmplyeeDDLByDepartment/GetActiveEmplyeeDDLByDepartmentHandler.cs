using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetActiveEmplyeeDDLByDepartment
{
    public class GetActiveEmplyeeDDLByBranchHandler : IRequestHandler<GetActiveEmplyeeDDLByBranchQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _GRepoContractDetails;

        public GetActiveEmplyeeDDLByBranchHandler(IGenericRepositoryAsync<ERP_DbContext, ContractDetails> gRepoContractDetails)
        {
            _GRepoContractDetails = gRepoContractDetails;
        }

        public async Task<JsonResult> Handle(GetActiveEmplyeeDDLByBranchQuery request, CancellationToken cancellationToken)
        {

            request.Language = GeneralHelper.SelectedLanauge(request.Language);
            Localization localize = new();
            var records = await _GRepoContractDetails.FindByCondition(x => x.IsActive
                                                                 && !x.IsDeleted
                                                                 && !x.EmployeeProfile!.IsDeleted
                                                                 && x.BranchId == request.BranchId)
                                                                     .Include(x => x.EmployeeProfile)
                                                                     .Include(x => x.PositionTitle)
                                                                     .Include(x => x.Branch)
                                                                     .Select(x => new GetActiveEmplyeeModel
                                                                     {
                                                                         EmployeeProfileId = x.EmployeeProfileId,
                                                                         ContractId = x.ID,
                                                                         EmployeeFullName = $"{x.EmployeeProfile!.PashtoFirstName}\" {x.EmployeeProfile!.PashtoSurName} \" ",
                                                                         EmployeePosition = localize.GetName(request.Language, x.PositionTitle),
                                                                         BranchId = x.BranchId,
                                                                         BranchName = localize.GetName(request.Language, x.Branch)
                                                                     }).ToListAsync(cancellationToken);
            return new JsonResult(records);

        }
    }
}
