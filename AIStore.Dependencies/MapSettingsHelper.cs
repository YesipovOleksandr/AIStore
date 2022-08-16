using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Settings.ClientConfigs;

namespace AIStore.Dependencies
{
    public static class MapSettingsHelper
    {
        public static void MapSettings(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<AppSettings>(settings =>
            {
                settings.ClientConfig = configuration.GetSection("ClientConfig").Get<ClientConfig>();
            });

            services.Configure<AppSettings>(settings =>
            {
                settings.JWTOptions = configuration.GetSection("JWT").Get<JWTOptions>();
            });
        }
    }
}