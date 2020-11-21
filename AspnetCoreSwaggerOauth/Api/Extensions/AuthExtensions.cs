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
    }
}
