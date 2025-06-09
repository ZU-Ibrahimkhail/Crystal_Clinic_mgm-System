using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.FilerPermissions
{
    public class FilterPermissions : IRequest<JsonResult>
    {
        public int ApplicationId { get; set; }
        public string ControllerName { get; set; } = string.Empty;
    }
}
