using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionDDL
{
    public class GetPermissionDDLQuery : IRequest<JsonResult>
    {
        public int ApplicationId { get; set; }
    }
}
