using MediatR;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.UserReport.UserRolePermission.Queries
{
    public class UserRolePermissionQuery : IRequest<Microsoft.AspNetCore.Mvc.JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        public Guid? UserID { get; set; } = Guid.Empty;
        public int? BranchId { get; set; } = 0;
        public bool? IsActive { get; set; } = false;
        public bool? IsManager { get; set; } = false;
    }
}
