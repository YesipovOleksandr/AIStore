using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStore.Domain.Abstract.Services
{
    public interface IStaticFilesConfigurator
    {
        void ConfigureStaticPaths(IApplicationBuilder app, IWebHostEnvironment env);
    }
}
