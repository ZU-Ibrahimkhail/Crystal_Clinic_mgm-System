using Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Queries.GetDetails
{
    public class PermissionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string ActionCategory { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
        public string Method { get; set; } = string.Empty;
        public int ApplicationId { get; set; }
        public string Application { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsGlobal { get; set; }
        public int RolesCount { get; set; }
        public bool Checked { get; set; }
        public List<ListRoleViewModel> Roles { get; set; } = new();

        public static Expression<Func<Permission, UMS_DbContext, PermissionViewModel>> Projection
        {
            get
            {
                return (p, _umsDbContext) => new PermissionViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Controller = p.Controller,
                    Action = p.Action,
                    ApplicationId = p.ApplicationId,
                    ActionCategory = p.ActionCategory,
                    IsDeleted = p.IsDeleted,
                    Description = p.Description,
                    Checked = _umsDbContext.RolePermission.Any(x => x.PermissionId == p.Id && x.IsDeleted == false),
                    Application = p.Application != null ? p.Application.Abbrevation : string.Empty,
                    Method = p.Method,
                    IsGlobal = p.IsGlobal,
                    RolesCount = _umsDbContext.RolePermission.Where(rp => rp.PermissionId == p.Id && rp.IsDeleted == false).Count()
                };
            }
        }
    }
}
