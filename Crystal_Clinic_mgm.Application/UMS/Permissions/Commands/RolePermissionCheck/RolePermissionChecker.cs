using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.UMS.ViewModel;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Reflection;

namespace Crystal_Clinic_Mgm.Application.UMS.Permissions.Commands.Create
{
    public class ApplicationViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }

    public static class RolePermissionChecker
    {
        public static bool IsPermissionExist(string Permission, UMS_DbContext _context)
        {
            return _context.Permission.Where(a => a.Name == Permission).Any();
        }
        public static bool IsRoleExist(string Role, UMS_DbContext _context)
        {
            return _context.ApplicationRoles.Where(a => a.Name == Role).Any();
        }

        public static List<ApplicationViewModel> GetApplication()
        {
            var asm = Assembly.GetEntryAssembly();
            if (asm != null)
            {
                var application = asm.GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.Namespace != null && type.Namespace.Contains("Crystal_Clinic_Mgm.UI.Controllers.")).Select(t => t.Namespace).Distinct().ToList();

                List<ApplicationViewModel> list = new();
                foreach (var data in application)
                {
                    ApplicationViewModel applicationViewModel = new()
                    {
                        DisplayName = data != null ? data.ToString().Replace("Crystal_Clinic_Mgm.UI.Controllers.", "") : string.Empty,
                        Name = data != null ? data.ToString() : string.Empty
                    };
                    list.Add(applicationViewModel);
                }
                return list;
            }
            else
            {
                return new List<ApplicationViewModel>();
            }

        }
        public static List<ControllerViewModel> GetControllers(string ApplicationName)
        {
            var asm = Assembly.GetEntryAssembly();


            var list = asm != null ? asm.GetTypes()
                   .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.Namespace == ApplicationName)
                   .Select(c => new ControllerViewModel
                   {
                       Name = c.Name
                   })
                   .OrderBy(c => c.Name)
                   .ToList() : new List<ControllerViewModel>();
            return list;

        }
        public static List<ControllerActionsViewModel> GetActionOfController(string ControllerName)
        {
            var asm = Assembly.GetEntryAssembly();
            var list = asm?.GetTypes()
                        .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                        .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()).AsQueryable();
            if (ControllerName != null)
            {
                list = list?.Where(c => c.DeclaringType != null && c.DeclaringType.Name == ControllerName);
            }
            var finallist = list?.Select(c => new ControllerActionsViewModel
            {
                Controller = c.DeclaringType != null ? c.DeclaringType.Name : string.Empty,
                Action = c.Name,
                IsGlobal = false,
                Method = GetMethodFromAttributes(string.Join(",", c.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))))

            }).OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return finallist ?? new List<ControllerActionsViewModel>();
        }

        static string GetMethodFromAttributes(string Attribute)
        {
            if (Attribute.Contains("Get"))
            {
                return "Get";
            }
            else if (Attribute.Contains("Post"))
            {
                return "Post";
            }
            else if (Attribute.Contains("Put"))
            {
                return "Put";
            }
            else if (Attribute.Contains("Delete"))
            {
                return "Delete";
            }
            else
            {
                return "UnKnown";
            }
        }



    }



}
