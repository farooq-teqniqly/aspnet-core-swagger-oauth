using System.Linq;
using Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApiConstants.AspNetAuth.ReadWritePolicyName, policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        var scopes = context.User.Claims
                            .Where(c => c.Type == ApiConstants.OAuth.ScopeClaimType)
                            .Where(c => ApiConstants.OAuth.SwaggerUIScopes.Contains(c.Value))
                            .ToList();

                        return scopes.Any();
                    });
                });

                options.AddPolicy(ApiConstants.AspNetAuth.ReadOnlyPolicyName, policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        var scopes = context.User.Claims
                            .Where(c => c.Type == ApiConstants.OAuth.ScopeClaimType)
                            .Where(c => ApiConstants.OAuth.SwaggerUIScopes.Contains(c.Value))
                            .ToList();

                        return scopes.Any();
                    });
                });
            });

            return services;
        }
    }
}
