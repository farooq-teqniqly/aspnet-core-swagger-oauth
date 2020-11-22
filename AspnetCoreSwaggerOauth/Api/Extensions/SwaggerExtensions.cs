using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Api.Filters;
using Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extensions
{
    public static class SwaggerExtensions
    {
        

        public static IServiceCollection AddSwaggerDefaults(this IServiceCollection services, AuthOptions authOptions)
        {
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

                options.AddJwtSecurityDefinition(ApiConstants.OAuth.SecurityDefinitionName, authOptions);
            });

            return services;
        }

        public static void AddJwtSecurityDefinition(
            this SwaggerGenOptions options, 
            string name, 
            AuthOptions authOptions)
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
                            AuthorizationUrl = new Uri(authOptions.AuthUrl),
                            TokenUrl = new Uri(authOptions.TokenUrl),
                            Scopes = ApiConstants.OAuth.SwaggerUIScopes.ToDictionary(s => $"{authOptions.ClientIdUri}/{s}")
                        }
                    }
                });

            options.OperationFilter<OAuthSecurityRequirementsOperationFilter>();
        }

        public static IApplicationBuilder UseSwaggerUIDefaults(this IApplicationBuilder app, AuthOptions authOptions)
        {
            
            app.UseSwagger();

            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                o.OAuthClientId(authOptions.ClientId);
            });

            return app;
        }
    }
}
