using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace AIStore.Domain.Abstract
{
    public interface IStaticFilesConfigurator
    {
        void ConfigureStaticPaths(IApplicationBuilder app, IWebHostEnvironment env);
    }
}
