using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Api.Filters;
using Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extensions
{
    public static class SwaggerExtensions
    {
        public const string SecurityDefinitionName = "aad-jwt";
        public const string Scope = "Api.SwaggerUI";

        public static IServiceCollection AddSwaggerDefaults(this IServiceCollection services)
        {
            var authOptions = services.BuildServiceProvider().GetRequiredService<IOptions<AuthOptions>>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Weather API",
                    Version = "1.0",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddJwtSecurityDefinition(SecurityDefinitionName, authOptions);
            });

            return services;
        }

        public static void AddJwtSecurityDefinition(
            this SwaggerGenOptions options, 
            string name, 
            IOptions<AuthOptions> authOptions)
        {
            options.AddSecurityDefinition(
                name,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authOptions.Value.AuthUrl),
                            TokenUrl = new Uri(authOptions.Value.TokenUrl),
                            Scopes = new Dictionary<string, string>
                            {
                                {
                                    $"{authOptions.Value.ClientIdUri}/{Scope}", "Perform API operation using the Swagger UI."
                                }
                            }
                        }
                    }
                });

            options.OperationFilter<OAuthSecurityRequirementsOperationFilter>();
        }

        public static IApplicationBuilder UseSwaggerUIDefaults(this IApplicationBuilder app, IOptions<AuthOptions> authOptions)
        {
            
            app.UseSwagger();

            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                o.OAuthClientId(authOptions.Value.ClientId);
            });

            return app;
        }
    }
}
