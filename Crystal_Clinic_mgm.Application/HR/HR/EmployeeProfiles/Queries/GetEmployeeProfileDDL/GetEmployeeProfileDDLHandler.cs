using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileDDL
{
    public class GetEmployeeProfileDDLHandler : IRequestHandler<GetEmployeeProfileDDLQuery, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly IGeneralHelperRepositoryAsync _helper;
        private readonly ILoggedInUser _loggedInUser;
        public GetEmployeeProfileDDLHandler(IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee, IGeneralHelperRepositoryAsync helper, ILoggedInUser loggedInUser)
        {
            _GRepoEmployee = gRepoEmployee;
            _helper = helper;
            _loggedInUser = loggedInUser;
        }

        public async Task<JsonResult> Handle(GetEmployeeProfileDDLQuery request, CancellationToken cancellationToken)
        {
            Localization localize = new();
            var Language = GeneralHelper.SelectedLanauge(request.Language);
            List<int> childBranchs = await _helper.GetChildBranchs(_loggedInUser.BranchId);
            var Employees = await _GRepoEmployee.FindByCondition(x => !x.IsDeleted).Select(x => new EmployeeDDLViewModel
            {
                Id = x.ID,
                Name = Language == Constants.Language.English ? $"{x.EnglishFirstName} {x.EnglishSurName}" : $"{x.PashtoFirstName} {x.PashtoSurName}",
                FatherName = Language == Constants.Language.English ? x.EnglishFatherName : x.PashtoFatherName,
                BranchId = x.BranchId,
                Branch = localize.GetName(Language, x.Branch),
                PersonalEmail = x.PersonalEmail,
                HasAccount = x.HasAccount,
            }).ToListAsync(cancellationToken);
            if (!request.GetAll)
            {
                Employees = Employees.Where(x => childBranchs.Contains(x.BranchId) || x.BranchId == _loggedInUser.BranchId).ToList();
            }
            return new JsonResult(Employees);
        }
    }
}
