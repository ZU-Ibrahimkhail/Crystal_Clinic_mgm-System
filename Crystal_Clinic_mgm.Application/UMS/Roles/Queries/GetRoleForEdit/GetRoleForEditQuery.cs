using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleForEdit
{
    public class GetRoleForEditQuery : IRequest<JsonResult>
    {
        public int Id { get; set; }
    }
}
