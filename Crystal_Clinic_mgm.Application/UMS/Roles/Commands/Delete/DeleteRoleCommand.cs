using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Delete
{
    public class DeleteRoleCommand : IRequest<JsonResult>
    {
        public int Id { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
