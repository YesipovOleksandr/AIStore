using Microsoft.AspNetCore.Routing;

namespace AIStore.Domain.Abstract.Services
{
    public interface IRoutesConfigurator
    {
        void BuildRoutesUsingAIStores(IEndpointRouteBuilder builder);
    }
}
