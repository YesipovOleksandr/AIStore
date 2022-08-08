using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIStore.Domain.Abstract.Services;

namespace AIStore.Domain.Concrete
{
    public class RoutesConfigurator : IRoutesConfigurator
    {
        private readonly IRoutesMapper _routesMapper;
        private readonly IRoutesParser _routesParser;
        public RoutesConfigurator(IRoutesMapper routesMapper, IRoutesParser routesParser)
        {
            _routesMapper = routesMapper;
            _routesParser = routesParser;
        }

        public void BuildRoutesUsingAIStores(IEndpointRouteBuilder builder)
        {
            _routesMapper.MapRoutes(_routesParser.ParseAIStores(), builder);
        }
    }
}
