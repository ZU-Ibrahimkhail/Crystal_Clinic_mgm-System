using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileForEdit
{
    public class GetEmployeeProfileForEditQuery : IRequest<JsonResult>
    {
        public int Id { get; set; }
    }
}
