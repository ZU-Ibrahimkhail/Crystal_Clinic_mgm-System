using MediatR;
using Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetActiveEmplyeeDDLByDepartment;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetAuthoritiesDDL
{
    public class GetAuthoritiesDDLQuery : IRequest<List<GetActiveEmplyeeModel>>
    {
        public string Language { get; set; } = string.Empty;
    }
}
