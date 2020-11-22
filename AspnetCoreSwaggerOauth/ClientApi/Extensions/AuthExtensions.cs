using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace ClientApi.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthDefaults(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(config)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddDownstreamWebApi("WeatherApi", config.GetSection("WeatherApi"))
                .AddInMemoryTokenCaches();

            return services;
        }
    }
}
