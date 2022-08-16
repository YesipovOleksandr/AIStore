using Microsoft.Extensions.DependencyInjection;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Abstract.Repository;
using AIStore.DAL.Repository;
using AIStore.BLL.Services;

namespace AIStore.Dependencies.DependencyModules
{
    public static class ServicesModule
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
