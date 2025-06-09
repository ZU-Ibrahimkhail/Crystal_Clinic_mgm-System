using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleDetail
{
    public class GetRoleDetailQuery : IRequest<JsonResult>
    {
        public int Id { get; set; }
    }
}
