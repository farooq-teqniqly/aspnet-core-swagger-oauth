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
            var requiredScopes = new[] {ApiConstants.OAuth.Scope};

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApiConstants.AspNetAuth.PolicyName, policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        var scopes = context.User.Claims
                            .Where(c => c.Type == ApiConstants.OAuth.ScopeClaimType)
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
