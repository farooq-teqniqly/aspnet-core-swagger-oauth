using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Extensions;
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
                            Id = SwaggerExtensions.SecurityDefinitionName
                        },
                        UnresolvedReference = true
                    },
                    new[] { $"{_authOptions.Value.ClientIdUri}/{SwaggerExtensions.Scope}" }
                }
            });
        }
    }
}
