using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Crystal_Clinic_Mgm.Persistence.Initializers
{
    public class PermissionInitializer
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async static Task InitializePermissions(UMS_DbContext context)
        {
            await SeedApplication(context);
            await SeedPermission(context);
            await SeedRoles(context);

        }

        #region Application
        public static async Task SeedApplication(UMS_DbContext context)
        {

            var paths = Directory.GetDirectories(@"Controllers/", "*", SearchOption.AllDirectories).ToList();
            List<string> ApplicationDirectories = new();
            foreach (var directory in paths)
            {
                ApplicationDirectories.Add(directory.Split("/").Last().Split("\\").Last().Replace("Controllers", ""));
            }

            var GUID = Guid.NewGuid();
            foreach (var application in ApplicationDirectories)
            {
                var ApplicationExist = context.Application.Any(x => x.Abbrevation == application);
                if (!ApplicationExist)
                {
                    try
                    {
                        context.Application.Add(new Applications
                        {
                            Title = application + " Module",
                            Abbrevation = application,
                            IconClass = "." + application.ToLower(),
                            Description = "Description of " + application,
                            DefaultRoute = "/" + application,
                            IsDeleted = false,
                            Area = application.ToUpper(),
                            CreatedBy = GUID,
                            ModifiedBy = GUID,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now

                        });
                        await context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

            }
            #region Old initualizer Code 

            //var applications = context.Application.ToList();
            //if (applications.Count() == 0)
            //{
            //    try
            //    {
            //        context.Application.Add(new Applications
            //        {
            //            Title = "Document Managment and Tracking System",
            //            Abbrevation = "DMTS",
            //            IconClass = ".dmts",
            //            Description = "Managment and Tracking Documents",
            //            DefaultRoute = "/DMTS",
            //            IsDeleted = false,
            //            Area = "DMTS",
            //            CreatedBy = GUID,
            //            ModifiedBy = GUID,
            //            CreatedOn = DateTime.Now,
            //            ModifiedOn = DateTime.Now

            //        });
            //        await context.SaveChangesAsync();
            //                context.Application.Add(new Applications
            //        {
            //            Title = "User Managment System",
            //            Abbrevation = "UMS",
            //            IconClass = ".ums",
            //            Description = "Managment and Tracking Users",
            //            DefaultRoute = "/UMS",
            //            IsDeleted = false,
            //            Area = "UMS",
            //            CreatedBy = GUID,
            //            ModifiedBy = GUID,
            //            CreatedOn = DateTime.Now,
            //            ModifiedOn = DateTime.Now

            //        });
            //        await context.SaveChangesAsync();
            //                context.Application.Add(new Applications
            //        {
            //            Title = "IT Services Management System",
            //            Abbrevation = "ITSMS",
            //            IconClass = ".itsms",
            //            Description = "Managment of IT services",
            //            DefaultRoute = "/ITSMS",
            //            IsDeleted = false,
            //            Area = "ITSMS",
            //            CreatedBy = GUID,
            //            ModifiedBy = GUID,
            //            CreatedOn = DateTime.Now,
            //            ModifiedOn = DateTime.Now

            //        });
            //        await context.SaveChangesAsync();

            //        context.Application.Add(new Applications
            //        {
            //            Title = "Archive System",
            //            Abbrevation = "ARCH",
            //            IconClass = ".arch",
            //            Description = "Managment of Archive",
            //            DefaultRoute = "/Archive",
            //            IsDeleted = false,
            //            Area = "Archive",
            //            CreatedBy = GUID,
            //            ModifiedBy = GUID,
            //            CreatedOn = DateTime.Now,
            //            ModifiedOn = DateTime.Now

            //        });
            //        await context.SaveChangesAsync();


            //        context.Application.Add(new Applications
            //        {
            //            Title = "Reception System",
            //            Abbrevation = "Reception",
            //            IconClass = ".rec",
            //            Description = "Managment of Reception",
            //            DefaultRoute = "/Reception",
            //            IsDeleted = false,
            //            Area = "Reception",
            //            CreatedBy = GUID,
            //            ModifiedBy = GUID,
            //            CreatedOn = DateTime.Now,
            //            ModifiedOn = DateTime.Now

            //        });
            //        await context.SaveChangesAsync();

            //    }
            //    catch (Exception e)
            //    {
            //        throw;
            //    }

            //}
            #endregion
        }

        #endregion

        #region Roles
        public static async Task SeedRoles(UMS_DbContext context)
        {
            var GUID = Guid.NewGuid();
            var Roles = context.ApplicationRoles.ToList();
            if (Roles.Count == 0)
            {
                try
                {
                    int? appID = context.Application?.FirstOrDefault(x => x.Abbrevation == "DMTS")?.ID;
                    context.ApplicationRoles.Add(new ApplicationRole
                    {
                        ApplicationId = appID != null ? appID.Value : 1,
                        RoleDescription = "CCBranch",
                        Name = "CCBranch",
                        IsDeleted = false,
                        CreatedBy = GUID,
                        ModifiedBy = GUID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now

                    });
                    await context.SaveChangesAsync();



                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

        #endregion

        #region Permission
        public static async Task SeedPermission(UMS_DbContext context)
        {
            var GUID = new Guid();
            var paths = Directory.GetDirectories(@"Controllers/", "*", SearchOption.AllDirectories).ToList();
            List<string> ApplicationDirectories = new();
            foreach (var directory in paths)
            {
                ApplicationDirectories.Add(directory.Split("/").Last().Split("\\").Last().Replace("Controllers", ""));
            }
            var asm = Assembly.GetEntryAssembly();
            foreach (var Directory in ApplicationDirectories)
            {
                var list = asm?.GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.Namespace!.Split(".").Last() == /*"MEW_ERP.UI.Controllers." + */Directory)
                                 .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                                 .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()).AsQueryable();

                // list for Deleting un wanted permissions 
                List<string> listForDelete = list!.Where(m =>
                                                m.CustomAttributes.Any(at => at.AttributeType.Name == "DisableRBACAttribute"))
                                                .Select(x => x.DeclaringType!.Name.Replace("Controller", "-") + x.Name).ToList();

                // filter list with wanted permissions 
                list = list!.Where(m => m.DeclaringType!.CustomAttributes.Any(at => at.AttributeType.Name == "RBACAttribute")
                                        && !m.CustomAttributes.Any(at => at.AttributeType.Name == "DisableRBACAttribute"));
                var finallist = list != null ? list.Select(c => new ControllerActionsViewModel
                {
                    Controller = c.DeclaringType!.Name,
                    IsGlobal = false,
                    Action = c.Name,
                    Method = GetMethodFromAttributes(String.Join(",", c.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))),
                    ActionCategory = "High Category",
                    Description = "Discription",
                }).ToList() : new List<ControllerActionsViewModel>();
                var App = context.Application.FirstOrDefault(x => x.IsDeleted == false && x.Abbrevation == Directory);
                if (App != null)
                {
                    // deleting unwanted permissions
                    var unWantedPermissions = context.Permission.Where(x => x.ApplicationId == App.ID && listForDelete.Contains(x.Name));
                    var unWantedPermissionIds = unWantedPermissions.Select(p => p.Id).ToList();
                    var unWantedRolePermissions = context.RolePermission.Where(x => unWantedPermissionIds.Contains(x.PermissionId));
                    context.RolePermission.RemoveRange(unWantedRolePermissions);
                    context.SaveChanges();

                    context.Permission.RemoveRange(unWantedPermissions);
                    context.SaveChanges();


                    foreach (var item in finallist)
                    {
                        bool isPermissionExist = context.Permission.Any(x => x.Controller == item.Controller && x.Action == item.Action && x.ApplicationId == App.ID);
                        if (!isPermissionExist)
                        {
                            context.Add(new Permission
                            {
                                ApplicationId = App.ID,
                                Controller = item.Controller,
                                Action = item.Action,
                                Method = item.Method,
                                Name = item.Controller.Replace("Controller", "-") + item.Action,
                                ActionCategory = item.ActionCategory,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                CreatedBy = GUID,
                                Description = item.Description,
                            });
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            #region Old initializer Code
            //var permission = context.Permission.ToList();
            //if (permission.Count() == 0)
            //{
            //    try
            //    {

            //        foreach (var application in ApplicationDirectories)
            //        {
            //            var list = asm.GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.Namespace == "MEW_ERP.UI.Controllers." + application)
            //                            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            //                            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()).AsQueryable();

            //            var finallist = list.Select(c => new ControllerActionsViewModel
            //            {
            //                controller = c.DeclaringType.Name,
            //                IsGlobal = false,
            //                action = c.Name,
            //                Method = GetMethodFromAttributes(String.Join(",", c.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))),
            //                ActionCategory = "High Category",
            //                Description = "Discription",
            //            });
            //            var App = context.Application.FirstOrDefault(x => x.IsDeleted == false && x.Abbrevation == application);
            //            if (App != null)
            //            {

            //                foreach (var item in finallist)
            //                {

            //                    context.Add(new Permission
            //                    {
            //                        ApplicationId = App.ID,
            //                        Controller = item.controller,
            //                        Action = item.action,
            //                        Method = item.Method,
            //                        Name = item.controller.Replace("Controller", "-") + item.action,
            //                        ActionCategory = item.ActionCategory,
            //                        CreatedOn = DateTime.Now,
            //                        ModifiedOn = DateTime.Now,
            //                        CreatedBy = GUID,
            //                        Description = item.Description,
            //                    });
            //                    await context.SaveChangesAsync();
            //                }
            //            }
            //        }



            //    }
            //    catch (Exception e)
            //    {

            //        throw;
            //    }
            //}
            #endregion

        }



        #endregion

        #region Methods
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
        #endregion
    }

    #region Controller Action View Model
    public class ControllerActionsViewModel
    {
        public string Controller { get; set; } = string.Empty;
        public bool IsGlobal { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string ActionCategory { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    #endregion
}
