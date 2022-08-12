using Microsoft.AspNetCore.Routing;
using AIStore.Domain.Abstract;

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
