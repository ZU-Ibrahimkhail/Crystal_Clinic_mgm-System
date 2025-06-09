using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Namotion.Reflection;
using System.Reflection;

namespace Crystal_Clinic_Mgm.UI.Providers
{

    public static class SwaggerAPIHeade
    {
        public static IServiceCollection AddSwaggerAPIHeader(this IServiceCollection services, IConfiguration configuration)
        {
            #region  swagger code for get token in swagger 
            services.AddSwaggerGen(c =>
            {
                // Add HierarchicalTagsDocumentFilter to the swagger generator
                c.TagActionsBy(api =>
                {
                    // Get the route template of the action
                    //api/Auth/SignIn
                    var routeTemplate = api.ActionDescriptor.DisplayName!.Split('.');//.Split('/').LastOrDefault();
                    // Split the route template by the folder, controller, and action names
                    //var parts = routeTemplate!.Split('/', StringSplitOptions.RemoveEmptyEntries);


                    var folderName = routeTemplate[routeTemplate.Length - 4];

                    // Use the folder and controller names as the tag for the action
                    var controllerName = api.ActionDescriptor.TryGetPropertyValue<string>("ControllerName");

                    // Return a tag name in the format "FolderName/ControllerName/ActionName"
                    return new[] { $"{folderName}.{controllerName}" };
                });


                c.SwaggerDoc("Crystal_Clinic_Mgm",
                    new OpenApiInfo
                    {
                        Title = "Crystal_Clinic Management",
                        Version = "v1",
                        Contact = new OpenApiContact
                        {
                            Name = "Crystal_Clinic_Mgm",
                            Url = new Uri("http://Crystal_Clinic_Mgm.af/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Crystal_Clinic_Mgm",
                            Url = new Uri("http://Crystal_Clinic_Mgm.af/")

                        }
                    });

                c.AddSecurityDefinition("token", new OpenApiSecurityScheme
                {
                    Description = @"<b>JWT Authorization header using the Bearer scheme.</b>" + @"<br><br>
                      Enter your token in the text input below." + @"<br><br>
                      Example: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJVEVtcGxveWVlTmFt'",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
                //--Add for Comments
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullpath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                c.IncludeXmlComments(cmlCommentsFullpath);
                // For Authorization and of the endpoints 
                c.OperationFilter<SecureEndpointAuthRequirementFilter>();

            });

            #endregion

            return services;
        }
    }





}
