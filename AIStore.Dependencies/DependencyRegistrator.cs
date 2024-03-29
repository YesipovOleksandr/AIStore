﻿using AIStore.Dependencies.DependencyModules;
using Microsoft.Extensions.DependencyInjection;

namespace AIStore.Dependencies
{
    public static class DependencyRegistrator
    {
        public static void RegisterDependencyModules(this IServiceCollection services)
        {
            services.RegisterRoutes();
            services.RegisterServices();
        }
    }
}
