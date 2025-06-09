using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionControllers
{
    public class GetPermissionControllerQuery : IRequest<JsonResult>
    {
        public int ApplicationId { get; set; }
    }
}
