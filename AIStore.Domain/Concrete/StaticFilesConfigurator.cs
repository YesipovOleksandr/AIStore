using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using AIStore.Domain.Abstract;

namespace AIStore.Domain.Concrete
{
    public class StaticFilesConfigurator : IStaticFilesConfigurator
    {
        private readonly IRoutesParser _routesParser;

        public StaticFilesConfigurator(IRoutesParser routesParser)
        {
            _routesParser = routesParser;
        }
        public void ConfigureStaticPaths(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var options = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/template"
            };

            env.WebRootFileProvider = new CompositeFileWithOptionsProvider(env.WebRootFileProvider, options);

            app.UseStaticFiles(options);

        }
    }
}
