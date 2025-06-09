using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetPermissionForEdit
{
    public class GetPermissionForEditQuery : IRequest<JsonResult>
    {
        public int Id { get; set; }
    }
}
