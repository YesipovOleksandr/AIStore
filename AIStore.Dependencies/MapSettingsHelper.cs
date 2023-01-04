using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Settings.ClientConfigs;

namespace AIStore.Dependencies
{
    public static class MapSettingsHelper
    {
        public static void MapSettingsApi(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<AppSettingsApi>(settings =>
            {
                settings.ClientConfig = configuration.GetSection("ClientConfig").Get<ClientConfig>();
            });

            services.Configure<AppSettingsApi>(settings =>
            {
                settings.JWTOptions = configuration.GetSection("JWT").Get<JWTOptions>();
            });

            services.Configure<AppSettingsApi>(settings =>
            {
                settings.AuthenticationsConfig = configuration.GetSection("Authentication").Get<AuthenticationsConfig>();
            });

            services.Configure<AppSettingsApi>(settings =>
            {
                settings.MailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
            });
        }

    }
}