using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Crystal_Clinic_Mgm.UI
{
    internal class SecureEndpointAuthRequirementFilter : IOperationFilter
    {
        /*
      This attribute takes the name of the function along with a description and some optional “tags” to be used for 
      categorizing the function like Api response code OK(200), contenty Type(Json), BodyType(string)
      Using OpenAPI operation filters to add security requirements to controller endpoints that 
      require authentication like authorize and allowAnonymous
     */
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription
                .ActionDescriptor
                .EndpointMetadata
                .OfType<AuthorizeAttribute>()
                .Any())
            {
                return;
            }

            operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "token" }
                }] = new List<string>()
            }
        };
        }
    }
}
