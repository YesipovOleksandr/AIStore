using Microsoft.AspNetCore.Routing;
using AIStore.Domain.Models.Routing;

namespace AIStore.Domain.Abstract.Services
{
    public interface IRoutesMapper
    {
        void MapRoutes(List<RoutesSet> routesCollection, IEndpointRouteBuilder builder);
    }
}
