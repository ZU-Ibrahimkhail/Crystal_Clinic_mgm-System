using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.UMS.Roles.Queries.GetRoleList
{
    public class ListRoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Checked { get; set; } = false;
        public bool IsDeleted { get; set; }
        public int ApplicationId { get; set; }
        public string Application { get; set; } = string.Empty;
        public int TotalPermissions { get; set; } = 0;

        public static Expression<Func<ApplicationRole, UMS_DbContext, ListRoleViewModel>> Projection
        {
            get
            {
                return (rp, _umsDbcontext) => new ListRoleViewModel
                {
                    Id = rp.Id,
                    Name = rp.Name ?? string.Empty,
                    Description = rp.RoleDescription,
                    Checked = _umsDbcontext.RolePermission.Any(x => x.RoleId == rp.Id),
                    IsDeleted = rp.IsDeleted,
                    ApplicationId = rp.ApplicationId,
                    Application = rp.Application!.Abbrevation,
                    TotalPermissions = _umsDbcontext.RolePermission.Where(p => p.RoleId == rp.Id).Count()
                };
            }
        }
    }
}
