using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetActiveEmplyeeDDLByDepartment
{
    public class GetActiveEmplyeeDDLByBranchQuery : IRequest<JsonResult>
    {
        public string Language { get; set; } = string.Empty;
        public int BranchId { get; set; }
    }
}
