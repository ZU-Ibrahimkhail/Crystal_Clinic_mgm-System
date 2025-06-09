using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Commands.Create
{
    public class CreateRoleCommand : IRequest<JsonResult>
    {
        public int ApplicationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<int> PermissionIds { get; set; } = new();
    }
}
