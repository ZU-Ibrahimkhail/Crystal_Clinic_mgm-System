using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails
{
    public class GetPermissionsDetailsQuery : IRequest<JsonResult>
    {
        public int Id { get; set; }
    }
}
