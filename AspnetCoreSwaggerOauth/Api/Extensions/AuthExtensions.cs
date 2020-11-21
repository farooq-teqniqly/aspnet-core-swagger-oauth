using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthDefaults(
            this IServiceCollection services, 
            AuthOptions authOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authOptions.Authority;
                    options.Audience = authOptions.ClientId;
                });

            return services;
        }

        public static IServiceCollection AddAuthorizationDefaults(this IServiceCollection services)
        {
            var requiredScopes = new[] {SwaggerExtensions.Scope};

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthPolicy", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        var scopes = context.User.Claims
                            .Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/scope")
                            .Where(c => requiredScopes.Contains(c.Value))
                            .ToList();

                        return scopes.Any();
                    });
                });
            });

            return services;
        }
    }
}
