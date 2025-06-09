using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.UserReport.RolesInfo.Queries
{
    public class GetRoleInfoListQuery : IRequest<JsonResult>
    {
        public string? SearchBy { get; set; }
        public int? ApplicationId { get; set; } = 0;
    }
}
