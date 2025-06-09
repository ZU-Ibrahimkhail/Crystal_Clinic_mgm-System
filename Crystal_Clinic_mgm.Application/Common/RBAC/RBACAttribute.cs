using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.LocalizeMessage;
using Crystal_Clinic_Mgm.Common.UMSLocalizations.ValidationMessageLocalization;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.Common.RBAC
{
    public class RBACAttribute : TypeFilterAttribute
    {
        public RBACAttribute() : base(typeof(RBACAttributeImplementation))
        {
        }

        private class RBACAttributeImplementation : IActionFilter
        {
            private readonly ILoggedInUser _loggedInUser;
            private readonly UMS_DbContext _dbContxt;
            private readonly TrackingTable _trakingTable;
            readonly UMSLocalizeMessage umslocalizeMessage;
            public RBACAttributeImplementation(UMS_DbContext context, ILoggedInUser loggedInUser, IStringLocalizer<UMSValidationResource> localizer)
            {
                _dbContxt = context;
                _loggedInUser = loggedInUser;
                _trakingTable = new TrackingTable
                {
                    RequestTime = DateTime.Now
                };
                umslocalizeMessage = new(localizer);
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }

            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.ActionDescriptor.EndpointMetadata.OfType<DisableRBACAttribute>().FirstOrDefault() != null)
                {
                    // RBAC enforcement is disabled for this action
                    //_trakingTable.IsAccessed = true;
                    //_dbContxt.TrakingTable.Add(_trakingTable);
                    //_dbContxt.SaveChanges();
                    return;
                }
                string Host = _loggedInUser.Host;
                string Path = _loggedInUser.Path;
                string URL = "";

                if (string.IsNullOrWhiteSpace(Path) || string.IsNullOrWhiteSpace(Host))
                {
                    URL = "Not set URL Please check your Connection or Code!";
                }
                else
                {
                    URL = Host + Path;
                }

                void InvalidAccess(string reason)
                {
                    try
                    {
                        _trakingTable.IsAccessed = false;
                        _dbContxt.TrakingTable.Add(_trakingTable);
                        _dbContxt.SaveChanges();
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "InvalidAccess" }, { "controller", "Auth" }, { "area", "" }, { "reason", reason } });
                    }
                    catch (Exception)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "InvalidAccess" }, { "controller", "Auth" }, { "area", "" }, { "reason", reason } });
                    }
                }

                string requiredPermission = String.Format("{0}-{1}",
                    filterContext.ActionDescriptor.RouteValues["controller"]?.ToString(),
                    filterContext.ActionDescriptor.RouteValues["action"]);

                var loggedInUser = filterContext.HttpContext.User;

                if (loggedInUser.Identity?.IsAuthenticated == true)
                {
                    var loggedInUserObj = loggedInUser.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value?.ToString();
                    var UserAuditTableId = loggedInUser.Claims.FirstOrDefault(x => x.Type == "UserAuditTableId")?.Value?.ToString();
                    Guid loggedInUserId = loggedInUserObj != null ? Guid.Parse(loggedInUserObj) : new Guid();

                    _trakingTable.UserAuditId = Math.Abs(Convert.ToInt32(UserAuditTableId));
                    _trakingTable.UserId = loggedInUserId;
                    _trakingTable.Url = URL;
                    _trakingTable.ActionName = filterContext.ActionDescriptor.RouteValues["action"]?.ToString() ?? string.Empty;



                    RBACUser requestingUser = new(loggedInUserId, _dbContxt);
                    var result = requestingUser.HasPermission(requiredPermission);
                    if (!result)
                    {
                        string ErrorDetails = string.Format("{0} {1}", umslocalizeMessage.NoPermission, requiredPermission);
                        _trakingTable.ErrorDetails = ErrorDetails;
                        InvalidAccess(ErrorDetails);
                    }
                    else
                    {
                        _trakingTable.IsAccessed = true;
                        _dbContxt.TrakingTable.Add(_trakingTable);
                        _dbContxt.SaveChanges();
                    }
                }
                else
                {
                    _trakingTable.ErrorDetails = umslocalizeMessage.NotAuthenticated;
                    _trakingTable.IsAccessed = false;
                    _trakingTable.ActionName = filterContext.ActionDescriptor.RouteValues["action"]?.ToString() ?? string.Empty;
                    _dbContxt.TrakingTable.Add(_trakingTable);
                    _dbContxt.SaveChanges();

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", umslocalizeMessage.NotAuthenticated }, { "controller", "Auth" }, { "area", "" } });
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DisableRBACAttribute : Attribute
    {
        // Used in RBAC Attribute , if it's not null then RBAC won't check the permissions
    }
}
