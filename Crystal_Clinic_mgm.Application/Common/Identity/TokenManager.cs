using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Crystal_Clinic_Mgm.Application.Common.Configuration;

namespace Crystal_Clinic_Mgm.Application.Common.Identity
{
    public class TokenManager
    {
        private readonly IDistributedCache cache;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly JwtConfig jwtConfig;

        public TokenManager(IDistributedCache cache,
                IHttpContextAccessor httpContextAccessor,
                JwtConfig jwtConfig)
        {
            this.cache = cache;
            this.httpContextAccessor = httpContextAccessor;
            this.jwtConfig = jwtConfig;
        }

        public async Task<bool> IsCurrentActiveToken()
        {
            var token = GetCurrentAsync();
            return await IsActiveAsync(token);
        }

        public async Task DeactivateCurrentAsync()
        {
            var token = GetCurrentAsync();
            await DeactivateAsync(token);
        }

        public async Task<bool> IsActiveAsync(string token)
        {
            var key = GetKey(token);
            var value = await cache.GetStringAsync(key);
            return value == null;
        }

        public async Task DeactivateAsync(string token)
        {
            var key = GetKey(token);
            await cache.SetStringAsync(key, "dummy", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(jwtConfig.DaysToExpire)
            });
        }

        private string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor.HttpContext != null ?
                                      httpContextAccessor.HttpContext.Request.Headers["authorization"] : StringValues.Empty;

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.First()?.Split(" ").Last() ?? string.Empty;
        }

        private static string GetKey(string token)
        {
            return $"tokens:{token}:deactivated";
        }
    }
}
