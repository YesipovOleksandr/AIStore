using AIStore.Domain.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AIStore.Domain.Abstract.Services;

namespace AIStore.Dependencies.DependencyModules
{
    public static class ServicesModule
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IAppSettingsService, AppSettingsService>();
            services.AddHttpContextAccessor();
        }
    }
}
