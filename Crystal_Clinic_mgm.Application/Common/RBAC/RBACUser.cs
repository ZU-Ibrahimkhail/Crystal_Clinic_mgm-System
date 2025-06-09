using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Common.RBAC
{
    /// <summary>
    /// to check the LoggedIn user permission and roles for authorization
    /// </summary>
    public class RBACUser
    {
        public Guid UserId { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string Username { get; set; } = string.Empty;

        private readonly List<UserRole> UserRoles = new();
        private readonly List<RolePermission> RolePermissions = new();
        public UMS_DbContext _dbContxt;
        public RBACUser(Guid loggedInUserId, UMS_DbContext dbContxt)
        {
            UserId = loggedInUserId;
            _dbContxt = dbContxt;
            GetDatabaseUserRolesPermissions();
        }
        private void GetDatabaseUserRolesPermissions()
        {
            ApplicationUser? _user = _dbContxt.Users.Where(u => u.Id == UserId).FirstOrDefault();
            if (_user != null)
            {
                IsSuperAdmin = _user.IsSuperAdmin;
                foreach (UserRole _userRole in _dbContxt.UserRoles.Where(u => u.UserId == _user.Id).Select(c => c))
                {
                    UserRole _urole = new()
                    {
                        UserId = UserId,
                        RoleId = _userRole.RoleId
                    };
                    foreach (RolePermission _rperm in _dbContxt.RolePermission.Where(r => r.RoleId == _urole.RoleId).Include(x => x.Permission))
                    {
                        RolePermissions.Add(_rperm);
                    }
                    UserRoles.Add(_urole);
                    _dbContxt.SaveChanges();
                }
            }

        }
        public bool HasPermission(string requiredPermission)
        {
            if (IsSuperAdmin)
            {
                return true;
            }
            var Permission = _dbContxt.Permission.Where(p => p.Name == requiredPermission).FirstOrDefault();
            if (Permission == null)
            {
                return false;
            }
            if (Permission.IsGlobal)
            {
                return true;
            }


            return RolePermissions.Any(p => p.PermissionId == Permission.Id);
        }
    }
}
