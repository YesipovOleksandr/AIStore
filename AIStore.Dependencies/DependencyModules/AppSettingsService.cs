using AIStore.Domain.Models.Settings;
using Microsoft.Extensions.Configuration;
using AIStore.Domain.Abstract.Services;

namespace AIStore.Dependencies.DependencyModules
{
    public class AppSettingsService : IAppSettingsService
    {
        public AppSettings GetSettings { get; }

        public AIStoreSettings AIStoreSettings { get; }


        public AppSettingsService(IConfiguration configuration)
        {
            GetSettings = new AppSettings();
            AIStoreSettings = new AIStoreSettings(configuration);
            configuration.GetSection("GlobalSettings").Bind(GetSettings);

        }
    }
}
