using MEW_ERP.Persistence.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MEW_ERP.Application.Common.Configuration
{
    public class NetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public NetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid Id => Guid.Parse(_accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public string Name => _accessor.HttpContext?.User.Identity.Name;

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }
}
