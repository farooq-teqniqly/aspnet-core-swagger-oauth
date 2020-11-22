using System.Linq;
using Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Filters
{
    public class OAuthSecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly IOptions<AuthOptions> _authOptions;

        public OAuthSecurityRequirementsOperationFilter(IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApiConstants.OAuth.SecurityDefinitionName
                        },
                        UnresolvedReference = true
                    },
                    ApiConstants.OAuth.SwaggerUIScopes.Select(s => $"{_authOptions.Value.ClientIdUri}/{s}").ToArray()
                }
            });
        }
    }
}
