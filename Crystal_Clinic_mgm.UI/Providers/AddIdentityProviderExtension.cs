using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Crystal_Clinic_Mgm.Application.Common.Configuration;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using System.Text;
namespace Crystal_Clinic_Mgm.UI.Providers
{
    public static class AddIdentityProviderExtension
    {
        //This is Identity provider which is used for token,role,application  and also for token configuration
        public static IServiceCollection AddIdentityProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtConfig = new JwtConfig();
            configuration.Bind("JwtConfig", jwtConfig);

            services.AddSingleton(jwtConfig);
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(conf =>
            {
                conf.Password.RequiredLength = 8;
                conf.Password.RequireDigit = true;
                conf.Password.RequireNonAlphanumeric = true;
                conf.Password.RequireUppercase = false;
                conf.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<UMS_DbContext>()
           .AddDefaultTokenProviders();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
                };
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        // Fallback to JWT_Token for backward compatibility
                        if (string.IsNullOrEmpty(accessToken))
                        {
                            accessToken = context.Request.Query["JWT_Token"];
                        }
                        //  var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/signalr"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization();
            //services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
