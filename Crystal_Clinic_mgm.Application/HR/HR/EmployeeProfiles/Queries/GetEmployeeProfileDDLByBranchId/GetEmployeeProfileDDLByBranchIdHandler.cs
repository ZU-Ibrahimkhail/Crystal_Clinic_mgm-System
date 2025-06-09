using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDDLByDepartmentId
{
    public class GetEmployeeProfileDDLByBranchIdHandler : IRequestHandler<GetEmployeeProfileDDLByBranchIdQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        public GetEmployeeProfileDDLByBranchIdHandler(
            IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee)
        {
            _GRepoEmployee = gRepoEmployee;
        }

        public async Task<JsonResult> Handle(GetEmployeeProfileDDLByBranchIdQuery request, CancellationToken cancellationToken)
        {
            Localization localize = new();
            var Language = GeneralHelper.SelectedLanauge(request.Language);

            var Employee = await _GRepoEmployee.FindByCondition(x => !x.IsDeleted && x.BranchId == request.BranchId)
            .Include(x => x.Branch)
            .Select(x => new EmployeeDDLViewModel
            {
                Id = x.ID,
                Name = Language == Constants.Language.English ? $"{x.EnglishFirstName} {x.EnglishSurName}" : $"{x.PashtoFirstName} {x.PashtoSurName}",
                FatherName = Language == Constants.Language.English ? x.EnglishFatherName : x.PashtoFatherName,
                BranchId = request.BranchId,
                Branch = localize.GetName(Language, x.Branch),
                PersonalEmail = x.PersonalEmail,
                HasAccount = x.HasAccount,
            }).ToListAsync(cancellationToken);

            return new JsonResult(Employee);
        }
    }
}