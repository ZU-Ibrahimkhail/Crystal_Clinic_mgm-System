using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.Delete
{
    public class DeleteEmployeeProfileCommand : IRequest<JsonResult>
    {
        public int ID { get; set; }
        public string? Remarks { get; set; }
    }
}
